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
            m_statement.BANKMSGSRSV1.STMTTRNRS.STATUS.SEVERITY = SimpleOfx.STATUSSEVERITY.INFO;
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRS();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROM();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID = "";
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE = SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROMACCTTYPE.CHECKING;
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID = "";
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLIST();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND = DateTime.Now.ToString("yyyyMMdd");
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTSTART = DateTime.Now.ToString("yyyyMMdd");
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN = new System.ComponentModel.BindingList<SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN>();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.CURDEF = SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSCURDEF.GBP; // todo: take this from some global preference
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL = new SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSLEDGERBAL();
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.BALAMT = "0.00";
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.DTASOF = DateTime.Now.ToString("yyyyMMdd");
            m_statement.BANKMSGSRSV1.STMTTRNRS.TRNUID = "0";
            m_statement.SIGNONMSGSRSV1 = new SimpleOfx.OFXSIGNONMSGSRSV1();
            m_statement.SIGNONMSGSRSV1.SONRS = new SimpleOfx.OFXSIGNONMSGSRSV1SONRS();
            m_statement.SIGNONMSGSRSV1.SONRS.DTSERVER = DateTime.Now.ToString("yyyyMMddHHmmss");
            m_statement.SIGNONMSGSRSV1.SONRS.LANGUAGE = "ENG";
            m_statement.SIGNONMSGSRSV1.SONRS.STATUS = new SimpleOfx.STATUS();
            m_statement.SIGNONMSGSRSV1.SONRS.STATUS.CODE = "0";
            m_statement.SIGNONMSGSRSV1.SONRS.STATUS.SEVERITY = SimpleOfx.STATUSSEVERITY.INFO;
        }

        public Document(string fileName)
        {
            Load(fileName);
        }

        public void Load(string fileName)
        {
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

        public void Save(string filename)
        {
            //sortTransactionsByDate();

            generateTransactionIDs();

            //validateStatement();

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

        public void calculateClosingBalanceDetails()
        {
            if (m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN.Count < 0)
            {
                return;
            }

            int total = 0;
            foreach (SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN transaction in m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN)
            {
                int amount = moneyInPenceFromString(transaction.TRNAMT);
                total += amount;
            }

            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.BALAMT = formatAsPoundsAndPenceString(total);
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.DTASOF = m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND;
        }

        public void calculateDateRange()
        {
            if (m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN.Count < 0)
            {
                return;
            }

            DateTime earliest = DateTime.MaxValue;
            DateTime latest = DateTime.MinValue;

            foreach (SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN transaction in m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN)
            {
                DateTime date = DateTime.ParseExact(transaction.DTPOSTED, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                if (date.CompareTo(earliest) == -1) earliest = date;
                if (date.CompareTo(latest) == 1) latest = date;
            }

            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTSTART = earliest.ToString("yyyyMMdd");
            m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND = latest.ToString("yyyyMMdd");
        }

        private void validateStatement()
        {
            throw new NotImplementedException();
        }

        private void generateTransactionIDs()
        {
            foreach (SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN transaction in m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN)
            {
                hashTransaction(transaction);
            }
        }

        private static void hashTransaction(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN transaction)
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

        private void sortTransactionsByDate()
        {
            throw new NotImplementedException();
        }

        public string m_fileName;
        public string m_version;
        public SimpleOfx.OFX m_statement;
    }
}
