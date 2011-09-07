using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace FineAntsStatementFromOFX
{
    public class Application
    {
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                if (Directory.Exists(arg))
                {
                    HandleDirectory(new System.IO.DirectoryInfo(arg));
                }
                else if (File.Exists(arg))
                {
                    HandleFile(new System.IO.FileInfo(arg));
                }
            }
        }

        private static void HandleDirectory(DirectoryInfo directoryInfo)
        {
            foreach (DirectoryInfo subDir in directoryInfo.EnumerateDirectories())
            {
                HandleDirectory(subDir);
            }

            foreach (FileInfo subDir in directoryInfo.EnumerateFiles())
            {
                HandleFile(subDir);
            }
        }

        private static void HandleFile(FileInfo fileInfo)
        {
            // Only do anything with ofx files that exist
            if (fileInfo.Extension == ".ofx" && fileInfo.Exists)
            {
                // Form the statement filename by chopping off 'ofx' and replacing with 'statement'
                string statementFilename = fileInfo.FullName.Substring(0, fileInfo.FullName.Length - fileInfo.Extension.Length) + ".statement";

                FileInfo statementFile = new FileInfo(statementFilename);

                // To save time, only convert if the destination file doesn't already exist, or is older than the source data
                if (!statementFile.Exists || statementFile.LastWriteTime < fileInfo.LastWriteTime)
                {
                    ConvertOfxFileToFineAntsStatementFile(fileInfo, statementFile);
                }
            }
        }

        private static void ConvertOfxFileToFineAntsStatementFile(FileInfo ofxFile, FileInfo statementFile)
        {
            Ofx.Document file = new Ofx.Document(ofxFile.FullName, "../../../external/SgmlReader/TestSuite/ofx160.dtd");

            FineAntsCore.Statement statement = file.ConvertToFineAntsStatement();

            XmlSerializer serializer = new XmlSerializer(statement.GetType());
            TextWriter textWriter = new StreamWriter(statementFile.FullName);
            serializer.Serialize(textWriter, statement);
            textWriter.Close();
        }
    }
}
