using System;
using System.Collections.Generic;
using System.Windows.Controls;
using SVCore;
using System.Windows;
using System.ComponentModel;

namespace SVControl
{
    /// <summary>
    /// SVWPFHeartPic.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPFHeartPic : UserControl
    {
        //图片的分类及对应的图片名称列表
        Dictionary<String, List<String>> _iconData = null;
        SVPixmapElementManage _picManager = new SVPixmapElementManage();

        public SVWPFHeartPic()
        {
            InitializeComponent();

            String file = System.IO.Path.Combine(SVProData.IconPath, "icon.proj");            
            _picManager.loadElementFromFile(file);
            _iconData = _picManager.getData();

            this.comboboxType.ItemsSource = _iconData.Keys;
        }

        private void OnMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item == null)
                return;

            var currItem = item.Content as String;
            if (currItem == null)
                return;

            item.IsSelected = true;
            DragDrop.DoDragDrop(this.listListView, currItem, DragDropEffects.Copy);
        }

        private void comboboxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///分类名称
            String typeName = this.comboboxType.SelectedValue as String;

            if (!_iconData.ContainsKey(typeName))
            {
                this.listListView.ItemsSource = null;
                return;
            }

            this.listListView.ItemsSource = _iconData[typeName];
        }

        private void resultListView_DragEnter(object sender, DragEventArgs e)
        {
            if (this.resultListView.Items.Count >= 8)
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            e.Effects = DragDropEffects.Copy;
        }

        private void resultListView_Drop(object sender, DragEventArgs e)
        {
            String dataString = e.Data.GetData(typeof(String)) as String;
            if (dataString == null)
                return;

            List<SVBitmap> result = this.resultListView.ItemsSource as List<SVBitmap>;
            SVBitmap svBitmap = new SVBitmap();
            svBitmap.ShowName = dataString;
            svBitmap.ImageFileName = _picManager.getFilePathFromName(dataString);
            result.Add(svBitmap);
            this.resultListView.Items.Refresh();
        }

        private void resultListView_DragOver(object sender, DragEventArgs e)
        {
            if (this.resultListView.Items.Count >= 8)
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            e.Effects = DragDropEffects.Copy;
        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            Int32 currIndex = this.resultListView.SelectedIndex;
            if (currIndex == -1)
                return;

            List<SVBitmap> result = this.resultListView.ItemsSource as List<SVBitmap>;
            result.RemoveAt(currIndex);
            
            this.resultListView.Items.Refresh();
            this.resultListView.SelectedIndex = currIndex;
        }

        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            if (this.resultListView.Items.Count >= 8)
                return;

            String dataString = this.listListView.SelectedValue as String;
            if (dataString == null)
                return;

            List<SVBitmap> result = this.resultListView.ItemsSource as List<SVBitmap>;
            SVBitmap svBitmap = new SVBitmap();
            svBitmap.ShowName = dataString;
            svBitmap.ImageFileName = _picManager.getFilePathFromName(dataString);
            result.Add(svBitmap);

            this.resultListView.Items.Refresh();
        }
    }
}
