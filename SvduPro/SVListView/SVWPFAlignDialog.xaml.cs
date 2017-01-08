using System.Windows;
using System.Windows.Controls;
using System;

namespace SVControl
{
    /// <summary>
    /// SVWPFAlignDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFAlignDialog : UserControl
    {
        public Byte AlignValue { get; set; }
        public Action CloseEvent = () => { };

        public SVWPFAlignDialog()
        {
            InitializeComponent();
        }

        private void lButton_Click(object sender, RoutedEventArgs e)
        {
            AlignValue = 0;
            CloseEvent();
        }

        private void rButton_Click(object sender, RoutedEventArgs e)
        {
            AlignValue = 1;
            CloseEvent();
        }

        private void cButton_Click(object sender, RoutedEventArgs e)
        {
            AlignValue = 2;
            CloseEvent();
        }

        private void hvButton_Click(object sender, RoutedEventArgs e)
        {
            AlignValue = 3;
            CloseEvent();
        }
    }
}
