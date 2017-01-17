using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SVCore;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Globalization;

namespace SVControl
{
    /// <summary>
    /// SVWPfIconPic.xaml 的交互逻辑
    /// </summary>
    public partial class SVWPfIconPic : UserControl
    {
        //图片的分类及对应的图片名称列表
        Dictionary<String, List<String>> _iconData = null;
        SVPixmapElementManage _picManager = new SVPixmapElementManage();
        SVBitmap _resultBitmap = new SVBitmap();

        public SVWPfIconPic()
        {
            InitializeComponent();

            String file = System.IO.Path.Combine(SVProData.IconPath, "icon.proj");
            _picManager.loadElementFromFile(file);
            _iconData = _picManager.getData();

            this.comboboxType.ItemsSource = _iconData.Keys;
        }

        public SVBitmap resultBitmap()
        {
            return _resultBitmap;
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

        private void OnMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item == null)
                return;

            var text = item.Content as String;
            if (text == null)
                return;

            item.IsSelected = true;

            String file = System.IO.Path.Combine(SVProData.IconPath, _picManager.getFilePathFromName(text));
            SVPixmapFile pixmap = new SVPixmapFile();
            pixmap.readPixmapFile(file);
            System.Drawing.Image srcImg = System.Drawing.Image.FromStream(pixmap.Pixmap);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(srcImg);
            BitmapImage source = BitmapToBitmapImage(bitmap);
            this.image.Source = source;

            _resultBitmap.ShowName = text;
            _resultBitmap.ImageFileName = _picManager.getFilePathFromName(text);
            //SVIcon icon = this.DataContext as SVIcon;
            //icon.Attrib.PicIconData.ShowName = text;
            //icon.Attrib.PicIconData.ImageFileName = _picManager.getFilePathFromName(text);
        }


        private BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }
    }
}
