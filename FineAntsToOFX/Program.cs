using System.IO;
using System.Xml.Serialization;

namespace FineAntsToOFX
{
    class Program
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
            if (fileInfo.Extension == ".statement" && fileInfo.Exists)
            {
                // Form the statement filename by chopping off 'ofx' and replacing with 'statement'
                string ofxFilename = fileInfo.FullName.Substring(0, fileInfo.FullName.Length - fileInfo.Extension.Length) + ".ofx";

                FileInfo ofxFile = new FileInfo(ofxFilename);

                // To save time, only convert if the destination file doesn't already exist, or is older than the source data
                if (!ofxFile.Exists || ofxFile.LastWriteTime < fileInfo.LastWriteTime)
                {
                    ConvertFineAntsStatementFileToOfxFile(fileInfo, ofxFile);
                }
            }
        }

        private static void ConvertFineAntsStatementFileToOfxFile(FileInfo statementFile, FileInfo ofxFile)
        {
            FineAntsCore.Statement statement = FineAntsCore.Statement.DeserialiseStatement(statementFile.FullName);

            Ofx.Document file = Ofx.Document.LoadFromFineAntsStatement(statement);

            file.Save(ofxFile.FullName);
        }
    }
}
