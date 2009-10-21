using System;
using System.Xml;
using System.Collections.Generic;

class Transaction
{
    public Transaction(int amountPence, DateTime date, string description, string note, string category)
    {
        this.amountPence = amountPence;
        this.date = date;
        this.description = description;
        this.note = note;
        this.category = category;
    }

    public int amountPence;
    public DateTime date;
    public string description;
    public string note;
    public string category;
}

class Converter
{
    static void Main(string[] args)
    {
        string inputPath = args[0];
        System.IO.FileInfo inputFileInfo = new System.IO.FileInfo(inputPath);
        string outputDirectory = inputFileInfo.DirectoryName;

        XmlDocument document = new XmlDocument();
        document.Load(args[0]);

        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(document.NameTable);
        namespaceManager.AddNamespace("d", "http://www.w3.org/1999/xhtml");

        XmlNode closingBalanceNode = document.SelectSingleNode("/d:html/d:body/d:form[1]/d:div[@id='page']/d:div[@id='area2']/d:table[@id='tblTransactionsTable']/d:tfoot/d:tr[1]/d:td[@class='money']", namespaceManager);
        int closingBalance = moneyInPenceFromString(closingBalanceNode.InnerText);

        List<Transaction> transactions = new List<Transaction>();

        XmlNodeList transactionNodes = document.SelectNodes("/d:html/d:body/d:form[1]/d:div[@id='page']/d:div[@id='area2']/d:table[@id='tblTransactionsTable']/d:tbody/d:tr[position()>1]", namespaceManager);
        foreach (XmlNode node in transactionNodes)
        {
            XmlNode dateNode = node.SelectSingleNode("d:td[@class='date']", namespaceManager);
            XmlNode descriptionNode = node.SelectSingleNode("d:td[@class='description']", namespaceManager);
            XmlNode categoryNode = node.SelectSingleNode("d:td[@class='category']", namespaceManager);
            XmlNode moneyNode = node.SelectSingleNode("d:td[@class='money']", namespaceManager);

            string date = dateNode.InnerText;
            string description = descriptionNode.InnerText;
            string category = categoryNode.InnerText;
            string money = moneyNode.InnerText;

            // This check put in place because sometimes (e.g. for foreign currency transactions) a transaction row is used just for extra details (e.g. exchange rate).
            // todo: Ideally we should capture this information too, but I can't be arsed right this second.
            if (date != "" && money != "")
            {
                transactions.Add(new Transaction(moneyInPenceFromString(money), dateFromDateString(date), descriptionFromLongDescription(description), noteFromLongDescription(description), category));
            }
        }

        XmlNode statementDateNode = document.SelectSingleNode("/d:html/d:body/d:form[1]/d:div[@id='page']/d:div[@id='area2']/d:div[@class='floatleft staticdatablock equalheight narrow ']/d:fieldset/d:div[1]/d:select/d:option[@selected='selected']", namespaceManager);

        string[] parts = statementDateNode.InnerText.Split(new string[] {" to "}, StringSplitOptions.None);

        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
        DateTime statementStart = DateTime.ParseExact(parts[0].Trim(), "dd MMMM yyyy", provider);
        DateTime statementEnd = DateTime.ParseExact(parts[1].Trim(), "dd MMMM yyyy", provider).AddDays(-1);

        XmlNode accountNumberNode = document.SelectSingleNode("/d:html/d:body/d:form[1]/d:div[@id='page']/d:div[@id='area2']/d:div[1]/d:div[2]/d:span[@id='lblCardNumber']", namespaceManager);
        long accountNumber = long.Parse(accountNumberNode.InnerText.Replace(" ", ""));

        string outputFileName = string.Format("{0} - {1}.ofx", statementStart.ToString("yyyy-MM-dd"), statementEnd.ToString("yyyy-MM-dd"));
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
        outputFile.WriteLine("<INTU.BID>01267"); // change for internationalisation
        outputFile.WriteLine("</SONRS>");
        outputFile.WriteLine("</SIGNONMSGSRSV1>");
        outputFile.WriteLine("<CREDITCARDMSGSRSV1>");
        outputFile.WriteLine("<CCSTMTTRNRS>");
        outputFile.WriteLine("<TRNUID>0</TRNUID>");
        outputFile.WriteLine("<STATUS>");
        outputFile.WriteLine("<CODE>0</CODE>");
        outputFile.WriteLine("<SEVERITY>INFO</SEVERITY>");
        outputFile.WriteLine("</STATUS>");
        outputFile.WriteLine("<CCSTMTRS>");
        outputFile.WriteLine("<CURDEF>GBP</CURDEF>"); // change for internationalisation

        outputFile.WriteLine("<CCACCTFROM>"); // may need to change element name to credit card type
        outputFile.WriteLine("<ACCTID>{0}</ACCTID>", accountNumber);
        outputFile.WriteLine("</CCACCTFROM>");

        outputFile.WriteLine("<BANKTRANLIST>");

        outputFile.WriteLine("<DTSTART>{0}</DTSTART>", statementStart.ToString("yyyyMMdd"));
        outputFile.WriteLine("<DTEND>{0}</DTEND>", statementEnd.ToString("yyyyMMdd"));

        foreach(Transaction transaction in transactions)
        {
            outputFile.WriteLine("<STMTTRN>");
            outputFile.WriteLine("<TRNTYPE>{0}</TRNTYPE>", creditOrDebitStringFromValue(transaction.amountPence));
            outputFile.WriteLine("<DTPOSTED>{0}</DTPOSTED>", transaction.date.ToString("yyyyMMdd"));
            outputFile.WriteLine("<TRNAMT>{0}</TRNAMT>", formatAsPoundsAndPenceString(transaction.amountPence));
            outputFile.WriteLine("<FITID>{0}</FITID>", hashTransaction(transaction));
            outputFile.WriteLine("<NAME>{0}</NAME>", transaction.description);
            outputFile.WriteLine("<MEMO>{0}</MEMO>", transaction.note);
            outputFile.WriteLine("</STMTTRN>");
        }

        outputFile.WriteLine("</BANKTRANLIST>");

        outputFile.WriteLine("<LEDGERBAL>");
        outputFile.WriteLine("<BALAMT>{0}</BALAMT>", formatAsPoundsAndPenceString(closingBalance));
        outputFile.WriteLine("<DTASOF>{0}</DTASOF>", statementEnd.ToString("yyyyMMdd"));
        outputFile.WriteLine("</LEDGERBAL>");

        outputFile.WriteLine("</CCSTMTRS>");
        outputFile.WriteLine("</CCSTMTTRNRS>");
        outputFile.WriteLine("</CREDITCARDMSGSRSV1>");
        outputFile.WriteLine("</OFX>");
        
        outputFile.Close();
    }

