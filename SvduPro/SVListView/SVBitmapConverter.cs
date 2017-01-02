using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using SVCore;

namespace SVControl
{
    public class SVBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            SVBitmap bitmap = value as SVBitmap;
            if (bitmap == null)
                return null;

            String infile = System.IO.Path.Combine(SVProData.IconPath, "icon.proj");
            SVPixmapElementManage manage = new SVPixmapElementManage();
            manage.loadElementFromFile(infile);

            String file1 = System.IO.Path.Combine(SVProData.IconPath, manage.getFilePathFromName(bitmap.ShowName));
            SVPixmapFile pixmap = new SVPixmapFile();
            pixmap.readPixmapFile(file1);
            System.Drawing.Image srcImg = System.Drawing.Image.FromStream(pixmap.Pixmap);
            System.Drawing.Bitmap oBitmap = new System.Drawing.Bitmap(srcImg);

            return BitmapToBitmapImage(oBitmap);
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
