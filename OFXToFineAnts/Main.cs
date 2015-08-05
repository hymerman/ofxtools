using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace OFXToFineAnts
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
                // Form the statement filename by chopping off 'ofx' and replacing with 'statementjson'
                string statementFilename = fileInfo.FullName.Substring(0, fileInfo.FullName.Length - fileInfo.Extension.Length) + ".statementjson";

                FileInfo statementFile = new FileInfo(statementFilename);

                // To save time, only convert if the destination file doesn't already exist, or is older than the source data
                if (!statementFile.Exists || statementFile.LastWriteTime < fileInfo.LastWriteTime)
                {
                    ConvertOfxFileToFineAntsStatementFile(fileInfo, statementFile);
                }
            }
            // Also upgrade old statements
            else if (fileInfo.Extension == ".statement" && fileInfo.Exists)
            {
                // Form the statement filename by chopping off 'statement' and replacing with 'statementjson'
                string statementFilename = fileInfo.FullName.Substring(0, fileInfo.FullName.Length - fileInfo.Extension.Length) + ".statementjson";

                FileInfo statementFile = new FileInfo(statementFilename);

                // To save time, only convert if the destination file doesn't already exist, or is older than the source data
                if (!statementFile.Exists || statementFile.LastWriteTime < fileInfo.LastWriteTime)
                {
                    UpgradeFineAntsStatement(fileInfo, statementFile);
                }
            }
            // Also pump out just the transactions from upgraded statements...
            else if (fileInfo.Extension == ".statementjson" && fileInfo.Exists)
            {
                // Form the statement filename by chopping off 'statementjson' and replacing with 'transactions'
                string statementFilename = fileInfo.FullName.Substring(0, fileInfo.FullName.Length - fileInfo.Extension.Length) + ".transactions";

                FileInfo statementFile = new FileInfo(statementFilename);

                // To save time, only convert if the destination file doesn't already exist, or is older than the source data
                if (!statementFile.Exists || statementFile.LastWriteTime < fileInfo.LastWriteTime)
                {
                    FineAntsCore.Statement statement = FineAntsCore.Statement.DeserialiseStatementJSON(fileInfo.FullName);

                    FineAntsCore.Statement.SerialiseStatementTransactionListJSON(statement, statementFile.FullName);
                }
            }
        }

        private static void ConvertOfxFileToFineAntsStatementFile(FileInfo ofxFile, FileInfo statementFile)
        {
            Ofx.Document file = new Ofx.Document(ofxFile.FullName, "ofx160.dtd");

            FineAntsCore.Statement statement = file.ConvertToFineAntsStatement();

            FineAntsCore.Statement.SerialiseStatementJSON(statement, statementFile.FullName);
        }

        private static void UpgradeFineAntsStatement(FileInfo oldFile, FileInfo newFile)
        {
            FineAntsCore.Statement statement = FineAntsCore.Statement.DeserialiseStatement(oldFile.FullName);

            FineAntsCore.Statement.SerialiseStatementJSON(statement, newFile.FullName);
        }
    }
}
