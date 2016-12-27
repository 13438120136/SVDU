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
    /// SVWPFSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFSettingWindow : Window
    {
        public SVWPFSettingWindow()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {            
            SVConfig config = SVConfig.instance();
            config.SaveInterval = Int32.Parse(this.textBox.Text);
            config.saveConfig();
            this.Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
