using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace SVCore
{
    /// <summary>
    /// SVWpfColorDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SVWpfColorDialog : UserControl
    {
        public Color SelectColor { get; set; }

        public SVWpfColorDialog()
        {
            InitializeComponent();
        }

        public void setDataContext(Color color)
        {
            this.canvas.SelectedColor = color;
            SelectColor = color;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            SolidColorBrush brush = btn.Background as SolidColorBrush;
            SelectColor = brush.Color;
            this.canvas.SelectedColor = brush.Color;
        }

        private void SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            SelectColor = this.canvas.SelectedColor;
        }

    }
}
