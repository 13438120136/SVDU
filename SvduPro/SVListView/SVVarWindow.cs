using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SVCore;

namespace SVControl
{
    public partial class SVVarWindow : Form
    {
        /// <summary>
        /// 保存从数据库中获取的值
        /// </summary>
        DataTable _dataTable = new DataTable();
        //
        String _filters = null;

        public SVVarWindow()
        {
            InitializeComponent();

            //loadDataFromDB(0);
            this.varTypeCombox.SelectedIndex = 0;

            this.textBox.TextChanged += new EventHandler(textBox_TextChanged);
            this.dataGridView.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
            this.varTypeCombox.SelectedIndexChanged += new EventHandler(varTypeCombox_SelectedIndexChanged);
        }

        /// <summary>
        /// 对变量类型的选择
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void varTypeCombox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = this.varTypeCombox.SelectedIndex;

            var instance = SVVaribleType.instance();
            DataTable dataTable = new DataTable();

            switch (index)
            {
                case 0:
                    {
                        dataTable = instance.loadRecvDataTable();
                        break;
                    }
                case 1:
                    {
                        dataTable = instance.loadSendDataTable();
                        break;
                    }
                case 2:
                    {
                        dataTable = instance.loadSystemDataTable();
                        break;
                    }
            }

            if (_filters != null)
                dataTable.DefaultView.RowFilter = _filters;
            dataGridView.DataSource = dataTable.DefaultView;
        }

        /// <summary>
        /// 设置变量类型过滤条件
        /// 
        /// 条件设置如下值：
        /// 
        /// INT, INT_VAR
        /// SHORT_INT, SHORTINT_VAR
        /// REAL, REAL_VAR
        /// BOOL, BOOL_VAR
        /// 
        /// </summary>
        /// <param Name="filters">过滤条件</param>
        public void setFilter(List<String> filters)
        {
            if (filters == null || filters.Count == 0)
                return;

            DataView dataView = _dataTable.DefaultView;

            StringBuilder strBuilder = new StringBuilder();
            Int32 nCount = filters.Count;
            for (int i = 0; i < nCount;  i++)
            {
                strBuilder.Append(String.Format("valueType = '{0}'", filters[i]));
                if (i != (nCount - 1))
                    strBuilder.Append(" or ");
            }

            _filters = strBuilder.ToString();

            if (dataView.Count > 0)
            {
                dataView.RowFilter = strBuilder.ToString();
                dataGridView.DataSource = dataView;
            }
        }

        /// <summary>
        /// 单击表格事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            if (row < 0 || row >= dataGridView.RowCount)
                return;

            this.labelVar.Text = dataGridView.Rows[row].Cells[0].Value.ToString();
        }

        /// <summary>
        /// 输入变量控件文本发生改变的事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void textBox_TextChanged(object sender, EventArgs e)
        {
            DataView dataView = dataGridView.DataSource as DataView;
            if (dataView == null)
                return;

            if (String.IsNullOrWhiteSpace(textBox.Text))
            {
                dataView.RowFilter = _filters;
            }
            else
            {
                if (_filters == null)
                    dataView.RowFilter = String.Format("ioblockname like '%{0}%'", textBox.Text.Trim());
                else
                    dataView.RowFilter = String.Format("ioblockname like '%{0}%' and ", textBox.Text.Trim()) + _filters;
            }

            dataGridView.DataSource = dataView;
        }

        /// <summary>
        /// 执行确定按钮事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(labelVar.Text))
            {
                MessageBox.Show("没有选择变量!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 执行取消按钮事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void cacenlBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        /// <summary>
        /// 执行刷新动作
        /// 将数据库中的数据显示在表格中
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void refreshBtn_Click(object sender, EventArgs e)
        {
            //loadDataFromDB();
        }

        /// <summary>
        /// 获取变量名称
        /// </summary>
        /// <returns>变量名称</returns>
        public String varText()
        {
            return this.labelVar.Text;
        }

        /// <summary>
        /// 获取变量类型
        /// </summary>
        /// <returns></returns>
        public Byte getVarType()
        {
            return Convert.ToByte(this.varTypeCombox.SelectedIndex);
        }
    }
}
