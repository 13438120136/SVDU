using System;
using System.ComponentModel;
using System.Windows;

namespace SVCore
{
    /// <summary>
    /// WPFNewIconClassDialog.xaml 的交互逻辑
    /// </summary>
    public partial class WPFNewIconClassDialog : Window
    {
        private BindingList<String> _nameClassList = new BindingList<String>();

        public WPFNewIconClassDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置当前所有的分类名称
        /// </summary>
        public void setNameData(BindingList<String> value)
        {
            _nameClassList = value;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox.Text))
            {
                MessageBox.Show("名称不能为空!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
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
