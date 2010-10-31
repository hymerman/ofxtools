using System;
using System.Collections.Generic;
using System.Text;

namespace Ofx
{
    // todo: implementation of credit card account types

    public class Document
    {
        public Document()
        {
            m_version = "1";

            m_statement = new SimpleOfx.OFX();

            // It's ridiculous that xsd.exe doesn't generate a default constructor that will do all this for me.
            // Honestly, it has the knowledge that some nodes are required in the schema, so it should know to at least new them.
            // I haven't implemented a constructor since it's generated code, and putting it here is less hassle, even if it's ugly and stupid.
            m_statement.BANKMSGSRSV1 = new SimpleOfx.OFXBANKMSGSRSV1();
            m_statement.BANKMSGSRSV1.STMTTRNRS = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRS();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STATUS = new SimpleOfx.STATUS();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STATUS.CODE = "0";
            m_statement.BANKMSGSRSV1.STMTTRNRS.STATUS.SEVERITY = "INFO";
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRS();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROM();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID = "";
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE = "CHECKING";
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID = "";
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST = new SimpleOfx.BankTranListType();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND = formatDateAsString(DateTime.Now);
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTSTART = formatDateAsString(DateTime.Now);
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN = new System.ComponentModel.BindingList<SimpleOfx.BankTranListTypeSTMTTRN>();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.CURDEF = "GBP";
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL = new SimpleOfx.LedgerBalType();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.BALAMT = formatAsPoundsAndPenceString(0);
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.DTASOF = formatDateAsString(DateTime.Now);
            m_statement.BANKMSGSRSV1.STMTTRNRS.TRNUID = "0";
            m_statement.SIGNONMSGSRSV1 = new SimpleOfx.OFXSIGNONMSGSRSV1();
            m_statement.SIGNONMSGSRSV1.SONRS = new SimpleOfx.OFXSIGNONMSGSRSV1SONRS();
            m_statement.SIGNONMSGSRSV1.SONRS.DTSERVER = formatDateAsString(DateTime.Now);
            m_statement.SIGNONMSGSRSV1.SONRS.LANGUAGE = "ENG";
            m_statement.SIGNONMSGSRSV1.SONRS.STATUS = new SimpleOfx.STATUS();
            m_statement.SIGNONMSGSRSV1.SONRS.STATUS.CODE = "0";
            m_statement.SIGNONMSGSRSV1.SONRS.STATUS.SEVERITY = "INFO";
        }

        public Document(string fileName)
        {
            Load(fileName);
        }

