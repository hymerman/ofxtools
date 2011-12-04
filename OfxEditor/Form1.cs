using System;
using System.Windows.Forms;

namespace OfxEditor
{
    public partial class OfxEditor : Form
    {
        private string fileName;
        private FineAntsCore.Statement statement;

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
            openFileDialog.Filter = "FineAnts files (*.statement)|*.statement|ofx files (*.ofx)|*.ofx|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;

                loadFile();
            }
        }

        private void loadFile()
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);

            if (fileInfo.Extension == ".ofx")
            {
                Ofx.Document document = new Ofx.Document(fileInfo.FullName, "../../../external/SgmlReader/TestSuite/ofx160.dtd");
                statement = document.ConvertToFineAntsStatement();
            }
            else if (fileInfo.Extension == ".statement")
            {
                statement = FineAntsCore.Statement.DeserialiseStatement(fileInfo.FullName);
            }

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

            DataGridViewTextBoxColumn dateColumn = new DataGridViewTextBoxColumn();
            dateColumn.Name = "Date";
            dateColumn.ToolTipText = "Date the funds were transferred";
            dateColumn.DataPropertyName = "Date";
            dataGridView1.Columns.Add(dateColumn);

            DataGridViewTextBoxColumn amountColumn = new DataGridViewTextBoxColumn();
            amountColumn.Name = "Amount";
            amountColumn.ToolTipText = "Amount transferred, in pence";
            amountColumn.DataPropertyName = "Amount";
            dataGridView1.Columns.Add(amountColumn);

            DataGridViewTextBoxColumn merchantColumn = new DataGridViewTextBoxColumn();
            merchantColumn.Name = "Name";
            merchantColumn.ToolTipText = "Name/description of merchant/source";
            merchantColumn.DataPropertyName = "Merchant";
            dataGridView1.Columns.Add(merchantColumn);

            DataGridViewTextBoxColumn descriptionColumn = new DataGridViewTextBoxColumn();
            descriptionColumn.Name = "Description";
            descriptionColumn.ToolTipText = "Additional information";
            descriptionColumn.DataPropertyName = "Description";
            dataGridView1.Columns.Add(descriptionColumn);

            BindingSource bs = new BindingSource();
            bs.DataSource = statement.Transactions;

            dataGridView1.DataSource = bs;

            // todo: validation of inputs
            // todo: validation/auto-fixing of amount cells

            closingBalanceTextBox.DataBindings.Clear();
            closingBalanceTextBox.DataBindings.Add(new Binding("Text", statement, "ClosingBalance"));

            startDatePicker.DataBindings.Clear();
            startDatePicker.DataBindings.Add(new Binding("Value", statement, "StartDate"));

            endDatePicker.DataBindings.Clear();
            endDatePicker.DataBindings.Add(new Binding("Value", statement, "EndDate"));

            //startDatePicker.Value = statement.StartDate;
            //endDatePicker.Value = statement.EndDate;
            //closingBalanceTextBox.Text = statement.ClosingBalance.ToString();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statement != null)
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
            if (statement != null)
            {
                doSaveAs();
            }
        }

        private void doSaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "FineAnts files (*.statement)|*.statement|ofx files (*.ofx)|*.ofx|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;

                doSave();
            }
        }

        private void doSave()
        {
            populateDocumentFromControls();

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);

            if (fileInfo.Extension == ".ofx")
            {
                Ofx.Document document = Ofx.Document.LoadFromFineAntsStatement(statement);
                document.Save(fileInfo.FullName);
            }
            else
            {
                FineAntsCore.Statement.SerialiseStatement(statement, fileInfo.FullName);
            }
        }

        private void populateDocumentFromControls()
        {
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openNewDocument();
        }

        private void openNewDocument()
        {
            fileName = null;
            statement = new FineAntsCore.Statement();

            bindControlsToDocument();
        }

        private void calculateDateRangeButton_Click(object sender, EventArgs e)
        {
            if (statement == null)
            {
                return;
            }

            DateTime latestDate = DateTime.MinValue;
            DateTime earliestDate = DateTime.MaxValue;

            // Find earliest and latest dates of any transaction in the statement.
            foreach (FineAntsCore.Transaction transaction in statement.Transactions)
            {
                if (transaction.Date > latestDate)
                {
                    latestDate = transaction.Date;
                }

                if (transaction.Date < earliestDate)
                {
                    earliestDate = transaction.Date;
                }
            }

            // If we didn't find them for any reason (perhaps there are no transactions?), just use the current time.
            if (latestDate == DateTime.MinValue)
            {
                latestDate = DateTime.Now;
            }

            if (earliestDate == DateTime.MaxValue)
            {
                earliestDate = DateTime.Now;
            }

            // Update the statement. Strangely it seems both the statement and the control
            // need to be update for this to be reflected properly.
            // I'd have thought this is exactly the kind of thing Binding is meant to do.
            statement.StartDate = earliestDate;
            statement.EndDate = latestDate;

            startDatePicker.Value = earliestDate;
            endDatePicker.Value = latestDate;
        }

        private void calculateClosingBalanceDetailsButton_Click(object sender, EventArgs e)
        {
            if (statement == null)
            {
                return;
            }

            int total = 0;

            // Sum the amounts of all the transactions in this statement.
            foreach (FineAntsCore.Transaction transaction in statement.Transactions)
            {
                total += transaction.Amount;
            }

            // Update the statement. Strangely it seems both the statement and the control
            // need to be update for this to be reflected properly.
            // I'd have thought this is exactly the kind of thing Binding is meant to do.
            statement.ClosingBalance = total;
            closingBalanceTextBox.Text = total.ToString();
        }

        private void dataGridView1_DefaultValuesNeeded(object sender, System.Windows.Forms.DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Date"].Value = DateTime.Now.Date;
            e.Row.Cells["Amount"].Value = 0;
            e.Row.Cells["Name"].Value = "";
            e.Row.Cells["Description"].Value = "";
        }
    }
}
