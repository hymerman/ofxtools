using System;
using System.Windows.Forms;

namespace OfxEditor
{
    public partial class OfxEditor : Form
    {
        private string fileName;
        private Ofx.Document document;

        public OfxEditor(string[] fileNames)
        {
            if(fileNames.Length > 0)
                fileName = fileNames[0];

            InitializeComponent();
        }

        private void OfxEditor_Load(object sender, EventArgs e)
        {
            if (fileName != null)
                loadFile();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ofx files (*.ofx)|*.ofx|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;

                loadFile();
            }
        }

        private void loadFile()
        {
            document = new Ofx.Document(fileName);

            dataGridView1.DataSource = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToResizeRows = false;

            accountTypeComboBox.DataSource = System.Enum.GetNames(typeof(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROMACCTTYPE));
            accountTypeComboBox.SelectedItem = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE;
            accountIDTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID;
            bankIDTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID;
            statementStartDateTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTSTART;
            statementEndDateTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND;
            ledgerBalanceTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.BALAMT.ToString();
            ledgerBalanceAsOfTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.DTASOF;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (document != null)
            {
                document.Save();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ofx files (*.ofx)|*.ofx|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;

                document.Save(fileName);
            }
        }
    }
}
