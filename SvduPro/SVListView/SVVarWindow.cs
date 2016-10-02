using System;
using System.Data;
using System.Resources;
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

        public SVVarWindow()
        {
            ResourceManager res = new ResourceManager(typeof(Resource));

            _dataTable.Columns.Add(res.GetString("变量"), Type.GetType("System.String"));
            _dataTable.Columns.Add(res.GetString("地址"), Type.GetType("System.Int32"));

            InitializeComponent();
            loadDataFromDB();

            this.textBox.TextChanged += new EventHandler(textBox_TextChanged);
            this.dataGridView.CellClick += new DataGridViewCellEventHandler(dataGridView_CellClick);
        }

        /// <summary>
        /// 单击表格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textBox_TextChanged(object sender, EventArgs e)
        {
            DataView dataView = _dataTable.DefaultView;
            if (String.IsNullOrWhiteSpace(textBox.Text))
            {
                dataView.RowFilter = null;
            }
            else
            {
                dataView.RowFilter = String.Format("变量 like '%{0}%'", textBox.Text.Trim());
            }

            dataGridView.DataSource = dataView;
        }

        /// <summary>
        /// 执行确定按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cacenlBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        /// <summary>
        /// 执行刷新动作
        /// 将数据库中的数据显示在表格中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshBtn_Click(object sender, EventArgs e)
        {
            loadDataFromDB();
        }

        /// <summary>
        /// 加载数据库中的数据到内存
        /// </summary>
        void loadDataFromDB()
        {
            SVInterfaceApplication app = SVApplication.Instance;
            SVSqlDataBase sqlDataBase = app.DataBase;

            _dataTable.Clear();
            //DataTable dataTable = sqlDataBase.getVarDataList(1);
            //_dataTable = new DataTable();

            _dataTable.Rows.Add(new object[] { "var", 100 });
            _dataTable.Rows.Add(new object[] { "var1", 120 });
            _dataTable.Rows.Add(new object[] { "vvar009", 160 });

            dataGridView.DataSource = _dataTable;
        }

        /// <summary>
        /// 获取变量名称
        /// </summary>
        /// <returns>变量名称</returns>
        public String varText()
        {
            return this.labelVar.Text;
        }
    }
}
