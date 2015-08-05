using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SantanderTextToFineAnts
{
    class Converter
    {
        static void Main(string[] args)
        {
            foreach(string arg in args)
            {
                if(Directory.Exists(arg))
                {
                    HandleDirectory(new System.IO.DirectoryInfo(arg));
                }
                else if(File.Exists(arg))
                {
                    HandleFile(new System.IO.FileInfo(arg));
                }
            }
        }

        private static void HandleDirectory(DirectoryInfo directoryInfo)
        {
            foreach(DirectoryInfo subDir in directoryInfo.EnumerateDirectories())
            {
                HandleDirectory(subDir);
            }

            foreach(FileInfo subDir in directoryInfo.EnumerateFiles())
            {
                HandleFile(subDir);
            }
        }

        private static void HandleFile(FileInfo fileInfo)
        {
            // Only do anything with files that exist
            if(fileInfo.Exists)
            {
                FineAntsCore.Statement statement = ConvertSantanderTextFileToFineAnts(fileInfo);

                string outputDirectory = fileInfo.DirectoryName;
                string outputFileName = string.Format("{0} - {1}.statementjson", statement.StartDate.ToString("yyyy-MM-dd"), statement.EndDate.ToString("yyyy-MM-dd"));
                FileInfo outFile = new FileInfo(outputDirectory + "/" + outputFileName);

                // To save time, only convert if the destination file doesn't already exist, or is older than the source data
                if(!outFile.Exists || outFile.LastWriteTime < fileInfo.LastWriteTime)
                {
                    FineAntsCore.Statement.SerialiseStatementJSON(statement, outFile.FullName);
                }
            }
        }

        static void readLines(StreamReader reader, int lines)
        {
            for(int i = 0; i < lines; ++i)
            {
                reader.ReadLine();
            }
        }

        private static int AmountFromString(string stringAmount)
        {
            // String amounts are in the format "[-]nn.nn". Removing the decimal place allows us to get the amount in pence as an int.
            int amount = int.Parse(stringAmount.Replace(".", ""));
            return amount;
        }

        class AssertException : Exception
        {
            public AssertException(string message) : base(message) {}
        }

        static void assert(bool condition, string message)
        {
            if(!condition)
            {
                //throw new AssertException(message);
            }
        }

        static FineAntsCore.Statement ConvertSantanderTextFileToFineAnts(FileInfo fileInfo)
        {
            StreamReader reader = new StreamReader(fileInfo.FullName, Encoding.Default, false);
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.InvariantCulture;

            var headerLine = reader.ReadLine();
            var headerLineHeader = "From: ";
            assert(headerLine.StartsWith(headerLineHeader), "Header line incorrectly formatted");

            var fromDateString = headerLine.Substring(headerLineHeader.Length, 10).Trim();
            var toDateString = headerLine.Substring(headerLineHeader.Length + 10 + 4).Trim();

            var fromDate = DateTime.ParseExact(fromDateString, "dd/MM/yyyy", culture);
            var toDate = DateTime.ParseExact(toDateString, "dd/MM/yyyy", culture);

            // Next line is account information.
            // Or sometimes it's a blank line and the account information is on the next line.
            // Seems to be random so skip two lines if the first one was blank.
            var maybeSkipThisLine = reader.ReadLine();
            if(maybeSkipThisLine.Trim() == "")
            {
                reader.ReadLine();
            }

            var dateLineHeader = "Date: ";
            var descriptionLineHeader = "Description: ";
            var amountLineHeader = "Amount: ";
            var balanceLineHeader = "Balance: ";

            List<FineAntsCore.Transaction> transactions = new List<FineAntsCore.Transaction>();

            int lastBalanceRead = 0;

            while(!reader.EndOfStream)
            {
                // Each entry is preceded by a blank line.
                reader.ReadLine();

                var dateLine = reader.ReadLine();
                var descriptionLine = reader.ReadLine();
                var amountLine = reader.ReadLine();
                var balanceLine = reader.ReadLine();

                assert(dateLine.StartsWith(dateLineHeader), "Date line incorrectly formatted");
                assert(descriptionLine.StartsWith(descriptionLineHeader), "Date line incorrectly formatted");
                assert(amountLine.StartsWith(amountLineHeader), "Date line incorrectly formatted");
                assert(balanceLine.StartsWith(balanceLineHeader), "Date line incorrectly formatted");

                var dateString = dateLine.Substring(dateLineHeader.Length).Trim();
                var amountString = amountLine.Substring(amountLineHeader.Length).Trim();
                var balanceString = balanceLine.Substring(balanceLineHeader.Length).Trim();

                var date = DateTime.ParseExact(dateString, "dd/MM/yyyy", culture);
                var description = descriptionLine.Substring(descriptionLineHeader.Length).Trim();
                var amount = AmountFromString(amountString);
                var balance = AmountFromString(balanceString);

                //if(transactions.Count > 0)
                //{
                //    assert(lastBalanceRead == balance - amount, "Balance and amount doesn't tally");
                //}

                lastBalanceRead = balance;

                transactions.Add(new FineAntsCore.Transaction(amount, date, description, ""));
            }

            // Sort transactions by date.
            transactions.Sort(new FineAntsCore.TransactionDateComparer());

            FineAntsCore.Statement statement = new FineAntsCore.Statement(transactions, fromDate, toDate, lastBalanceRead);

            return statement;
        }
    }
}
