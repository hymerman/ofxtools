using System;
using System.Collections.Generic;
using System.IO;

namespace OfxMerger
{
    class Merger
    {
        static void Main(string[] args)
        {
            // Store statements alongside the name of the file they were loaded from, for diagnostic messages later.
            List<KeyValuePair<string, FineAntsCore.Statement>> statements = new List<KeyValuePair<string, FineAntsCore.Statement>>();

            DirectoryInfo outputDirectory;

            // For directory inputs, iterate all files inside the directory and output the merged file there.
            // For multiple file inputs, iterate each of them and output the merged file to the same directory as the first.
            if (Directory.Exists(args[0]))
            {
                // Use the input directory as the output directory.
                DirectoryInfo inputDirectory = new DirectoryInfo(args[0]);
                outputDirectory = inputDirectory;

                // Load each file in the directory as a statement
                foreach (var file in inputDirectory.GetFiles())
                {
                    statements.Add(new KeyValuePair<string, FineAntsCore.Statement>(file.Name, statementFromFileName(file.FullName)));
                }
            }
            else
            {
                // Use the first file's directory as the output directory.
                FileInfo firstFileInfo = new FileInfo(args[0]);
                outputDirectory = firstFileInfo.Directory;

                // Load each argument as statement.
                foreach (string filename in args)
                {
                    FileInfo file = new FileInfo(filename);
                    statements.Add(new KeyValuePair<string, FineAntsCore.Statement>(file.Name, statementFromFileName(file.FullName)));
                }
            }

            FineAntsCore.Statement merged;
            List<string> warnings;

            GenerateMergedStatement(statements, out merged, out warnings);

            // Wait for the user to acknowledge warnings.
            if (warnings.Count > 0)
            {
                foreach (string warning in warnings)
                {
                    Console.WriteLine(warning);
                }

                Console.ReadKey();
            }

            // write merged file
            string outputFileName = string.Format("{0} - {1} merged.statementjson", merged.StartDate.ToString("yyyy-MM-dd"), merged.EndDate.ToString("yyyy-MM-dd"));
            FineAntsCore.Statement.SerialiseStatementJSON(merged, outputDirectory + "/" + outputFileName);
        }

        private static void GenerateMergedStatement(List<KeyValuePair<string, FineAntsCore.Statement>> statements, out FineAntsCore.Statement merged, out List<string> warnings)
        {
            statements.Sort(delegate(KeyValuePair<string, FineAntsCore.Statement> a, KeyValuePair<string, FineAntsCore.Statement> b) { return a.Value.StartDate.CompareTo(b.Value.StartDate); });

            // create merged statement object with properties of first file in files
            merged = new FineAntsCore.Statement();
            merged.StartDate = statements[0].Value.StartDate;
            merged.EndDate = statements[statements.Count - 1].Value.EndDate;
            merged.ClosingBalance = statements[statements.Count - 1].Value.ClosingBalance;

            // add all the transactions from the first document to the merged document
            foreach (var transaction in statements[0].Value.Transactions)
            {
                // add to transactions of merged statement
                merged.Transactions.Add(transaction);
            }

            warnings = new List<string>();

            for (int index = 1; index < statements.Count; ++index)
            {
                int dateComparison = DateTime.Compare(statements[index].Value.StartDate, statements[index - 1].Value.EndDate.AddDays(1));
                if (dateComparison < 0)
                {
                    // todo: address duplicate transactions
                    warnings.Add("overlapping date range between documents " + (statements[index - 1].Key) + " and " + statements[index].Key);
                }
                else if (dateComparison > 0)
                {
                    warnings.Add("gap in date range between documents " + (statements[index - 1].Key) + " and " + statements[index].Key);
                }

                // Detect balance inaccuracies.
                int laterClosingBalance = statements[index].Value.ClosingBalance;
                int earlierClosingBalance = statements[index - 1].Value.ClosingBalance;
                int transactionsInBetween = sumOfTransactions(statements[index].Value);

                if (earlierClosingBalance + transactionsInBetween != laterClosingBalance)
                {
                    warnings.Add("Document " + statements[index].Key +
                        " has a closing balance of " + earlierClosingBalance +
                        " which is inconsistent with its transactions (totalling " + transactionsInBetween +
                        ") and the closing balance of " + laterClosingBalance +
                        " in " + (statements[index - 1].Key)
                    );
                }

                // add all the transactions to the merged document
                foreach (var transaction in statements[index].Value.Transactions)
                {
                    // add to transactions of merged statement
                    merged.Transactions.Add(transaction);
                }
            }

            merged.Transactions.Sort(delegate(FineAntsCore.Transaction a, FineAntsCore.Transaction b) { return a.Date.CompareTo(b.Date); });
        }

        private static int sumOfTransactions(FineAntsCore.Statement statement)
        {
            int total = 0;
            foreach (var transaction in statement.Transactions)
            {
                total += transaction.Amount;
            }
            return total;
        }

        static FineAntsCore.Statement statementFromFileName(string fileName)
        {
            FileInfo info = new FileInfo(fileName);
            if (String.Compare(info.Extension, ".ofx", true) == 0)
            {
                Ofx.Document file = new Ofx.Document(fileName, "ofx160.dtd");
                return file.ConvertToFineAntsStatement();
            }
            else if (String.Compare(info.Extension, ".statement", true) == 0)
            {
                return FineAntsCore.Statement.DeserialiseStatement(fileName);
            }
            else if (String.Compare(info.Extension, ".statementjson", true) == 0)
            {
                return FineAntsCore.Statement.DeserialiseStatementJSON(fileName);
            }

            throw new Exception("Not a statement file.");
        }
    }
}
