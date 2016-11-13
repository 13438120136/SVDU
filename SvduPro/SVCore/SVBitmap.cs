using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Drawing;
using System.IO;

namespace SVCore
{
    /// <summary>
    /// 自定义的图片类，用来修饰属性窗口中的图片字段
    /// 该图片类，不保证实际的图片数据，只记录图片的路径以及界面显示名称
    /// </summary>
    [Serializable]
    public class SVBitmap : TypeConverter, ISerializable
    {
        /// <summary>
        /// 图片的文件名
        /// </summary>
        public String ImageFileName { get; set; }

        /// <summary>
        /// 界面显示的名称
        /// </summary>
        public String ShowName { get; set; }

        /// <summary>
        /// 判断当前的图片对象是否被设置为有效
        /// </summary>
        /// <returns>true-合法，false-不合法</returns>
        public Boolean isValidShow()
        {
            return !(String.IsNullOrEmpty(ImageFileName) || String.IsNullOrEmpty(ShowName));
        }

        public Bitmap bitmap()
        {
            Bitmap result = null;

            try
            {
                String file = Path.Combine(SVProData.IconPath, ImageFileName);
                SVPixmapFile pixmapFile = new SVPixmapFile();
                pixmapFile.readPixmapFile(file);
                result = pixmapFile.getBitmapFromData();
            }
            catch
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// 获取图片数据
        /// </summary>
        /// <returns></returns>
        public byte[] bitmap8Data(int width, int height)
        {
            try
            {
                String picFile = Path.Combine(SVProData.IconPath, ImageFileName);
                SVPixmapFile file = new SVPixmapFile();
                file.readPixmapFile(picFile);
                Bitmap bitmap = file.get8Bitmap(width, height);
                SVBitmapHead head = new SVBitmapHead(bitmap);
                return head.data();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        //public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        //{
        //    return true;
        //}

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(SVBitmap))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            ///如果为SVBitmap对象，才执行父类操作
            SVBitmap bitmap = value as SVBitmap;
            if (bitmap == null)
                return base.ConvertFrom(context, culture, value);

            return bitmap;
        }

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(SVBitmap))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            SVBitmap bitmap = value as SVBitmap;
            if (bitmap == null)
                return base.ConvertTo(context, culture, value, destinationType);

            return bitmap.ShowName;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SVBitmap()
        {
        }

        /// <summary>
        /// 序列化需要的构造函数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SVBitmap(SerializationInfo info, StreamingContext context)
        {
            ImageFileName = (String)info.GetValue("ImageFilePath", typeof(String));
            ShowName = (String)info.GetValue("ShowName", typeof(String));
        }

        /// <summary>
        /// 这里执行序列化是为了，使其复制产生一个新的对象而不是引用
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ImageFilePath", ImageFileName);
            info.AddValue("ShowName", ShowName);
        }
    }
}
