using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OfxEditor
{
    public partial class OfxEditor : Form
    {
        private string fileName;

        public OfxEditor(string fileName)
        {
            this.fileName = fileName;
            InitializeComponent();
        }

        private void OfxEditor_Load(object sender, EventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OFX.OFX));
            TextReader reader = new StreamReader(fileName);
            OFX.OFX ofx = (OFX.OFX)serializer.Deserialize(reader);
            reader.Close();

            dataGridView1.DataSource = ofx.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN;

            accountTypeComboBox.DataSource = System.Enum.GetNames(typeof(OFX.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROMACCTTYPE));
            accountTypeComboBox.SelectedItem = ofx.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE;
            accountIDTextBox.Text = ofx.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID;
            bankIDTextBox.Text = ofx.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID;
            statementStartDateTextBox.Text = ofx.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTSTART;
            statementEndDateTextBox.Text = ofx.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND;
            ledgerBalanceTextBox.Text = ofx.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.BALAMT.ToString();
            ledgerBalanceAsOfTextBox.Text = ofx.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.DTASOF;

        }
    }
}
