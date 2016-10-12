using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace SVCore
{
    /// <summary>
    /// 针对自定义的图片对象，封装的图片数组类。
    /// </summary>
    [Serializable]
    public class SVBitmapArray : TypeConverter, ISerializable
    {
        /// <summary>
        /// 图片数组
        /// </summary>
        public List<SVBitmap> BitmapArray { get; set; }

        protected SVBitmapArray(SerializationInfo info, StreamingContext context)
        {
            BitmapArray = (List<SVBitmap>)info.GetValue("stream", typeof(List<SVBitmap>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("stream", BitmapArray);
        }

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(SVBitmapArray))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            SVBitmapArray bitmap = value as SVBitmapArray;
            if (bitmap == null)
                return base.ConvertFrom(context, culture, value);

            return bitmap;
        }

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(SVBitmapArray))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            SVBitmapArray bitmap = value as SVBitmapArray;
            if (bitmap == null)
                return base.ConvertTo(context, culture, value, destinationType);

            if (bitmap.BitmapArray != null)
            {
                String text = String.Format("个数:{0}", bitmap.imageArray().Count);
                return text;
            }
            else
            {
                return "空!";
            }
        }

        public SVBitmapArray()
        {
            BitmapArray = new List<SVBitmap>();
        }

        /// <summary>
        /// 获取当前实际图片个数
        /// </summary>
        /// <returns>图片个数</returns>
        public List<SVBitmap> imageArray()
        {
            List<SVBitmap> result = new List<SVBitmap>();

            if (BitmapArray == null)
                return result;

            for (int i = 0; i < BitmapArray.Count; i++)
            {
                SVBitmap vTmp = BitmapArray[i];
                if (vTmp == null)
                    continue;

                if (!vTmp.isValidShow())
                    continue;

                result.Add(vTmp);
            }

            return result;
        }
    }
}