        public void Load(string fileName)
        {
            // log this for later
            m_fileName = fileName;

            string dtdRelativePath = "../../../external/SgmlReader/TestSuite/ofx160.dtd";
            string dtdFullPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).AbsolutePath), dtdRelativePath);

            // deserialise statement from file
            Sgml.SgmlReader reader = new Sgml.SgmlReader();
            reader.SystemLiteral = dtdFullPath;
            reader.InputStream = new System.IO.StreamReader(fileName);
            reader.WhitespaceHandling = System.Xml.WhitespaceHandling.Significant;

            System.IO.StringWriter output = new System.IO.StringWriter();
            System.Xml.XmlTextWriter w = new System.Xml.XmlTextWriter(output);
            w.Formatting = System.Xml.Formatting.Indented;

            reader.Read();

            m_version = "2"; // todo: change

            while (reader.NodeType == System.Xml.XmlNodeType.Text || reader.NodeType == System.Xml.XmlNodeType.Whitespace)
            {
                reader.Read();
                m_version = "1"; // todo: change
            }

            while (!reader.EOF)
            {
                w.WriteNode(reader, true);
            }

            reader.InputStream.Close();
            reader.Close();


            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SimpleOfx.OFX));
            System.IO.StringReader otherreader = new System.IO.StringReader(output.ToString());
            m_statement = (SimpleOfx.OFX)serializer.Deserialize(otherreader);

            otherreader.Close();
        }

        public void Save()
        {
            if (m_fileName != null)
            {
                Save(m_fileName);
            }
            else
            {
                throw new Exception("Can't save without specifying a file name");
            }
        }

        public void Save(string filename)
        {
            //sortTransactionsByDate();

            generateTransactionIDs();

            //validateStatement();

            removeEmptyData();

            // create streamWriter
            System.IO.StreamWriter writer = new System.IO.StreamWriter(filename);

            // pump out OFX crap if version is 1.xx
            if(m_version == "1")
            {
                writer.WriteLine("OFXHEADER:100");
                writer.WriteLine("DATA:OFXSGML");
                writer.WriteLine("VERSION:102");
                writer.WriteLine("SECURITY:NONE");
                writer.WriteLine("ENCODING:USASCII");
                writer.WriteLine("CHARSET:1252");
                writer.WriteLine("COMPRESSION:NONE");
                writer.WriteLine("OLDFILEUID:NONE");
                writer.WriteLine("NEWFILEUID:NONE");
                writer.WriteLine();
            }

            // Create an intermediary XML writer which writes to the stream but doesn't bother with formal XML nonsense
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();

            if(m_version == "1")
            {
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;
                // this is supposedly required but actually just seems to break things. Seems to work ok without it.
                //settings.ConformanceLevel = System.Xml.ConformanceLevel.Fragment;
                // todo: prevent tags of the form <blah/> being written, as apparently some programs don't like those, even though they're valid XML
            }

            System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(writer, settings);

            //Create our own empty namespace for the output since we don't really care, especially for OFX 1.x
            System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
            ns.Add("", "");

            // serialise statement to stream
            // todo: tags with empty text will be serialised as e.g. <MEMO /> instead of <MEMO></MEMO>. Not sure if it's a big deal for OFX or not - I should check.
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SimpleOfx.OFX));
            serializer.Serialize(xmlWriter, m_statement, ns);

            // save stream to file
            xmlWriter.Flush();
            xmlWriter.Close();
            writer.Flush();
            writer.Close();
        }

        private void removeEmptyData()
        {
            foreach (SimpleOfx.BankTranListTypeSTMTTRN transaction in TransactionList.STMTTRN)
            {
                // the " " bits are kind of a workaround really, to prevent nodes like <NAME/> being written.
                // todo: fix this!
                if (transaction.DTAVAIL == "") transaction.DTAVAIL = null;
                if (transaction.DTPOSTED == "") transaction.DTPOSTED = " ";
                if (transaction.DTUSER == "") transaction.DTUSER = null;
                if (transaction.MEMO == "") transaction.MEMO = null;
                if (transaction.NAME == "") transaction.NAME = " ";
                if (transaction.REFNUM == "") transaction.REFNUM = " ";
                if (transaction.TRNAMT == "") transaction.TRNAMT = " ";
            }
        }

        public void calculateClosingBalanceDetails()
        {
            if (TransactionList.STMTTRN.Count < 0)
            {
                return;
            }

            LedgerBal.BALAMT = formatAsPoundsAndPenceString(sumOfTransactions());
            LedgerBal.DTASOF = TransactionList.DTEND;
        }

        public void calculateDateRange()
        {
            if (TransactionList.STMTTRN.Count < 0)
            {
                return;
            }

            DateTime earliest = DateTime.MaxValue;
            DateTime latest = DateTime.MinValue;

            foreach (SimpleOfx.BankTranListTypeSTMTTRN transaction in TransactionList.STMTTRN)
            {
                DateTime date = dateFromDateString(transaction.DTPOSTED);

                if (date.CompareTo(earliest) == -1) earliest = date;
                if (date.CompareTo(latest) == 1) latest = date;
            }

            TransactionList.DTSTART = formatDateAsString(earliest);
            TransactionList.DTEND = formatDateAsString(latest);
        }

        private static string formatDateAsString(DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        private void validateStatement()
        {
            throw new NotImplementedException();
        }

        private void generateTransactionIDs()
        {
            foreach (SimpleOfx.BankTranListTypeSTMTTRN transaction in TransactionList.STMTTRN)
            {
                hashTransaction(transaction);
            }
        }

        private static void hashTransaction(SimpleOfx.BankTranListTypeSTMTTRN transaction)
        {
            string mungedTransaction = "";
            if (transaction.DTAVAIL != null) mungedTransaction += transaction.DTAVAIL;
            if (transaction.DTPOSTED != null) mungedTransaction += transaction.DTPOSTED;
            if (transaction.DTUSER != null) mungedTransaction += transaction.DTUSER;
            if (transaction.MEMO != null) mungedTransaction += transaction.MEMO;
            if (transaction.NAME != null) mungedTransaction += transaction.NAME;
            if (transaction.REFNUM != null) mungedTransaction += transaction.REFNUM;
            if (transaction.TRNAMT != null) mungedTransaction += transaction.TRNAMT;
            if (transaction.TRNTYPE != null) mungedTransaction += transaction.TRNTYPE;

            byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(mungedTransaction);
            byte[] hash = System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(bytes);

            transaction.FITID = byteArrayToString(hash);
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

        public static int moneyInPenceFromString(string moneyAsString)
        {
            string fixedString = moneyAsString.Replace(".", "");
            int value = int.Parse(fixedString);
            return value;
        }

        private static string formatAsPoundsAndPenceString(int value)
        {
            int pounds = Math.Abs(value) / 100;
            int pence = Math.Abs(value) % 100;
            string signCharacter = value < 0 ? "-" : "";
            return signCharacter + string.Format("{0}.{1:00}", pounds, pence);
        }

        private void sortTransactionsByDate()
        {
            throw new NotImplementedException();
        }

        public string m_fileName;
        public string m_version;
        private SimpleOfx.OFX m_statement;

        public bool IsCreditCard
        {
            get
            {
                return m_statement.CREDITCARDMSGSRSV1 != null;
            }
            set
            {
                if (value == true && !IsCreditCard)
                {
                    m_statement.CREDITCARDMSGSRSV1 = new SimpleOfx.OFXCREDITCARDMSGSRSV1();
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS = new SimpleOfx.OFXCREDITCARDMSGSRSV1CCSTMTTRNRS();
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.STATUS = m_statement.BANKMSGSRSV1.STMTTRNRS.STATUS;
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS = new SimpleOfx.OFXCREDITCARDMSGSRSV1CCSTMTTRNRSCCSTMTRS();
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.BANKTRANLIST = m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST;
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CURDEF = m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.CURDEF;
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.LEDGERBAL = m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL;
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CCACCTFROM = new SimpleOfx.OFXCREDITCARDMSGSRSV1CCSTMTTRNRSCCSTMTRSCCACCTFROM();
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CCACCTFROM.ACCTID = m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID;
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.TRNUID = m_statement.BANKMSGSRSV1.STMTTRNRS.TRNUID;
                    m_statement.BANKMSGSRSV1 = null;
                }
                else if(value == false && IsCreditCard)
                {
                    m_statement.BANKMSGSRSV1 = new SimpleOfx.OFXBANKMSGSRSV1();
                    m_statement.BANKMSGSRSV1.STMTTRNRS = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRS();
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STATUS = m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.STATUS;
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRS();
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST = m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.BANKTRANLIST;
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.CURDEF = m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CURDEF;
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL = m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.LEDGERBAL;
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROM();
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID = m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CCACCTFROM.ACCTID;
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID = "";
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE = "CHECKING";
                    m_statement.BANKMSGSRSV1.STMTTRNRS.TRNUID = m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.TRNUID;
                    m_statement.CREDITCARDMSGSRSV1 = null;
                }
            }
        }

        public SimpleOfx.BankTranListType TransactionList
        {
            get
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    return m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST;
                }
                else if (m_statement.CREDITCARDMSGSRSV1 != null)
                {
                    return m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.BANKTRANLIST;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST = value;
                }
                else if (m_statement.CREDITCARDMSGSRSV1 != null)
                {
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.BANKTRANLIST = value;
                }
            }
        }

        public SimpleOfx.LedgerBalType LedgerBal
        {
            get
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    return m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL;
                }
                else if (m_statement.CREDITCARDMSGSRSV1 != null)
                {
                    return m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.LEDGERBAL;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL = value;
                }
                else if (m_statement.CREDITCARDMSGSRSV1 != null)
                {
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.LEDGERBAL = value;
                }
            }
        }

        public string AccountId
        {
            get
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    return m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID;
                }
                else if (m_statement.CREDITCARDMSGSRSV1 != null)
                {
                    return m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CCACCTFROM.ACCTID;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID = value;
                }
                else if (m_statement.CREDITCARDMSGSRSV1 != null)
                {
                    m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CCACCTFROM.ACCTID = value;
                }
            }
        }

        public string AccountType
        {
            get
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    return m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE;
                }
                else
                {
                    // todo: not entirely sure what to do for this one!
                    return null;
                }
            }
            set
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE = value;
                }
            }
        }

        public string BankId
        {
            get
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    return m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID;
                }
                else
                {
                    // todo: not entirely sure what to do for this one!
                    return null;
                }
            }
            set
            {
                if (m_statement.BANKMSGSRSV1 != null)
                {
                    m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID = value;
                }
            }
        }
        
        public DateTime startDate
        {
            get
            {
                return dateFromDateString(TransactionList.DTSTART);
            }
            set
            {
                TransactionList.DTSTART = formatDateAsString(value);
            }
        }

        public static DateTime dateFromDateString(string dateString)
        {
            return DateTime.ParseExact(dateString.Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
        }

        public void usePropertiesFrom(Document document)
        {
            if (document.m_statement.BANKMSGSRSV1 != null)
            {
                IsCreditCard = false;
                m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM;
            }
            else
            {
                IsCreditCard = true;
                m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CCACCTFROM = document.m_statement.CREDITCARDMSGSRSV1.CCSTMTTRNRS.CCSTMTRS.CCACCTFROM;
            }
        }

        public DateTime closingBalanceDate
        {
            get
            {
                return dateFromDateString(LedgerBal.DTASOF);
            }
            set
            {
                LedgerBal.DTASOF = formatDateAsString(value);
            }   
        }

        public int closingBalance
        {
            get
            {
                return moneyInPenceFromString(LedgerBal.BALAMT);
            }
            set
            {
                LedgerBal.BALAMT = formatAsPoundsAndPenceString(value);
            }   
        }

        public DateTime endDate
        {
            get
            {
                return dateFromDateString(TransactionList.DTEND);
            }
            set
            {
                TransactionList.DTEND = formatDateAsString(value);
            }
        }

        public System.ComponentModel.BindingList<SimpleOfx.BankTranListTypeSTMTTRN> transactions
        {
            get
            {
                return TransactionList.STMTTRN;
            }
            set
            {
                TransactionList.STMTTRN = value;
            }
        }

        public int sumOfTransactions()
        {
            int total = 0;
            foreach (SimpleOfx.BankTranListTypeSTMTTRN transaction in TransactionList.STMTTRN)
            {
                int amount = moneyInPenceFromString(transaction.TRNAMT);
                total += amount;
            }
            return total;
        }

        public string accountNumber
        {
            get
            {
                return m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID;
            }
            set
            {
                m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID = value;
            }
        }

        public string bankNumber
        {
            get
            {
                return m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID;
            }
            set
            {
                m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID = value;
            }
        }

        public void addTransaction(int amount, System.DateTime datePosted, string name, string type, string memo)
        {
            SimpleOfx.BankTranListTypeSTMTTRN transaction = new SimpleOfx.BankTranListTypeSTMTTRN();
            transaction.DTPOSTED = formatDateAsString(datePosted);
            transaction.MEMO = memo;
            transaction.NAME = name;
            transaction.TRNAMT = formatAsPoundsAndPenceString(amount);
            transaction.TRNTYPE = type;

            TransactionList.STMTTRN.Add(transaction);
        }
    }
}
