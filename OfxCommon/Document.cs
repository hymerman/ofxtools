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
            // set version from first few lines
            // deserialise statement from file
            Sgml.SgmlReader reader = new Sgml.SgmlReader();
            reader.SystemLiteral = "../../../external/SgmlReader/TestSuite/ofx160.dtd";
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

        public void Save(string filename)
        {
            // create streamWriter
            // pump out OFX crap if version is 1.xx
            // pump out XML header crap if version is 2.xx
            // serialise statement to stream
            // save stream to file
            throw new NotImplementedException();
        }

        public string m_version;
        public SimpleOfx.OFX m_statement;
    }
}
