using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace HalifaxCreditCardCSVToFineAnts
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
                string outputFileName = string.Format("{0} - {1}.statementjson", statement.StartDate.ToString("yyyy-MM-dd"), statement.EndDate.ToString("yyyy-MM-dd"));
                FileInfo outFile = new FileInfo(outputDirectory + "/" + outputFileName);

                // To save time, only convert if the destination file doesn't already exist, or is older than the source data
                if (!outFile.Exists || outFile.LastWriteTime < fileInfo.LastWriteTime)
                {
                    FineAntsCore.Statement.SerialiseStatementJSON(statement, outFile.FullName);
                }
            }
        }

        static FineAntsCore.Statement ConvertHalifaxCSVFileToFineAnts(FileInfo fileInfo)
        {
            List<FineAntsCore.Transaction> transactions;
            TransactionsFromCSVFile(fileInfo, out transactions);

            DateTime latestDate;
            DateTime earliestDate;
            DateRangeFromTransactions(transactions, out latestDate, out earliestDate);

            // Can't get closing balance as it's not given to us anywhere, not even on the website.
            FineAntsCore.Statement statement = new FineAntsCore.Statement(transactions, earliestDate, latestDate, 0);

            return statement;
        }

        private static void TransactionsFromCSVFile(FileInfo fileInfo, out List<FineAntsCore.Transaction> transactions)
        {
            transactions = new List<FineAntsCore.Transaction>();

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
            }

            parser.Close();

            // Finally, sort the transactions on date, since they're in the wrong order in the CSV.
            transactions.Sort(new FineAntsCore.TransactionDateComparer());
        }

        private static FineAntsCore.Transaction TransactionFromCSVFields(string[] fields)
        {
            // We must have 5 fields; Transaction Date, Entered date, some kind of number..., Transaction Description, Amount, and possibly a stupid blank one
            if (!(fields.Length == 5 || (fields.Length == 6 && fields[5] == "")))
            {
                throw new Exception("CSV file must have 5 fields per line, or 6 and a blank one. Format may have changed since this program was written.");
            }

            // Date is in the first column, stored as "dd/MM/yyyy".
            DateTime date = DateTime.ParseExact(fields[0], "dd\\/MM\\/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // Merchant/description is in the 4th column. They don't appear to be separated so just use the whole thing as the merchant.
            string merchant = fields[3].Trim();

            // The 5th column is the transaction amount. Positive numbers are debits.
            int amount = -AmountFromString(fields[4]);

            // Create the transaction.
            FineAntsCore.Transaction transaction = new FineAntsCore.Transaction(amount, date, merchant, "");
            return transaction;
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
