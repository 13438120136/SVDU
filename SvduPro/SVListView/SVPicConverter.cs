using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using SVCore;
using System.Windows.Controls;

namespace SVControl
{
    public class SVPicConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            String fileShowName = value as String;
            if (String.IsNullOrWhiteSpace(fileShowName))
                return null;

            String infile = System.IO.Path.Combine(SVProData.IconPath, "icon.proj");
            SVPixmapElementManage manage = new SVPixmapElementManage();
            manage.loadElementFromFile(infile);

            String imageFile = manage.getFilePathFromName(fileShowName);
            if (imageFile == null)
                return null;

            String file1 = System.IO.Path.Combine(SVProData.IconPath, imageFile);
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