    private static string hashTransaction(Transaction transaction)
    {
        string mungedTransaction = string.Format("{0}{1}{2}{3}", transaction.amountPence, transaction.date, transaction.description, transaction.note);
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

    private static string formatAsPoundsAndPenceString(int value)
    {
        int pounds = value / 100;
        int pence = Math.Abs(value % 100);
        return string.Format("{0}.{1:00}", pounds, pence);
    }

    private static string creditOrDebitStringFromValue(int value)
    {
        if (value < 0) return "DEBIT";
        else return "CREDIT";
    }

    private static int moneyInPenceFromString(string moneyAsString)
    {
        string[] parts = moneyAsString.Substring(1).Split(' ');
        string[] partsOfMoney = parts[0].Split('.');

        int pounds = Math.Abs(int.Parse(partsOfMoney[0]));
        int pence = int.Parse(partsOfMoney[1]);

        int money = pounds * 100 + pence;

        if (parts.Length > 1)
        {
            bool isDebit = parts[1].Equals("DR");
            if (isDebit) money = -money;
        }

        return money;        
    }

    private static string descriptionFromLongDescription(string longDescription)
    {
        return longDescription.Substring(0,23).Trim();
    }

    private static string noteFromLongDescription(string longDescription)
    {
        return longDescription.Substring(24, 13).Trim();
    }

    private static string countryFromLongDescription(string longDescription)
    {
        return longDescription.Substring(38, 2).Trim();
    }

    private static DateTime dateFromDateString(string dateString)
    {
        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
        return DateTime.ParseExact(dateString, "dd MMM yyyy", provider);
    }
}
