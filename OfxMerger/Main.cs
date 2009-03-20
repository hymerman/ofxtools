namespace OfxMerger
{
    class Merger
    {
        static void Main(string[] args)
        {
            System.Collections.Generic.List<OfxFile> files = new System.Collections.Generic.List<OfxFile>();

            // Read all OFX files into memory and store in a list
            foreach (string arg in args)
            {
                OfxFile file = new OfxFile(arg);
                files.Add(file);
            }

            // create merged statement object with properties of first file in files
            OfxFile merged = new OfxFile();
            merged.usePropertiesFrom(files[0]);

            // add all transactions from all files to merged file
            System.DateTime earliestStartDate = System.DateTime.MaxValue;
            System.DateTime latestEndDate = System.DateTime.MinValue;
            System.DateTime latestLedgerBalanceDate = System.DateTime.MinValue;
            int latestLedgerBalance = int.MinValue;

            files.Sort(delegate(OfxFile a, OfxFile b) { return a.m_startDate.CompareTo(b.m_startDate); });

            foreach (OfxFile file in files)
            {
                if (file.m_closingBalanceDate > latestLedgerBalanceDate)
                {
                    latestLedgerBalanceDate = file.m_closingBalanceDate;
                    latestLedgerBalance = file.m_closingBalance;
                }

                if (file.m_startDate < earliestStartDate)
                {
                    earliestStartDate = file.m_startDate;
                }

                if (file.m_endDate > latestEndDate)
                {
                    latestEndDate = file.m_endDate;
                }

                // loop through transactions
                foreach (OfxTransaction transaction in file.transactions())
                {
                    // add to transactions of merged statement
                    merged.AddTransaction(transaction);
                }
            }

            merged.m_startDate = earliestStartDate;
            merged.m_endDate = latestEndDate;
            merged.m_closingBalanceDate = latestLedgerBalanceDate;
            merged.m_closingBalance = latestLedgerBalance;

            // todo: address continuity of statements, sorting of transactions and avoiding duplicate transactions

            // write merged file to disc
            System.IO.FileInfo inputFileInfo = new System.IO.FileInfo(args[0]);
            string outputDirectory = inputFileInfo.DirectoryName;
            merged.writeToFile(outputDirectory);
        }
    }
}
