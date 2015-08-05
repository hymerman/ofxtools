using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AmexOFXCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string path in args)
            {
                CleanAmexOFX(path);
            }
        }

        private static void CleanAmexOFX(string path)
        {
            // Create a reader to read the broken file
            System.IO.TextReader reader = new System.IO.StreamReader(path);

            // Read the whole file into a string
            string content = reader.ReadToEnd();

            // Close the reader stream
            reader.Close();

            // Remove the non-standard bits
            content = System.Text.RegularExpressions.Regex.Replace(content, "<AMEX.UNIVID>[^<]*<", "<");
            content = System.Text.RegularExpressions.Regex.Replace(content, "<ORIGIN.ID>[^<]*<", "<");
            content = System.Text.RegularExpressions.Regex.Replace(content, "<START.TIME>[^<]*<", "<");
            content = System.Text.RegularExpressions.Regex.Replace(content, "<CYCLECUT.INDICATOR>[^<]*<", "<");
            content = System.Text.RegularExpressions.Regex.Replace(content, "<PURGE.INDICATOR>[^<]*<", "<");
            content = System.Text.RegularExpressions.Regex.Replace(content, "<INTL.INDICATOR>[^<]*<", "<");
            
            // Append '.broken' to the path of the file, which we will rename the old file to
            string newPath = path + ".broken";

            // Rename the old, broken file
            System.IO.File.Move(path, newPath);

            // Create a writer to write to the fixed file
            System.IO.TextWriter writer = new System.IO.StreamWriter(path);

            // Write the fixed content to the new stream
            writer.Write(content);

            // Close the write stream
            writer.Close();

            // Open up the fixed file again since there are some more things to do to it
            Ofx.Document document = new Ofx.Document(path, "ofx160.dtd");

            bool containsPayment = false;
            int previousPayment = 0;

            // Loop through all the transactions
            foreach (SimpleOfx.BankTranListTypeSTMTTRN transaction in document.transactions)
            {
                // Add decimal places to the amount field
                // 5.4 => 5.40, 20 => 20.00
                double amount = double.Parse(transaction.TRNAMT);
                transaction.TRNAMT = amount.ToString("F", System.Globalization.CultureInfo.InvariantCulture);

                // Remember if it was a payment of the previous statement - we'll use this in a minute to fix the closing balance
                if (transaction.NAME == "PAYMENT RECEIVED - THANK YOU")
                {
                    containsPayment = true;
                    previousPayment = Ofx.Document.moneyInPenceFromString(transaction.TRNAMT);
                }
            }

            // Calculate the closing balance date
            document.calculateClosingBalanceDetails();

            // Recalculate the closing balance
            if(containsPayment)
            {
                // Work out the actual closing balance - will be the sum of transactions other than the first payment, since this is equal to the opening balance
                int statementTotal = document.sumOfTransactions();
                int closingBalance = statementTotal - previousPayment;
                document.closingBalance = closingBalance;
            }

            document.Save();
        }
    }
}
