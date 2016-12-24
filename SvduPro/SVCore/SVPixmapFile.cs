using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace SVCore
{
    public class SVPixmapFile
    {
        /// <summary>
        /// 图片要显示的名称
        /// </summary>
        public String ShowName { get; set; }

        /// <summary>
        /// 图片本身的图片数据对象
        /// </summary>
        public MemoryStream Pixmap { get; set; }

        /// <summary>
        /// 文件尺寸
        /// </summary>
        Int64 _bitFileSize;

        //读取图标文件
        public void readPixmapFile(String filename)
        {
            FileStream fstream = new FileStream(filename, FileMode.Open);
            BinaryReader binRead = new BinaryReader(fstream);

            ShowName = binRead.ReadString();
            _bitFileSize = binRead.ReadInt64();
            byte[] bytes = binRead.ReadBytes((Int32)_bitFileSize);
            Pixmap = new MemoryStream(bytes);

            binRead.Close();
            fstream.Close();
        }

        //写入图标文件
        public void writePixmapFile(String filename)
        {
            FileStream fstream = new FileStream(filename, FileMode.OpenOrCreate);
            BinaryWriter binWrite = new BinaryWriter(fstream);

            binWrite.Write(ShowName);      
            binWrite.Write(Pixmap.Length);
            binWrite.Write(Pixmap.ToArray());
            _bitFileSize = Pixmap.Length;

            binWrite.Close();
            fstream.Close();
        }

        //获取标准的位图文件对象
        public Bitmap getBitmapFromData()
        {
            return (Bitmap)Bitmap.FromStream(this.Pixmap);
        }

        //根据输入的宽和高返回8位的位图对象
        /// <summary>
        /// 返回图片指定图片对象
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="mark">标志：8:返回8位图片，24返回24位图片</param>
        /// <returns>具体的内存图片对象</returns>
        public Bitmap getBitmapObject(Int32 width, Int32 height, Int32 mark)
        {
            Bitmap origin = getBitmapFromData();

            Bitmap img = new Bitmap(width, height);
            img.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(img);
            grPhoto.CompositingMode = CompositingMode.SourceCopy;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

            grPhoto.DrawImage(origin, new Rectangle(0, 0, width, height), new Rectangle(0, 0, origin.Width, origin.Height), GraphicsUnit.Pixel);
            img.RotateFlip(RotateFlipType.Rotate180FlipX);
            //Bitmap upBitmap = KiRotate(img, 180.0f, Color.Transparent);

            switch (mark)
            {
                case 8:
                    {
                        Bitmap bitmapResult = img.Clone(new Rectangle(0, 0, img.Width, img.Height), PixelFormat.Format8bppIndexed);
                        return bitmapResult;
                    }
                case 24:
                    {
                        Bitmap bitmapResult = img.Clone(new Rectangle(0, 0, img.Width, img.Height), PixelFormat.Format32bppRgb);
                        return bitmapResult;
                    }
                default:
                    return null;
            }
        }

        //上下反转图片
        public Bitmap RevPic(Bitmap inputBitmap)
        {
            int width = inputBitmap.Width;
            int height = inputBitmap.Height;

            Bitmap bitmapReturn = new Bitmap(width, height);
            int x, y, z;
            Color pixel;

            for (x = 0; x < width; x++)
            {
                for (y = height - 1, z = 0; y >= 0; y--)
                {
                    pixel = inputBitmap.GetPixel(x, y);//获取当前像素的值
                    bitmapReturn.SetPixel(x, z++, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图
                }
            }
            return bitmapReturn;//返回翻转后的图片
        }

        public Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

        /// <summary>
        /// 从图片数据的指定位置读取一副图片并返回
        /// </summary>
        /// <param name="buffer">图片数据</param>
        /// <param name="offset">偏移位置</param>
        /// <returns>图片对象</returns>
        public Bitmap getFromFile(byte[] buffer, UInt32 offset)
        {
            Int32 length = (Int32)(buffer.Length - offset);
            byte[] vTmp = new byte[length];
            Buffer.BlockCopy(buffer, (Int32)offset, vTmp, 0, length);

            MemoryStream stream = new MemoryStream(vTmp);
            Bitmap vtmp = (Bitmap)Bitmap.FromStream(stream);

            ///进行180旋转
            vtmp.RotateFlip(RotateFlipType.Rotate180FlipX);
            return vtmp;
        }
    }
}
