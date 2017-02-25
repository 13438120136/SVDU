using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SVCore
{
    /// <summary>
    /// WPFNewIconClassDialog.xaml 的交互逻辑
    /// </summary>
    public partial class WPFNewIconClassDialog : Window
    {
        private List<String> _nameClassList = new List<String>();

        public WPFNewIconClassDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置当前所有的分类名称
        /// </summary>
        public void setNameData(List<String> value)
        {
            _nameClassList = value;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox.Text))
            {
                MessageBox.Show("不能为空!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_nameClassList.Contains(textBox.Text))
            {
                MessageBox.Show(String.Format("分类名称'{0}'已经存在!", textBox.Text), "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.DialogResult = true;
        }
    }
}
