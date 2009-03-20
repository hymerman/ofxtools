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
        foreach (string arg in args)
        {
            convertINGDirectHTMLFileToOFX(arg);
        }
    }

    static void convertINGDirectHTMLFileToOFX(string path)
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

        XmlNode closingBalanceNode = document.SelectSingleNode("/span/html/body/center/table/tr/td/table/tr/td[2]/table[@summary='Main Table']/tr[5]/td[3]/table[@summary='Transactional Data']/tr[2]/td[4]");
        int closingBalance = moneyInPenceFromString(HtmlAgilityPack.HtmlEntity.DeEntitize(closingBalanceNode.InnerText).Trim().Replace(",", ""));

        XmlNode periodNode = document.SelectSingleNode("/span/html/body/center/table/tr/td/table/tr/td[2]/table[@summary='Main Table']/tr[5]/td[3]/table[@summary='Content Area']/tr[3]/td[4]");
        string[] parts = HtmlAgilityPack.HtmlEntity.DeEntitize(periodNode.InnerText).Split(new string[] {"to"}, StringSplitOptions.None);
        DateTime startDate = DateTime.ParseExact(parts[0].Trim(), "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
        DateTime endDate = DateTime.ParseExact(parts[1].Trim(), "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);

        List<Transaction> transactions = new List<Transaction>();

        XmlNodeList transactionNodes = document.SelectNodes("/span/html/body/center/table/tr/td/table/tr/td[2]/table[@summary='Main Table']/tr[5]/td[3]/table[@summary='Transactional Data']/tr[position()>1 and position()<last()]");
        foreach (XmlNode node in transactionNodes)
        {
            XmlNode dateNode = node.SelectSingleNode("td[1]");
            XmlNode nameNode = node.SelectSingleNode("td[2]");
            XmlNode moneyNode = node.SelectSingleNode("td[3]");

            DateTime date = DateTime.ParseExact(HtmlAgilityPack.HtmlEntity.DeEntitize(dateNode.InnerText).Trim(), "dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
            int money = moneyInPenceFromString(HtmlAgilityPack.HtmlEntity.DeEntitize(moneyNode.InnerText).Trim().Replace(",", ""));
            string name = HtmlAgilityPack.HtmlEntity.DeEntitize(nameNode.InnerText).Trim();
            string type = getTypeFromNameOrValue(name, money);

            transactions.Add(new Transaction(money, date, name, type, ""));
        }

        XmlNode accountNumberNode = document.SelectSingleNode("/span/html/body/center/table/tr/td/table/tr/td[2]/table[@summary='Main Table']/tr[5]/td[3]/table[@summary='Content Area']/tr[7]/td[4]");
        string accountNumber = HtmlAgilityPack.HtmlEntity.DeEntitize(accountNumberNode.InnerText).Trim();

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
        outputFile.WriteLine("<BANKID>{0}</BANKID>", 0); // todo: find this out? It's not in the HTML.
        outputFile.WriteLine("<ACCTID>{0}</ACCTID>", accountNumber);
        outputFile.WriteLine("<ACCTTYPE>SAVINGS</ACCTTYPE>");
        outputFile.WriteLine("</BANKACCTFROM>");

        outputFile.WriteLine("<BANKTRANLIST>");

        outputFile.WriteLine("<DTSTART>{0}</DTSTART>", startDate.ToString("yyyyMMdd"));
        outputFile.WriteLine("<DTEND>{0}</DTEND>", endDate.ToString("yyyyMMdd"));

        foreach (Transaction transaction in transactions)
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

    private static string getTypeFromNameOrValue(string name, int value)
    {
        switch (name)
        {
            case "INTEREST": return "INT";
            case "CREDIT": return "CREDIT";
            default: return creditOrDebitStringFromValue(value);
        }
    }

    private static string creditOrDebitStringFromValue(int value)
    {
        if (value < 0) return "DEBIT";
        else return "CREDIT";
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
