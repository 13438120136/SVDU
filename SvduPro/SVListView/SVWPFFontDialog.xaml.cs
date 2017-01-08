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
using System.Drawing;

namespace SVControl
{
    /// <summary>
    /// SVWPFFontDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFFontDialog : UserControl
    {
        public SVWPFFontDialog()
        {
            InitializeComponent();

            List<Font> fontList = new List<Font>()
            {
                new Font("华文细黑", 8),
                new Font("华文细黑", 12),
                new Font("华文细黑", 16)
            };

            this.listView.ItemsSource = fontList;
        }
    }
}
