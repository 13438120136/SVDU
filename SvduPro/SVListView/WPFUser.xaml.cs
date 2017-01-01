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
using SVCore;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace SVControl
{
    /// <summary>
    /// WPFUser.xaml 的交互逻辑
    /// </summary>
    public partial class WPFUser : UserControl
    {
        List<String> picList = new List<String>();

        public WPFUser()
        {
            InitializeComponent();

            this.lv.ItemsSource = typeof(Colors).GetProperties();

            String file = System.IO.Path.Combine(SVProData.IconPath, "icon.proj");
            SVPixmapElementManage manage = new SVPixmapElementManage();
            manage.loadElementFromFile(file);
            
            Dictionary<String, List<String>> vDict = manage.getData();
            foreach (var item in vDict)
                picList.AddRange(item.Value);

            this.listViewPic.ItemsSource = picList;
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PropertyInfo proInfo = this.lv.SelectedItem as PropertyInfo;

            var color = (Color)System.Windows.Media.ColorConverter.ConvertFromString(proInfo.Name);
            SolidColorBrush myBrush = new SolidColorBrush(color);
            this.color.Fill = myBrush;
        }

        private void listViewPic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String str = this.listViewPic.SelectedItem as String;

            String infile = System.IO.Path.Combine(SVProData.IconPath, "icon.proj");
            SVPixmapElementManage manage = new SVPixmapElementManage();
            manage.loadElementFromFile(infile);

            String file1 = System.IO.Path.Combine(SVProData.IconPath, manage.getFilePathFromName(str));
            SVPixmapFile pixmap = new SVPixmapFile();
            pixmap.readPixmapFile(file1);
            System.Drawing.Image srcImg = System.Drawing.Image.FromStream(pixmap.Pixmap);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(srcImg);

            BitmapImage source = BitmapToBitmapImage(bitmap);
            this.picture.Source = source;

            SVPageProperties attrib = this.DataContext as SVPageProperties;
            attrib.PicIconData.ShowName = str;
            attrib.PicIconData.ImageFileName = manage.getFilePathFromName(str);
            //attrib.BackGroundType = (Byte)1;
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

    public class PicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //SVBitmap svBitmap = value as SVBitmap;
            //if (!svBitmap.isValidShow())
            //    return null;

            if (value == null)
                return null;

            String str = value as String;
            if (String.IsNullOrWhiteSpace(str))
                return null;

            String infile = System.IO.Path.Combine(SVProData.IconPath, "icon.proj");
            SVPixmapElementManage manage = new SVPixmapElementManage();
            manage.loadElementFromFile(infile);

            String file1 = System.IO.Path.Combine(SVProData.IconPath, manage.getFilePathFromName(str));
            SVPixmapFile pixmap = new SVPixmapFile();
            pixmap.readPixmapFile(file1);
            System.Drawing.Image srcImg = System.Drawing.Image.FromStream(pixmap.Pixmap);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(srcImg);

            return BitmapToBitmapImage(bitmap);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //String str = value as String;

            return value;
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

    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            System.Drawing.Color color = (System.Drawing.Color)value;
            System.Windows.Media.Color result = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            SolidColorBrush brush = value as SolidColorBrush;
            System.Windows.Media.Color color = brush.Color;
            System.Drawing.Color result = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

            return result;
        }
    }

    public class NameToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return value;

            String str = value as String;

            String infile = System.IO.Path.Combine(SVProData.IconPath, "icon.proj");
            SVPixmapElementManage manage = new SVPixmapElementManage();
            manage.loadElementFromFile(infile);

            String file1 = System.IO.Path.Combine(SVProData.IconPath, manage.getFilePathFromName(str));
            SVPixmapFile pixmap = new SVPixmapFile();
            pixmap.readPixmapFile(file1);
            System.Drawing.Image srcImg = System.Drawing.Image.FromStream(pixmap.Pixmap);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(srcImg);

            return BitmapToBitmapImage(bitmap);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
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
