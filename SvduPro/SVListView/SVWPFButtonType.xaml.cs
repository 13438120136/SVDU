﻿using System;
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

namespace SVControl
{
    /// <summary>
    /// SVWPFButtonType.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFButtonType : UserControl
    {
        public Action CloseEvent;

        public SVWPFButtonType()
        {
            InitializeComponent();

            CloseEvent = new Action(() => { });
        }

        private void listView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseEvent();
        }
    }
}