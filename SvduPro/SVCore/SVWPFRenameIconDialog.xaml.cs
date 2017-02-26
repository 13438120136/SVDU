using System;
using System.Collections.Generic;
using System.Windows;
using System.ComponentModel;

namespace SVCore
{
    /// <summary>
    /// SVWPFIconRenma.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFRenameIconDialog : Window
    {
        private BindingList<String> _nameClassList = new BindingList<String>();

        public SVWPFRenameIconDialog()
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

        public void setOldName(String name)
        {
            this.oldName.Content = name;
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
