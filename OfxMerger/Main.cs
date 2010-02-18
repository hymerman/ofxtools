namespace OfxMerger
{
    class Merger
    {
        static void Main(string[] args)
        {
            System.Collections.Generic.List<Ofx.Document> documents = new System.Collections.Generic.List<Ofx.Document>();

            string outputDirectory;

            // check if a directory has been supplied
            if ((System.IO.File.GetAttributes(args[0]) & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory)
            {
                outputDirectory = args[0];

                // load each file in the directory if it is an ofx file
                foreach (string filename in System.IO.Directory.GetFiles(args[0]))
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(filename);
                    if (System.String.Compare(info.Extension, ".ofx", true) == 0)
                    {
                        Ofx.Document file = new Ofx.Document(filename);
                        documents.Add(file);
                    }
                }
            }
            else
            {
                System.IO.FileInfo inputFileInfo = new System.IO.FileInfo(args[0]);
                outputDirectory = inputFileInfo.DirectoryName;

                // load each argument as an ofx file
                foreach (string filename in args)
                {
                    Ofx.Document file = new Ofx.Document(filename);
                    documents.Add(file);
                }
            }

            documents.Sort(delegate(Ofx.Document a, Ofx.Document b) { return a.startDate.CompareTo(b.startDate); });

            // create merged statement object with properties of first file in files
            Ofx.Document merged = new Ofx.Document();
            merged.usePropertiesFrom(documents[0]);
            merged.startDate = documents[0].startDate;
            merged.endDate = documents[documents.Count - 1].endDate;
            merged.closingBalanceDate = documents[documents.Count - 1].closingBalanceDate;
            merged.closingBalance = documents[documents.Count - 1].closingBalance;

            // add all the transactions from the first document to the merged document
            foreach (SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN transaction in documents[0].transactions)
            {
                // add to transactions of merged statement
                merged.transactions.Add(transaction);
            }

            System.Collections.Generic.List<string> warnings = new System.Collections.Generic.List<string>();

            for (int index = 1; index < documents.Count; ++index)
            {
                int value = System.DateTime.Compare(documents[index].startDate, documents[index-1].endDate.AddDays(1));
                if (value < 0)
                {
                    // todo: address duplicate transactions
                    warnings.Add("overlapping date range between documents " + (documents[index - 1].m_fileName) + " and " + documents[index].m_fileName);
                }
                else if(value > 0)
                {
                    warnings.Add("gap in date range between documents " + (documents[index - 1].m_fileName) + " and " + documents[index].m_fileName);
                }

                int laterClosingBalance = documents[index].closingBalance;
                int earlierClosingBalance = documents[index - 1].closingBalance;
                int transactionsInBetween = documents[index].sumOfTransactions();
                if (earlierClosingBalance + transactionsInBetween != laterClosingBalance)
                {
                    warnings.Add("Document " + documents[index].m_fileName + " has a closing balance of " + earlierClosingBalance + " which is inconsistent with its transactions (totalling " + transactionsInBetween + ") and the closing balance of " + laterClosingBalance + " in " + (documents[index - 1].m_fileName));
                }

                // add all the transactions to the merged document
                foreach (SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRN transaction in documents[index].transactions)
                {
                    // add to transactions of merged statement
                    merged.transactions.Add(transaction);
                }
            }

            if (warnings.Count > 0)
            {
                foreach (string warning in warnings)
                {
                    System.Console.WriteLine(warning);
                }

                System.Console.ReadKey();
            }

            // write merged file
            string outputFileName = string.Format("{0} - {1}.ofx", merged.startDate.ToString("yyyy-MM-dd"), merged.endDate.ToString("yyyy-MM-dd"));
            merged.Save(outputDirectory + "/" + outputFileName);
        }
    }
}
