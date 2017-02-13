using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using SVCore;
using System.Windows.Data;
using System.Globalization;
using System.Text;

namespace SVControl
{
    /// <summary>
    /// SVWPFVariableDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFVariableDialog : UserControl
    {
        private DataTable _systemDataTable = new DataTable();
        private DataTable _recvTable = new DataTable(); //接收地址表格
        private DataTable _sendTable = new DataTable(); //发送地址表格

        public SVWPFVariableDialog()
        {
            InitializeComponent();

            var instance = SVVaribleType.instance();
            _recvTable = instance.loadRecvDataTable();
            _sendTable = instance.loadSendDataTable();
            _systemDataTable = instance.loadSystemDataTable();

            this.inputDataGrid.ItemsSource = _recvTable.DefaultView;
            this.outputDataGrid.ItemsSource = _sendTable.DefaultView;
            this.systemDataGrid.ItemsSource = _systemDataTable.DefaultView;
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

            StringBuilder strBuilder = new StringBuilder();
            Int32 nCount = filters.Count;
            for (int i = 0; i < nCount; i++)
            {
                strBuilder.Append(String.Format("valueType = '{0}'", filters[i]));
                if (i != (nCount - 1))
                    strBuilder.Append(" or ");
            }

            _recvTable.DefaultView.RowFilter = strBuilder.ToString();
            _sendTable.DefaultView.RowFilter = strBuilder.ToString();
            _systemDataTable.DefaultView.RowFilter = strBuilder.ToString();

            this.inputDataGrid.ItemsSource = _recvTable.DefaultView;
            this.outputDataGrid.ItemsSource = _sendTable.DefaultView;
            this.systemDataGrid.ItemsSource = _systemDataTable.DefaultView;
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
