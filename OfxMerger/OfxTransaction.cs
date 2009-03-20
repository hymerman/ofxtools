using System;

namespace OfxMerger
{
    class OfxTransaction
    {
        // todo: handle cases where some data is not present (optional 'memo')
        public OfxTransaction(System.Xml.XmlNode node)
        {
            amountPence = moneyInPenceFromString(node.SelectSingleNode("TRNAMT").InnerText);
            name = node.SelectSingleNode("NAME").InnerText;
            date = DateTime.ParseExact(node.SelectSingleNode("DTPOSTED").InnerText, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            type = node.SelectSingleNode("TRNTYPE").InnerText;
            memo = node.SelectSingleNode("MEMO").InnerText;
        }

        public OfxTransaction(int amountPence, DateTime date, string name, string type, string memo)
        {
            this.amountPence = amountPence;
            this.date = date;
            this.name = name;
            this.type = type;
            this.memo = memo;
        }

        public string hash()
        {
            string mungedTransaction = string.Format("{0}{1}{2}{3}{4}", amountPence, date, name, memo, type);
            byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(mungedTransaction);
            byte[] hash = System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(bytes);
            return byteArrayToString(hash);
        }

        private static string byteArrayToString(byte[] arrInput)
        {
            int i;
            System.Text.StringBuilder sOutput = new System.Text.StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        private static int moneyInPenceFromString(string moneyAsString)
        {
            string[] parts = moneyAsString.Split('.');
            int pounds = int.Parse(parts[0]);
            int pence = int.Parse(parts[1]);
            int value = pounds * 100;

            if (pounds < 0)
            {
                value -= pence;
            }
            else
            {
                value += pence;
            }

            return value;
        }
        
        public int amountPence;
        public DateTime date;
        public string name;
        public string type;
        public string memo;
    }
}
