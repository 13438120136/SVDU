using System;
using System.Windows.Forms;
using SVCore;

namespace SVControl
{
    public partial class SVPageSelectWindow : Form
    {
        String _pageText;
        UInt16 _pageID = 0;

        public SVPageSelectWindow()
        {
            InitializeComponent();
            initalizeWindow();
        }

        public String getPageText()
        {
            return _pageText;
        }

        public UInt16 getPageID()
        {
            return _pageID;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (pageText.Text == String.Empty || pageID.Text == String.Empty)
            {
                MessageBox.Show("没有选中页面项!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _pageText = String.Empty;
                _pageID = 0;
                return;
            }

            _pageText = pageText.Text;
            _pageID = UInt16.Parse(pageID.Text);
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void initalizeWindow()
        {
            foreach (var v in SVGlobalData.PageContainer)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell text = new DataGridViewTextBoxCell();
                text.Value = v.Key;

                DataGridViewTextBoxCell id = new DataGridViewTextBoxCell();
                SVPageWidget widget = (SVPageWidget)v.Value;
                id.Value = widget.Attrib.id.ToString();

                row.Cells.Add(text);
                row.Cells.Add(id);

                dataGridView.Rows.Add(row);
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            if (row < 0 || row >= dataGridView.RowCount)
                return;

            pageText.Text = dataGridView.Rows[row].Cells[0].Value.ToString();
            pageID.Text = dataGridView.Rows[row].Cells[1].Value.ToString();
        }

        private void pageTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();

            if (pageTextBox.Text == String.Empty)
            {
                initalizeWindow();
                return ;
            }

            foreach (var v in SVGlobalData.PageContainer)
            {
                String upValue = v.Key.ToUpper();
                String upText = pageTextBox.Text.ToUpper();

                SVPageWidget widget = (SVPageWidget)v.Value;
                String strID = widget.Attrib.id.ToString();

                if (!upValue.Contains(upText)
                    && !strID.Contains(pageTextBox.Text))
                    continue;

                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell text = new DataGridViewTextBoxCell();
                text.Value = v.Key;

                DataGridViewTextBoxCell id = new DataGridViewTextBoxCell();
                id.Value = strID;

                row.Cells.Add(text);
                row.Cells.Add(id);

                dataGridView.Rows.Add(row);
            }
        }
    }
}
