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

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SVCore;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Globalization;

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
