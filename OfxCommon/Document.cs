using System;
using System.Collections.Generic;
using System.Text;

namespace Ofx
{
    public class Document
    {
        public Document(string fileName)
        {
            Load(fileName);
        }

        public void Load(string fileName)
        {
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

            reader.Close();


            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SimpleOfx.OFX));
            System.IO.StringReader otherreader = new System.IO.StringReader(output.ToString());
            m_statement = (SimpleOfx.OFX)serializer.Deserialize(otherreader);

            otherreader.Close();
        }

        public void Save()
        {
            Save(m_fileName);
        }

        public void Save(string filename)
        {
            m_fileName = filename;

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

        public string m_fileName;
        public string m_version;
        public SimpleOfx.OFX m_statement;
    }
}
