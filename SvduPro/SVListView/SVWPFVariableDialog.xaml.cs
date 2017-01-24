using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using SVCore;
using System.Windows.Data;
using System.Globalization;

namespace SVControl
{
    /// <summary>
    /// SVWPFVariableDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFVariableDialog : UserControl
    {
        public SVWPFVariableDialog()
        {
            InitializeComponent();

            var instance = SVVaribleType.instance();
            var dataTable = instance.loadVariableData();

            DataTable dd = dataTable;
            this.inputDataGrid.ItemsSource = dataTable.DefaultView;
            this.outputDataGrid.ItemsSource = dataTable.DefaultView;
            initSysTable();
        }

        private void inputDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            var item = inputDataGrid.SelectedItem;
            DataRowView dataRowView = item as DataRowView;
            DataRow dataRow = dataRowView.Row;
            String columnName = dataRow[0] as String;

            this.name.DataContext = columnName;
            this.type.DataContext = 0;
        }

        /// <summary>
        /// 初始化系统变量表格
        /// </summary>
        private void initSysTable()
        {
            DataTable systemDataTable = new DataTable();

            ///添加表头
            systemDataTable.Columns.Add("blockName", typeof(String));
            systemDataTable.Columns.Add("varAddress", typeof(UInt32));
            systemDataTable.Columns.Add("varType", typeof(String));

            ///表格中的内容
            List<SVVarNode> vList = new List<SVVarNode>() 
            {
                new SVVarNode("当前运行版本0", 0, "INT"),
                new SVVarNode("当前运行软件1", 1, "INT"),
                new SVVarNode("配置文件版本0", 2, "INT"),
                new SVVarNode("配置文件版本1", 3, "INT"),
                new SVVarNode("接收任务时间", 4, "INT"),
                new SVVarNode("接收任务最大时间", 5, "INT"),
                new SVVarNode("显示任务时间", 6, "INT"),
                new SVVarNode("显示任务最大时间", 7, "INT"),
                new SVVarNode("发送任务时间", 8, "INT"),
                new SVVarNode("发送任务最大时间", 9, "INT"),
                new SVVarNode("杂项任务时间", 10, "INT"),
                new SVVarNode("杂项任务最大时间", 11, "INT"),

                new SVVarNode("当前空闲时间", 12, "INT"),
                new SVVarNode("当前周期时间", 13, "INT"),
                new SVVarNode("周期最大设置时间", 14, "INT"),

                new SVVarNode("当前周期个数", 15, "INT"),

                new SVVarNode("平台ROM状态", 16, "INT"),
                new SVVarNode("平台RAM状态", 17, "INT"),
                new SVVarNode("cpu诊断状态", 18, "INT"),
                new SVVarNode("时钟诊断状态", 19, "INT"),

                new SVVarNode("内存使用率", 20, "INT"),
                new SVVarNode("cpu使用率", 21, "INT"),

                new SVVarNode("数据接收状态", 22, "INT"),
                new SVVarNode("数据发送状态", 23, "INT"),
                new SVVarNode("人机交互状态", 24, "INT"),

                new SVVarNode("当前运行模式", 25, "INT"),

                new SVVarNode("接收任务超时", 26, "INT"),
                new SVVarNode("显示任务超时", 27, "INT"),
                new SVVarNode("发送任务超时", 28, "INT"),
                new SVVarNode("杂项任务超时", 29, "INT"),
                new SVVarNode("周期超时", 30, "INT")
            };

            foreach (var item in vList)
            {
                DataRow dr = systemDataTable.NewRow();
                dr["blockName"] = item.Name;
                dr["varAddress"] = item.Address;
                dr["varType"] = item.Type;
                systemDataTable.Rows.Add(dr);
            }

            this.systemDataGrid.ItemsSource = systemDataTable.DefaultView;
        }

        private void outputDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = outputDataGrid.SelectedItem;
            DataRowView dataRowView = item as DataRowView;
            DataRow dataRow = dataRowView.Row;
            String columnName = dataRow[0] as String;

            this.name.DataContext = columnName;
            this.type.DataContext = 1;
        }

        private void systemDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = systemDataGrid.SelectedItem;
            DataRowView dataRowView = item as DataRowView;
            DataRow dataRow = dataRowView.Row;
            String columnName = dataRow[0] as String;

            this.name.DataContext = columnName;
            this.type.DataContext = 2;
        }
    }

    /// <summary>
    /// 类型与字符串之间的转换
    /// </summary>
    public class VarTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            Byte bValue = System.Convert.ToByte(value);
            switch (bValue)
            {
                case 0:
                    return "接收区";
                case 1:
                    return "发送区";
                case 2:
                    return "系统区";
                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
