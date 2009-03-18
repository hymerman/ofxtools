using System;
using System.Xml;
using System.Collections.Generic;

class Transaction
{
    public Transaction(int amountPence, DateTime date, string name, string type, string memo)
    {
        this.amountPence = amountPence;
        this.date = date;
        this.name = name;
        this.type = type;
        this.memo = memo;
    }

    public string hash()
    {
        string mungedTransaction = string.Format("{0}{1}{2}{3}{4}", amountPence, date, name, memo, type);
        byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(mungedTransaction);
        byte[] hash = System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(bytes);
        return byteArrayToString(hash);
    }

    private static string byteArrayToString(byte[] arrInput)
    {
        int i;
        System.Text.StringBuilder sOutput = new System.Text.StringBuilder(arrInput.Length);
        for (i = 0; i < arrInput.Length; i++)
        {
            sOutput.Append(arrInput[i].ToString("X2"));
        }
        return sOutput.ToString();
    }

    public int amountPence;
    public DateTime date;
    public string name;
    public string type;
    public string memo;
}

class Converter
{
    static void Main(string[] args)
    {
        foreach(string arg in args)
        {
            convertHSBCHTMLFileToOFX(arg);
        }
    }

    static void convertHSBCHTMLFileToOFX(string path)
    {
        System.IO.FileInfo inputFileInfo = new System.IO.FileInfo(path);
        string outputDirectory = inputFileInfo.DirectoryName;

        HtmlAgilityPack.HtmlDocument brokenDocument = new HtmlAgilityPack.HtmlDocument();
        brokenDocument.Load(path);
        brokenDocument.OptionOutputAsXml = true;
        string fixedXmlFileName = path + ".fixed.xml";
        brokenDocument.Save(fixedXmlFileName);
        XmlDocument document = new XmlDocument();
        document.Load(fixedXmlFileName);

        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(document.NameTable);
        namespaceManager.AddNamespace("d", "http://www.w3.org/1999/xhtml");

        XmlNode closingBalanceNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[last()]/d:td[6]/d:p", namespaceManager);
        int closingBalance = moneyInPenceFromString(closingBalanceNode.InnerText.Trim());

        XmlNode endDateNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:div[@class='extPibRow hsbcRow']/d:div[@class='hsbcPadding']/d:div[@class='hsbcTextRight']", namespaceManager);
        string endDateString = HtmlAgilityPack.HtmlEntity.DeEntitize(endDateNode.InnerText).Trim();

        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
        DateTime endDate = DateTime.ParseExact(endDateString, "dd MMM yyyy", provider);

        XmlNode startDateNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[1]/d:td[1]/d:p", namespaceManager);
        string startDateString = HtmlAgilityPack.HtmlEntity.DeEntitize(startDateNode.InnerText).Trim();

        DateTime startDate = dateFromDateStringFixedUsingUpperBoundDate(startDateString, endDate).AddDays(1);

        List<Transaction> transactions = new List<Transaction>();

        XmlNodeList transactionNodes = document.SelectNodes("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[position()>1 and position()<last()]", namespaceManager);
        foreach (XmlNode node in transactionNodes)
        {
            XmlNode dateNode = node.SelectSingleNode("d:td[1]/d:p", namespaceManager);
            XmlNode typeNode = node.SelectSingleNode("d:td[2]/d:p", namespaceManager);
            XmlNode nameNode = node.SelectSingleNode("d:td[3]/d:p", namespaceManager);
            XmlNode moneyOutNode = node.SelectSingleNode("d:td[4]/d:p", namespaceManager);
            XmlNode moneyInNode = node.SelectSingleNode("d:td[5]/d:p", namespaceManager);

            string date = HtmlAgilityPack.HtmlEntity.DeEntitize(dateNode.InnerText).Trim();
            string type = HtmlAgilityPack.HtmlEntity.DeEntitize(convertTransactionTypeHSBCToOFX(typeNode.InnerText).Trim());
            string name = HtmlAgilityPack.HtmlEntity.DeEntitize(getInnerTextIgnoringLinks(nameNode));
            string moneyIn = HtmlAgilityPack.HtmlEntity.DeEntitize(moneyInNode.InnerText).Trim();
            string moneyOut = "-" + HtmlAgilityPack.HtmlEntity.DeEntitize(moneyOutNode.InnerText).Trim();
            string money = moneyIn == "" ? moneyOut : moneyIn; // todo: don't know if this is right

            // todo: figure out how to get the memo
            transactions.Add(new Transaction(moneyInPenceFromString(money), dateFromDateStringFixedUsingUpperBoundDate(date, endDate), name, type, ""));
        }

        XmlNode accountNumberNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:div[@class='extRowButton extPibRow hsbcRow']/d:div[@class='hsbcPadding']/d:div[@class='hsbcActiveAccount']/d:div[@class='hsbcAccountName']/d:div[@class='hsbcAccountNumber']", namespaceManager);
        string accountNumber = HtmlAgilityPack.HtmlEntity.DeEntitize(accountNumberNode.InnerText).Trim().Replace(" ", "").Replace("-", "");
        string bankNumber = accountNumber.Substring(0, 6);

        string outputFileName = string.Format("{0} - {1}.ofx", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
        System.IO.StreamWriter outputFile = new System.IO.StreamWriter(outputDirectory + "/" + outputFileName);

        outputFile.WriteLine("OFXHEADER:100");
        outputFile.WriteLine("DATA:OFXSGML");
        outputFile.WriteLine("VERSION:102");
        outputFile.WriteLine("SECURITY:NONE");
        outputFile.WriteLine("ENCODING:USASCII"); // may need to change
        outputFile.WriteLine("CHARSET:1252"); // may need to change
        outputFile.WriteLine("COMPRESSION:NONE");
        outputFile.WriteLine("OLDFILEUID:NONE");
        outputFile.WriteLine("NEWFILEUID:NONE");

        outputFile.WriteLine();

        outputFile.WriteLine("<OFX>");
        outputFile.WriteLine("<SIGNONMSGSRSV1>");
        outputFile.WriteLine("<SONRS>");
        outputFile.WriteLine("<STATUS>");
        outputFile.WriteLine("<CODE>0</CODE>");
        outputFile.WriteLine("<SEVERITY>INFO</SEVERITY>");
        outputFile.WriteLine("</STATUS>");
        outputFile.WriteLine("<DTSERVER>{0}</DTSERVER>", DateTime.Now.ToString("yyyyMMddHHmmss"));
        outputFile.WriteLine("<LANGUAGE>ENG</LANGUAGE>"); // change for internationalisation
        outputFile.WriteLine("</SONRS>");
        outputFile.WriteLine("</SIGNONMSGSRSV1>");
        outputFile.WriteLine("<BANKMSGSRSV1>");
        outputFile.WriteLine("<STMTTRNRS>");
        outputFile.WriteLine("<TRNUID>0</TRNUID>");
        outputFile.WriteLine("<STATUS>");
        outputFile.WriteLine("<CODE>0</CODE>");
        outputFile.WriteLine("<SEVERITY>INFO</SEVERITY>");
        outputFile.WriteLine("</STATUS>");
        outputFile.WriteLine("<STMTRS>");
        outputFile.WriteLine("<CURDEF>GBP</CURDEF>"); // change for internationalisation

        outputFile.WriteLine("<BANKACCTFROM>");
        outputFile.WriteLine("<BANKID>{0}</BANKID>", bankNumber);
        outputFile.WriteLine("<ACCTID>{0}</ACCTID>", accountNumber);
        outputFile.WriteLine("<ACCTTYPE>CHECKING</ACCTTYPE>");
        outputFile.WriteLine("</BANKACCTFROM>");

        outputFile.WriteLine("<BANKTRANLIST>");

        outputFile.WriteLine("<DTSTART>{0}</DTSTART>", startDate.ToString("yyyyMMdd"));
        outputFile.WriteLine("<DTEND>{0}</DTEND>", endDate.ToString("yyyyMMdd"));

        foreach(Transaction transaction in transactions)
        {
            outputFile.WriteLine("<STMTTRN>");
            outputFile.WriteLine("<TRNTYPE>{0}</TRNTYPE>", transaction.type);
            outputFile.WriteLine("<DTPOSTED>{0}</DTPOSTED>", transaction.date.ToString("yyyyMMdd"));
            outputFile.WriteLine("<TRNAMT>{0}</TRNAMT>", formatAsPoundsAndPenceString(transaction.amountPence));
            outputFile.WriteLine("<FITID>{0}</FITID>", transaction.hash());
            outputFile.WriteLine("<NAME>{0}</NAME>", transaction.name);
            outputFile.WriteLine("<MEMO>{0}</MEMO>", transaction.memo);
            outputFile.WriteLine("</STMTTRN>");
        }

        outputFile.WriteLine("</BANKTRANLIST>");

        outputFile.WriteLine("<LEDGERBAL>");
        outputFile.WriteLine("<BALAMT>{0}</BALAMT>", formatAsPoundsAndPenceString(closingBalance));
        outputFile.WriteLine("<DTASOF>{0}</DTASOF>", endDate.ToString("yyyyMMdd"));
        outputFile.WriteLine("</LEDGERBAL>");

        outputFile.WriteLine("</STMTRS>");
        outputFile.WriteLine("</STMTTRNRS>");
        outputFile.WriteLine("</BANKMSGSRSV1>");
        outputFile.WriteLine("</OFX>");
        
        outputFile.Close();

        // remove the temporary fixed file
        System.IO.File.Delete(fixedXmlFileName);
    }

    private static DateTime dateFromDateStringFixedUsingUpperBoundDate(string date, DateTime endDate)
    {
        // Add the end date's year onto the string
        string potentialDateString = date + " " + endDate.Year.ToString();

        // Parse this string into a date
        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
        DateTime potentialDate = DateTime.ParseExact(potentialDateString, "dd MMM yyyy", provider);

        // if this date comes after the end date of the statement, make it the year before
        if (potentialDate > endDate) potentialDate = potentialDate.AddYears(-1);

        return potentialDate;
    }

    private static string convertTransactionTypeHSBCToOFX(string HSBCType)
    {
        switch (HSBCType)
        {
            case "ATM": return "ATM";
            case "BP": return "DEBIT"; // ? Not necessarily electronic payment, but will be a debit
            case "CHQ": return "CHECK";
            case "CIR": return "PAYMENT";
            case "CR": return "CREDIT";
            case "DD": return "DIRECTDEBIT";
            case "DIV": return "DIV";
            case "DR": return "DEBIT";
            case "MAE": return "OTHER"; // ? could be debit or credit
            case "SO": return "REPEATPMT";
            case "SOL": return "OTHER"; // ? could be debit or credit
            case "SWT": return "OTHER"; // ? could be debit or credit - old school, not listed on current HSBC website but still present in previous statements
            case "TRF": return "XFER";
            default: return "OTHER";
        }
    }

    private static string getInnerTextIgnoringLinks(XmlNode node)
    {
        if (node.HasChildNodes)
        {
            return node.ChildNodes[0].InnerText.Trim();
        }
        else
        {
            return node.InnerText.Trim();
        }
    }

    private static string formatAsPoundsAndPenceString(int value)
    {
        int pounds = value / 100;
        int pence = Math.Abs(value % 100);
        return string.Format("{0}.{1}", pounds, pence);
    }

    private static int moneyInPenceFromString(string moneyAsString)
    {
        return int.Parse(moneyAsString.Replace(".", ""));        
    }
}
