using System;

namespace OfxMerger
{
    class OfxFile
    {
        public OfxFile()
        {
            m_transactions = new System.Collections.Generic.List<OfxTransaction>();
        }

        public OfxFile(string path)
        {
            Sgml.SgmlReader reader = new Sgml.SgmlReader();
            reader.SystemLiteral = "D:/personalProjects/ofxtools/external/SgmlReader/TestSuite/ofx160.dtd";
            //reader.DocType = "SGML";
            reader.InputStream = new System.IO.StreamReader(path);
            reader.WhitespaceHandling = System.Xml.WhitespaceHandling.Significant;
            
            System.IO.StringWriter output = new System.IO.StringWriter();
            System.Xml.XmlTextWriter w = new System.Xml.XmlTextWriter(output);
            w.Formatting = System.Xml.Formatting.Indented;

            reader.Read();

            while (reader.NodeType == System.Xml.XmlNodeType.Text || reader.NodeType == System.Xml.XmlNodeType.Whitespace)
            {
                reader.Read();
            }

            while (!reader.EOF)
            {
                w.WriteNode(reader, true);
            }

            string xmlString = output.ToString();
            System.Xml.XmlDocument document = new System.Xml.XmlDocument();
            document.LoadXml(xmlString);

            // find out whether it's a bank or credit card statement
            if (document.SelectSingleNode("/OFX/BANKMSGSRSV1") != null)
            {
                m_broadAccountType = AccountType.Bank;
            }
            else
            {
                m_broadAccountType = AccountType.CreditCard;
            }

            // get account information
            System.Xml.XmlNode bankIDNode = document.SelectSingleNode("/OFX/BANKMSGSRSV1/STMTTRNRS/STMTRS/BANKACCTFROM/BANKID");
            System.Xml.XmlNode bankAccountIDNode = document.SelectSingleNode("/OFX/BANKMSGSRSV1/STMTTRNRS/STMTRS/BANKACCTFROM/ACCTID");
            System.Xml.XmlNode bankAccountTypeNode = document.SelectSingleNode("/OFX/BANKMSGSRSV1/STMTTRNRS/STMTRS/BANKACCTFROM/ACCTTYPE");
            m_bankID = bankIDNode.InnerText;
            m_accountID = bankAccountIDNode.InnerText;
            m_accountType = bankAccountTypeNode.InnerText;

            // get start and end dates of statement
            System.Xml.XmlNode startDateNode = document.SelectSingleNode("/OFX/BANKMSGSRSV1/STMTTRNRS/STMTRS/BANKTRANLIST/DTSTART");
            System.Xml.XmlNode endDateNode = document.SelectSingleNode("/OFX/BANKMSGSRSV1/STMTTRNRS/STMTRS/BANKTRANLIST/DTEND");
            m_startDate = DateTime.ParseExact(startDateNode.InnerText, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            m_endDate = DateTime.ParseExact(endDateNode.InnerText, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            
            // get closing balance and date this refers to
            System.Xml.XmlNode closingBalanceNode = document.SelectSingleNode("/OFX/BANKMSGSRSV1/STMTTRNRS/STMTRS/LEDGERBAL/BALAMT");
            System.Xml.XmlNode closingBalanceDateNode = document.SelectSingleNode("/OFX/BANKMSGSRSV1/STMTTRNRS/STMTRS/LEDGERBAL/DTASOF");
            m_closingBalance = moneyInPenceFromString(closingBalanceNode.InnerText);
            m_closingBalanceDate = DateTime.ParseExact(closingBalanceDateNode.InnerText, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

            System.Xml.XmlNodeList transactionNodes = document.SelectNodes("/OFX/BANKMSGSRSV1/STMTTRNRS/STMTRS/BANKTRANLIST/STMTTRN");
            m_transactions = new System.Collections.Generic.List<OfxTransaction>();
            foreach (System.Xml.XmlNode transactionNode in transactionNodes)
            {
                m_transactions.Add(new OfxTransaction(transactionNode));
            }
        }

        internal void usePropertiesFrom(OfxFile ofxFile)
        {
            m_accountID = ofxFile.m_accountID;
            m_accountType = ofxFile.m_accountType;
            m_bankID = ofxFile.m_bankID;
            m_broadAccountType = ofxFile.m_broadAccountType;
        }

        //todo: make respect broad account type
        internal void writeToFile(string outputPath)
        {
            string outputFileName = string.Format("{0} - {1}.ofx", m_startDate.ToString("yyyy-MM-dd"), m_endDate.ToString("yyyy-MM-dd"));
            System.IO.StreamWriter outputFile = new System.IO.StreamWriter(outputPath + "/" + outputFileName);

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
            outputFile.WriteLine("<BANKID>{0}</BANKID>", m_bankID);
            outputFile.WriteLine("<ACCTID>{0}</ACCTID>", m_accountID);
            outputFile.WriteLine("<ACCTTYPE>{0}</ACCTTYPE>", m_accountType);
            outputFile.WriteLine("</BANKACCTFROM>");

            outputFile.WriteLine("<BANKTRANLIST>");

            outputFile.WriteLine("<DTSTART>{0}</DTSTART>", m_startDate.ToString("yyyyMMdd"));
            outputFile.WriteLine("<DTEND>{0}</DTEND>", m_endDate.ToString("yyyyMMdd"));

            foreach (OfxTransaction transaction in m_transactions)
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
            outputFile.WriteLine("<BALAMT>{0}</BALAMT>", formatAsPoundsAndPenceString(m_closingBalance));
            outputFile.WriteLine("<DTASOF>{0}</DTASOF>", m_closingBalanceDate.ToString("yyyyMMdd"));
            outputFile.WriteLine("</LEDGERBAL>");

            outputFile.WriteLine("</STMTRS>");
            outputFile.WriteLine("</STMTTRNRS>");
            outputFile.WriteLine("</BANKMSGSRSV1>");
            outputFile.WriteLine("</OFX>");

            outputFile.Close();
        }

        internal System.Collections.Generic.List<OfxTransaction> transactions()
        {
            return m_transactions;
        }

        internal void AddTransaction(OfxTransaction transaction)
        {
            m_transactions.Add(transaction);
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

        private static string formatAsPoundsAndPenceString(int value)
        {
            int pounds = value / 100;
            int pence = Math.Abs(value % 100);
            return string.Format("{0}.{1:00}", pounds, pence);
        }

        enum AccountType
        {
            Bank,
            CreditCard
        }

        AccountType m_broadAccountType;
        string m_bankID;
        string m_accountID;
        string m_accountType;
        public int m_closingBalance;
        public DateTime m_closingBalanceDate;
        public DateTime m_startDate;
        public DateTime m_endDate;
        System.Collections.Generic.List<OfxTransaction> m_transactions;
    }
}
