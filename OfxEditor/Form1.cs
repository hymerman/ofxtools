﻿using System;
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
            //dataGridView1.AutoGenerateColumns = false;
            //dataGridView1.AllowUserToAddRows = true;
            //dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystroke;
            //dataGridView1.AllowUserToDeleteRows = true;
            //dataGridView1.AllowUserToOrderColumns = true;
            //dataGridView1.AllowUserToResizeColumns = true;
            //dataGridView1.AllowUserToResizeRows = false;

            //DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
            //col.Name = "My Enum Column";
            //col.DataPropertyName = "TRNTYPE";
            //col.DataSource = Enum.GetValues(typeof(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRNTRNTYPE));
            //col.ValueType = typeof(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRNTRNTYPE);
            ////col.CellTemplate = new DataGridViewComboBoxCell(); // needed?
            //dataGridView1.Columns.Add(col);

            //DataGridViewTextBoxColumn col2 = new DataGridViewTextBoxColumn();
            //col2.Name = "My Non-Enum Column";
            //col2.DataPropertyName = "DTPOSTED";
            ////col2.DataSource = Enum.GetValues(typeof(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRNTRNTYPE));
            ////col2.ValueType = typeof(SimpleOfx.OFXBANKMSGSRSV1STMTTRNRSSTMTRSBANKTRANLISTSTMTTRNTRNTYPE);
            ////col.CellTemplate = new DataGridViewTextBoxCell(); // needed?
            //dataGridView1.Columns.Add(col2);


            //dataGridView1.DataSource = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN;

            dataGridView1.AllowUserToAddRows = true;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystroke;
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.DataSource = document.m_statement.BANKMSGSRSV1.STMTTRNRS.STMTRS.BANKTRANLIST.STMTTRN;
            // todo: make TRNTYPE use a combobox cell
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
            }
        }

        private void calculateClosingBalanceDetailsButton_Click(object sender, EventArgs e)
        {
            if (document != null)
            {
                document.calculateClosingBalanceDetails();
            }
        }
    }
}
