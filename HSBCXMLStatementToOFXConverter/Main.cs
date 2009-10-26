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

        Ofx.Document ofxDocument = new Ofx.Document();

        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(document.NameTable);
        namespaceManager.AddNamespace("d", "http://www.w3.org/1999/xhtml");

        XmlNode closingBalanceNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[last()]/d:td[6]/d:p", namespaceManager);
        XmlNode closingBalanceSignNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[last()]/d:td[7]/d:p", namespaceManager);
        int closingBalance = moneyInPenceFromString(closingBalanceNode.InnerText.Trim());
        if (closingBalanceSignNode.InnerText.Trim() == "D") closingBalance = -closingBalance;
        ofxDocument.closingBalance = closingBalance;

        XmlNode endDateNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:div[@class='extPibRow hsbcRow']/d:div[@class='hsbcPadding']/d:div[@class='hsbcTextRight']", namespaceManager);
        string endDateString = HtmlAgilityPack.HtmlEntity.DeEntitize(endDateNode.InnerText).Trim();

        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
        DateTime endDate = DateTime.ParseExact(endDateString, "dd MMM yyyy", provider);
        ofxDocument.endDate = endDate;

        XmlNode startDateNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[1]/d:td[1]/d:p", namespaceManager);
        string startDateString = HtmlAgilityPack.HtmlEntity.DeEntitize(startDateNode.InnerText).Trim();

        DateTime startDate = dateFromDateStringFixedUsingUpperBoundDate(startDateString, endDate.AddDays(-1)).AddDays(1);
        ofxDocument.startDate = startDate;

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
            ofxDocument.addTransaction(moneyInPenceFromString(money), dateFromDateStringFixedUsingUpperBoundDate(date, endDate), name, type, null);
        }

        XmlNode accountNumberNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='outerwrap']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='extVariableContentContainer']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:div[@class='extRowButton extPibRow hsbcRow']/d:div[@class='hsbcPadding']/d:div[@class='hsbcActiveAccount']/d:div[@class='hsbcAccountName']/d:div[@class='hsbcAccountNumber']", namespaceManager);
        string accountNumber = HtmlAgilityPack.HtmlEntity.DeEntitize(accountNumberNode.InnerText).Trim().Replace(" ", "").Replace("-", "");
        ofxDocument.accountNumber = accountNumber;

        string bankNumber = accountNumber.Substring(0, 6);
        ofxDocument.bankNumber = bankNumber;

        string outputFileName = string.Format("{0} - {1}.ofx", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

        ofxDocument.Save(outputDirectory + "/" + outputFileName);

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
        return string.Format("{0}.{1:00}", pounds, pence);
    }

    private static int moneyInPenceFromString(string moneyAsString)
    {
        string[] parts = moneyAsString.Split('.');
        int pounds = int.Parse(parts[0]);
        int pence = int.Parse(parts[1]);
        int value = pounds * 100;

        if (pounds < 0)
        {
            value -= pence;
        }
        else
        {
            value += pence;
        }

        return value;
    }
}
