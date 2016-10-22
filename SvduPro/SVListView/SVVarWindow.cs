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
            loadDataFromDB(0);
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

            switch (index)
            {
                case 0:
                    {
                        loadDataFromDB(index);
                        break;
                    }
                case 1:
                    {
                        loadDataFromDB(index);
                        break;
                    }
                case 2:
                    {
                        loadSystemData();
                        break;
                    }
            }
        }

        /// <summary>
        /// 初始系统的默认变量
        /// </summary>
        void loadSystemData()
        {
            DataTable systemDataTable = new DataTable();

            ///添加表头
            systemDataTable.Columns.Add("变量名", typeof(String));
            systemDataTable.Columns.Add("变量地址", typeof(UInt32));
            systemDataTable.Columns.Add("变量类型", typeof(String));

            ///表格中的内容
            List<SVVarNode> vList = new List<SVVarNode>() 
            {
                new SVVarNode("当前运行版本0", 0, "INT"),
                new SVVarNode("当前运行软件1", 4, "INT"),
                new SVVarNode("配置文件版本0", 8, "INT"),
                new SVVarNode("配置文件版本1", 12, "INT"),
                new SVVarNode("接收任务时间", 16, "INT"),
                new SVVarNode("接收任务最大时间", 20, "INT"),
                new SVVarNode("接收任务最大时间", 24, "INT"),
                new SVVarNode("显示任务时间", 28, "INT"),
                new SVVarNode("显示任务最大时间", 32, "INT"),
                new SVVarNode("发送任务时间", 36, "INT"),
                new SVVarNode("发送任务最大时间", 40, "INT"),
                new SVVarNode("杂项任务时间", 44, "INT"),
                new SVVarNode("杂项任务最大时间", 48, "INT"),

                new SVVarNode("当前杂项时间", 52, "INT"),
                new SVVarNode("当前周期使用时间", 56, "INT"),
                new SVVarNode("周期最大设置时间", 60, "INT"),

                new SVVarNode("当前周期时间", 64, "INT"),

                new SVVarNode("平台ROM状态", 68, "INT"),
                new SVVarNode("平台RAM状态", 72, "INT"),
                new SVVarNode("cpu诊断状态", 76, "INT"),
                new SVVarNode("时钟诊断状态", 80, "INT"),

                new SVVarNode("内存使用率", 84, "INT"),
                new SVVarNode("cpu使用率", 88, "INT"),

                new SVVarNode("数据接收状态", 92, "INT"),
                new SVVarNode("数据发送状态", 96, "INT"),
                new SVVarNode("人机交互状态", 100, "INT"),

                new SVVarNode("当前运行模式", 104, "INT"),

                new SVVarNode("接收任务超时", 108, "INT"),
                new SVVarNode("显示任务超时", 112, "INT"),
                new SVVarNode("发送任务超时", 116, "INT"),
                new SVVarNode("杂项任务超时", 120, "INT"),
                new SVVarNode("周期超时", 124, "INT")
            };

            foreach (var item in vList)
            {
                DataRow dr = systemDataTable.NewRow();
                dr["变量名"] = item.Name;
                dr["变量地址"] = item.Address;
                dr["变量类型"] = item.Type;
                systemDataTable.Rows.Add(dr);
            }

            _dataTable = systemDataTable;
            dataGridView.DataSource = _dataTable;

            filters();
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
                strBuilder.Append(String.Format("变量类型 = '{0}'", filters[i]));
                if (i != (nCount - 1))
                    strBuilder.Append(" or ");
            }

            _filters = strBuilder.ToString();
            dataView.RowFilter = strBuilder.ToString();
            dataGridView.DataSource = dataView;
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
            filters();
        }

        private void filters()
        {
            DataView dataView = _dataTable.DefaultView;
            if (String.IsNullOrWhiteSpace(textBox.Text))
            {
                dataView.RowFilter = _filters;
            }
            else
            {
                if (_filters == null)
                    dataView.RowFilter = String.Format("变量名 like '%{0}%'", textBox.Text.Trim());
                else
                    dataView.RowFilter = String.Format("变量名 like '%{0}%' and ", textBox.Text.Trim()) + _filters;
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
        /// 加载数据库中的数据到内存
        /// 
        /// 0 - 表示接收变量
        /// 1 - 表示发送变量
        /// </summary>
        void loadDataFromDB(Int32 type)
        {
            var instance = SVVaribleType.instance();
            _dataTable = instance.loadVariableData();
            instance.setDataType(type);

            filters();
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

    public class SVVarNode
    {
        public String Name;
        public UInt32 Address;
        public String Type;

        public SVVarNode(String name, UInt32 address, String type)
        {
            this.Name = name;
            this.Address = address;
            this.Type = type;
        }
    }
}
