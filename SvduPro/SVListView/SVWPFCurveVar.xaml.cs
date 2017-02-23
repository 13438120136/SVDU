using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;

namespace SVControl
{
    /// <summary>
    /// SVWPFCurveVar.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFCurveVar : Window
    {
        PropertyGrid _propertyGrid = new PropertyGrid();

        public SVWPFCurveVar()
        {
            InitializeComponent();

            _propertyGrid.Font = new Font(_propertyGrid.Font.Name, _propertyGrid.Font.Size + 2);
            _propertyGrid.Size = new System.Drawing.Size(180, 180);

            this.winFormHost.Child = _propertyGrid;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _propertyGrid.SelectedObject = this.listView.SelectedItem;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            List<SVCurveProper> valueList =  this.listView.ItemsSource as List<SVCurveProper>;
            if (valueList.Count >= 4)
                return;

            valueList.Add(new SVCurveProper());
            this.listView.Items.Refresh();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            List<SVCurveProper> valueList = this.listView.ItemsSource as List<SVCurveProper>;
            var value = this.listView.SelectedValue;
            if (value == null)
                return;

            valueList.Remove((SVCurveProper)value);
            this.listView.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        
    }

    public class CurveVarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Drawing.Color color = (System.Drawing.Color)value;

            if (color.IsKnownColor)
                return color.Name;
            else
                return "#" + color.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CurveVarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SVCurveProper proper = value as SVCurveProper;
            if (proper == null)
                return null;

            return proper.Var.VarName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
