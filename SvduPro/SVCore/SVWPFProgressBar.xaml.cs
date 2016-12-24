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
using System.Globalization;

namespace SVCore
{
    /// <summary>
    /// SVWPFProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFProgressBar : Window
    {
        public ProgressBar Pro { set{this.ProgressBar = value;} get { return this.ProgressBar; } }

        private double MaxValue { get; set; }

        public SVWPFProgressBar()
        {
            InitializeComponent();
            var converter = (ProgressValueConverter)FindResource("valueConverter");
            converter.WidthLen = (Int32)this.Width;
        }

        public void setText(String text)
        {
            this.box.Text = text;
            this.ProgressBar.DataContext = this;
        }

        public void setMaxBarValue(double value)
        {
            var converter = (ProgressValueConverter)FindResource("valueConverter");
            this.ProgressBar.Maximum = 100;
            MaxValue = value;
        }

        public void setBarValue(double value)
        {
            this.ProgressBar.Value = System.Convert.ToInt32(value * 100 / MaxValue);
        }

        private void ProgressBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }

    public class ProgressValueConverter : IValueConverter
    {
        public Int32 WidthLen { get; set; }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            double dValue = System.Convert.ToDouble(value);
            double result = (dValue * WidthLen) / 100;
            return result;
            // Do the conversion from bool to visibility
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // Do the conversion from visibility to bool
            return null;
        }
    }
}
