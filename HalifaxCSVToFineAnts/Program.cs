using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace HalifaxCSVToFineAnts
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
            // Only do anything with files that exist
            if (fileInfo.Exists)
            {
                FineAntsCore.Statement statement = ConvertHalifaxCSVFileToFineAnts(fileInfo);

                string outputDirectory = fileInfo.DirectoryName;
                string outputFileName = string.Format("{0} - {1}.statement", statement.StartDate.ToString("yyyy-MM-dd"), statement.EndDate.ToString("yyyy-MM-dd"));
                FileInfo outFile = new FileInfo(outputDirectory + "/" + outputFileName);

                // To save time, only convert if the destination file doesn't already exist, or is older than the source data
                if (!outFile.Exists || outFile.LastWriteTime < fileInfo.LastWriteTime)
                {
                    FineAntsCore.Statement.SerialiseStatement(statement, outFile.FullName);
                }
            }
        }

        static FineAntsCore.Statement ConvertHalifaxCSVFileToFineAnts(FileInfo fileInfo)
        {
            List<FineAntsCore.Transaction> transactions;
            int closingBalance;
            TransactionsAndClosingBalanceFromCSVFile(fileInfo, out transactions, out closingBalance);

            DateTime latestDate;
            DateTime earliestDate;
            DateRangeFromTransactions(transactions, out latestDate, out earliestDate);

            FineAntsCore.Statement statement = new FineAntsCore.Statement(transactions, earliestDate, latestDate, closingBalance);

            return statement;
        }

        private static void TransactionsAndClosingBalanceFromCSVFile(FileInfo fileInfo, out List<FineAntsCore.Transaction> transactions, out int closingBalance)
        {
            transactions = new List<FineAntsCore.Transaction>();
            closingBalance = 0;

            TextFieldParser parser = new TextFieldParser(fileInfo.FullName);

            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Skip the first line, as it is just the headers.
            parser.ReadLine();

            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();

                // Generate a transaction from the line.
                FineAntsCore.Transaction transaction = TransactionFromCSVFields(fields);

                // Add it to the list.
                transactions.Add(transaction);

                // The 8th column holds the running total, and the file is sorted newest to oldest, so if this is the first line (after the headers, 1-based, and LineNumber is the line to be read next), store the balance as the closing balance.
                if (parser.LineNumber == 3)
                {
                    closingBalance = AmountFromString(fields[7]);
                }
            }

            parser.Close();

            // Finally, sort the transactions on date, since they're in the wrong order in the CSV.
            transactions.Sort(new FineAntsCore.TransactionDateComparer());
        }

        private static FineAntsCore.Transaction TransactionFromCSVFields(string[] fields)
        {
            // We must have 8 fields; Transaction Date, Transaction Type, Sort Code, Account Number, Transaction Description, Debit Amount, Credit Amount, Balance
            if (fields.Length != 8)
            {
                throw new Exception("CSV file must have 8 fields per line");
            }

            // Date is in the first column, stored as "dd/MM/yyyy".
            DateTime date = DateTime.ParseExact(fields[0], "dd\\/MM\\/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // The first 19 chars of the second column are the merchant name, and the rest (if any) are the description.
            string merchant = fields[4].Trim();
            string description = "";

            // The 6th column is debit amount and the 7th is credit. Only one will be filled out, and both will be positive. So check which has contents and parse it, and negate the amount if it is a debit.
            int amount = AmountFromFields(fields);

            // Create the transaction.
            FineAntsCore.Transaction transaction = new FineAntsCore.Transaction(amount, date, merchant, description);
            return transaction;
        }

        private static int AmountFromFields(string[] fields)
        {
            // The 6th column is debit amount and the 7th is credit.
            // Both will never be filled out at the same time, so check if one has characters and return it (and negate the debit column since the value will be positive).
            // Sometimes neither are filled out (such as in the opening balance of 0 line), in which case return 0.
            if (fields[5].Length > 0)
            {
                return -AmountFromString(fields[5]);
            }
            else if (fields[6].Length > 0)
            {
                return AmountFromString(fields[6]);
            }
            else
            {
                return 0;
            }
        }

        private static int AmountFromString(string stringAmount)
        {
            // String amounts are in the format "[-]nn.nn". Removing the decimal place allows us to get the amount in pence as an int.
            int amount = int.Parse(stringAmount.Replace(".", ""));
            return amount;
        }

        private static void DateRangeFromTransactions(List<FineAntsCore.Transaction> transactions, out DateTime latestDate, out DateTime earliestDate)
        {
            latestDate = DateTime.MinValue;
            earliestDate = DateTime.MaxValue;

            // Find earliest and latest dates of any transaction in the statement.
            foreach (FineAntsCore.Transaction transaction in transactions)
            {
                if (transaction.Date > latestDate)
                {
                    latestDate = transaction.Date;
                }

                if (transaction.Date < earliestDate)
                {
                    earliestDate = transaction.Date;
                }
            }

            // If we didn't find them for any reason (perhaps there are no transactions?), just use the current time.
            if (latestDate == DateTime.MinValue)
            {
                latestDate = DateTime.Now;
            }

            if (earliestDate == DateTime.MaxValue)
            {
                earliestDate = DateTime.Now;
            }
        }
    }
}
