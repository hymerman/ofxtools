namespace OfxMerger
{
    class Merger
    {
        static void Main(string[] args)
        {
            System.Collections.Generic.List<Ofx.Document> documents = new System.Collections.Generic.List<Ofx.Document>();

            // check if a directory has been supplied
            if ((System.IO.File.GetAttributes(args[0]) & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory)
            {
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
                // load each argument as an ofx file
                foreach (string filename in args)
                {
                    Ofx.Document file = new Ofx.Document(filename);
                    documents.Add(file);
                }
            }

            // create merged statement object with properties of first file in files
            Ofx.Document merged = new Ofx.Document();
            merged.usePropertiesFrom(documents[0]);
            merged.startDate = documents[0].startDate;
            merged.endDate = documents[documents.Count - 1].endDate;
            merged.closingBalanceDate = documents[documents.Count - 1].closingBalanceDate;
            merged.closingBalance = documents[documents.Count - 1].closingBalance;

            documents.Sort(delegate(Ofx.Document a, Ofx.Document b) { return a.startDate.CompareTo(b.startDate); });

            System.Collections.Generic.List<string> warnings = new System.Collections.Generic.List<string>();

            for (int index = 1; index < documents.Count; ++index)
            {
                int value = System.DateTime.Compare(documents[index].startDate, documents[index-1].endDate.AddDays(1));
                if (value < 0)
                {
                    // todo: address duplicate transactions
                    warnings.Add("overlapping date range between documents " + (index-1) + " and " + index);
                }
                else if(value > 0)
                {
                    warnings.Add("gap in date range between documents " + (index-1) + " and " + index);
                }

                if (documents[index - 1].closingBalance + documents[index].sumOfTransactions() != documents[index].closingBalance)
                {
                    warnings.Add("Document " + index + "'s closing balance inconsistent with transactions and previous document's closing balance");
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
            System.IO.FileInfo inputFileInfo = new System.IO.FileInfo(args[0]);
            string outputDirectory = inputFileInfo.DirectoryName;
            string outputFileName = string.Format("{0} - {1}.ofx", merged.startDate.ToString("yyyy-MM-dd"), merged.endDate.ToString("yyyy-MM-dd"));
            merged.Save(outputDirectory + "/" + outputFileName);
        }
    }
}
