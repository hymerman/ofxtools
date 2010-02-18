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
            if (fileNames.Length > 0)
            {
                fileName = fileNames[0];
            }

            InitializeComponent();
        }

        private void OfxEditor_Load(object sender, EventArgs e)
        {
            if (fileName != null)
            {
                loadFile();
            }
            else
            {
                openNewDocument();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
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

            bindControlsToDocument();
        }

        private void bindControlsToDocument()
        {
            dataGridView1.Columns.Clear();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystroke;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToResizeRows = false;

            DataGridViewComboBoxColumn trntypeColumn = new DataGridViewComboBoxColumn();
            trntypeColumn.Name = "Type";
            trntypeColumn.ToolTipText = "Type of transaction. If you don't know, just choose 'other', or 'credit'/'debit'";
            trntypeColumn.DataPropertyName = "TRNTYPE";
            trntypeColumn.DataSource = Enum.GetValues(typeof(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRNTRNTYPE));
            trntypeColumn.ValueType = typeof(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRNTRNTYPE);
            dataGridView1.Columns.Add(trntypeColumn);

            DataGridViewTextBoxColumn dtpostedColumn = new DataGridViewTextBoxColumn();
            dtpostedColumn.Name = "Date posted";
            dtpostedColumn.ToolTipText = "Date the transaction took place (not necessarily the date funds were transferred)";
            dtpostedColumn.DataPropertyName = "DTPOSTED";
            dataGridView1.Columns.Add(dtpostedColumn);

            //DataGridViewTextBoxColumn dtuserColumn = new DataGridViewTextBoxColumn();
            //dtuserColumn.Name = "User date";
            //dtuserColumn.ToolTipText = "Not sure what this is for, to be honest.";
            //dtuserColumn.DataPropertyName = "DTUSER";
            //dataGridView1.Columns.Add(dtuserColumn);

            DataGridViewTextBoxColumn dtavailColumn = new DataGridViewTextBoxColumn();
            dtavailColumn.Name = "Date available";
            dtavailColumn.ToolTipText = "Date funds were actually transferred; this may be different to the date posted e.g. if the merchant has a delay before notifying the bank of a credit card transaction for example";
            dtavailColumn.DataPropertyName = "DTAVAIL";
            dataGridView1.Columns.Add(dtavailColumn);

            DataGridViewTextBoxColumn trnamtColumn = new DataGridViewTextBoxColumn();
            trnamtColumn.Name = "Amount";
            trnamtColumn.ToolTipText = "Amount transferred, in the format n*.nn";
            trnamtColumn.DataPropertyName = "TRNAMT";
            dataGridView1.Columns.Add(trnamtColumn);

            DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.Name = "Name";
            nameColumn.ToolTipText = "Name/description of merchant/source";
            nameColumn.DataPropertyName = "NAME";
            dataGridView1.Columns.Add(nameColumn);

            DataGridViewTextBoxColumn memoColumn = new DataGridViewTextBoxColumn();
            memoColumn.Name = "Memo";
            memoColumn.ToolTipText = "Additional information";
            memoColumn.DataPropertyName = "MEMO";
            dataGridView1.Columns.Add(memoColumn);

            dataGridView1.DataSource = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN;

            // todo: validation of inputs
            // todo: some easier way to put dates in date cells
            // todo: validation/auto-fixing of amount cells

            accountTypeComboBox.DataSource = System.Enum.GetValues(typeof(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROMACCTTYPE));
            accountTypeComboBox.SelectedItem = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE;
            accountIDTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID;
            bankIDTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID;
            statementStartDateTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTSTART;
            statementEndDateTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND;
            ledgerBalanceTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.BALAMT;
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
                if (fileName != null && fileName != "")
                {
                    doSave();
                }
                else
                {
                    doSaveAs();
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (document != null)
            {
                doSaveAs();
            }
        }

        private void doSaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "ofx files (*.ofx)|*.ofx|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;

                doSave();
            }
        }

        private void doSave()
        {
            populateDocumentFromControls();

            document.Save(fileName);
        }

        private void populateDocumentFromControls()
        {
            document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTID = accountIDTextBox.Text;
            document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.ACCTTYPE = (SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKACCTFROMACCTTYPE)accountTypeComboBox.SelectedItem;
            document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKACCTFROM.BANKID = bankIDTextBox.Text;
            document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND = statementEndDateTextBox.Text;
            document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTSTART = statementStartDateTextBox.Text;
            document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.BALAMT = ledgerBalanceTextBox.Text;
            document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.DTASOF = ledgerBalanceAsOfTextBox.Text;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openNewDocument();
        }

        private void openNewDocument()
        {
            fileName = null;
            document = new Ofx.Document();

            bindControlsToDocument();
        }

        private void calculateDateRangeButton_Click(object sender, EventArgs e)
        {
            if (document != null)
            {
                document.calculateDateRange();
                statementStartDateTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTSTART;
                statementEndDateTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.DTEND;
            }
        }

        private void calculateClosingBalanceDetailsButton_Click(object sender, EventArgs e)
        {
            if (document != null)
            {
                document.calculateClosingBalanceDetails();
                ledgerBalanceTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.BALAMT;
                ledgerBalanceAsOfTextBox.Text = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.LEDGERBAL.DTASOF;
            }
        }
    }
}
