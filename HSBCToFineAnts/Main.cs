using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace HSBCToFineAnts
{
    class Converter
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
                FineAntsCore.Statement statement = ConvertHSBCHTMLFileToFineAnts(fileInfo);

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

        static FineAntsCore.Statement ConvertHSBCHTMLFileToFineAnts(FileInfo fileInfo)
        {
            HtmlAgilityPack.HtmlDocument brokenDocument = new HtmlAgilityPack.HtmlDocument();
            brokenDocument.Load(fileInfo.FullName);
            brokenDocument.OptionOutputAsXml = true;
            string fixedXmlFileName = fileInfo.FullName + ".fixed.xml";
            brokenDocument.Save(fixedXmlFileName);
            XmlDocument document = new XmlDocument();
            document.Load(fixedXmlFileName);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(document.NameTable);
            namespaceManager.AddNamespace("d", "http://www.w3.org/1999/xhtml");

            XmlNode closingBalanceNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='top']/d:div[@id='innerPage']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[last()]/d:td[6]/d:p", namespaceManager);
            XmlNode closingBalanceSignNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='top']/d:div[@id='innerPage']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[last()]/d:td[7]/d:p", namespaceManager);
            int closingBalance = moneyInPenceFromString(closingBalanceNode.InnerText.Trim());
            if (closingBalanceSignNode.InnerText.Trim() == "D") closingBalance = -closingBalance;

            XmlNode endDateNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='top']/d:div[@id='innerPage']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:div[@class='extPibRow hsbcRow']/d:div[@class='hsbcPadding']/d:div[@class='hsbcTextRight']", namespaceManager);
            string endDateString = HtmlAgilityPack.HtmlEntity.DeEntitize(endDateNode.InnerText).Trim();

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DateTime endDate = DateTime.ParseExact(endDateString, "dd MMM yyyy", provider);

            XmlNode startDateNode = document.SelectSingleNode("/span/d:html/d:body/d:div[@id='top']/d:div[@id='innerPage']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[1]/d:td[1]/d:p", namespaceManager);
            string startDateString = HtmlAgilityPack.HtmlEntity.DeEntitize(startDateNode.InnerText).Trim();

            DateTime startDate = dateFromDateStringFixedUsingUpperBoundDate(startDateString, endDate.AddDays(-1)).AddDays(1);

            List<FineAntsCore.Transaction> transactions = new List<FineAntsCore.Transaction>();

            XmlNodeList transactionNodes = document.SelectNodes("/span/d:html/d:body/d:div[@id='top']/d:div[@id='innerPage']/d:div[@id='wrapper']/d:div[@id='main']/d:div[@id='content']/d:div[@class='containerMain']/d:div[@class='hsbcMainContent hsbcCol']/d:div[@class='extContentHighlightPib hsbcCol']/d:table/d:tbody/d:tr[position()>1 and position()<last()]", namespaceManager);
            foreach (XmlNode node in transactionNodes)
            {
                XmlNode dateNode = node.SelectSingleNode("d:td[1]/d:p", namespaceManager);
                XmlNode typeNode = node.SelectSingleNode("d:td[2]/d:p", namespaceManager);
                XmlNode nameNode = node.SelectSingleNode("d:td[3]/d:p", namespaceManager);
                XmlNode moneyOutNode = node.SelectSingleNode("d:td[4]/d:p", namespaceManager);
                XmlNode moneyInNode = node.SelectSingleNode("d:td[5]/d:p", namespaceManager);

                string date = HtmlAgilityPack.HtmlEntity.DeEntitize(dateNode.InnerText).Trim();
                string name = HtmlAgilityPack.HtmlEntity.DeEntitize(getInnerTextIgnoringLinks(nameNode));
                string moneyIn = HtmlAgilityPack.HtmlEntity.DeEntitize(moneyInNode.InnerText).Trim();
                string moneyOut = HtmlAgilityPack.HtmlEntity.DeEntitize(moneyOutNode.InnerText).Trim();
                int money = moneyIn == "" ? -moneyInPenceFromString(moneyOut) : moneyInPenceFromString(moneyIn);

                transactions.Add(new FineAntsCore.Transaction(money, dateFromDateStringFixedUsingUpperBoundDate(date, endDate), name, ""));
            }

            // remove the temporary fixed file
            System.IO.File.Delete(fixedXmlFileName);

            FineAntsCore.Statement statement = new FineAntsCore.Statement(transactions, startDate, endDate, closingBalance);

            return statement;
        }

        private static DateTime dateFromDateStringFixedUsingUpperBoundDate(string date, DateTime endDate)
        {
            // Add the end date's year onto the string
            string potentialDateString = date + " " + endDate.Year.ToString();

            // Parse this string into a date
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DateTime potentialDate = DateTime.ParseExact(potentialDateString, "dd MMM yyyy", provider);

            // if this date comes after the end date of the statement, make it the year before
            if (potentialDate > endDate) potentialDate = potentialDate.AddYears(-1);

            return potentialDate;
        }

        private static string getInnerTextIgnoringLinks(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                return node.ChildNodes[0].InnerText.Trim();
            }
            else
            {
                return node.InnerText.Trim();
            }
        }

        private static int moneyInPenceFromString(string moneyAsString)
        {
            string fixedString = moneyAsString.Replace(".", "");
            int value = int.Parse(fixedString);
            return value;
        }
    }
}
