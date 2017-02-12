using System;
using System.Windows.Controls;


namespace SVCore
{
    /// <summary>
    /// SVWPFLockDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFLockDialog : UserControl
    {
        public SVWPFLockDialog()
        {
            InitializeComponent();
        }

        public void setChecked(Boolean b)
        {
            this.tmpCheckbox.IsChecked = b;
        }

        public Boolean? getChecked()
        {
            return this.tmpCheckbox.IsChecked;
        }
    }
}
