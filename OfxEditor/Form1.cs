using System;
using System.Windows.Forms;

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
            Ofx.Document doc = new Ofx.Document(fileName);

            dataGridView1.DataSource = doc.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN;

            accountTypeComboBox.DataSource = System.Enum.GetNames(typeof(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROMACCTTYPE));
            accountTypeComboBox.SelectedItem = doc.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE;
            accountIDTextBox.Text = doc.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID;
            bankIDTextBox.Text = doc.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID;
            statementStartDateTextBox.Text = doc.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTSTART;
            statementEndDateTextBox.Text = doc.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND;
            ledgerBalanceTextBox.Text = doc.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.BALAMT.ToString();
            ledgerBalanceAsOfTextBox.Text = doc.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.DTASOF;

        }
    }
}
