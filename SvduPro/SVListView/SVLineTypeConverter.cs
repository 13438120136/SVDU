using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace SVControl
{
    /// <summary>
    /// 线条属性自定义类,
    /// 用来修饰当前线条控件的显示(分为:横向，纵向)
    /// </summary>
    [Serializable]
    public class SVLineTypeConverter : StringConverter
    {
        /// <summary>
        /// 重写父类
        /// 这里不需要关注
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// 重写父类
        /// 这里不需要关注
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ///列表有[横向][纵向]两个字段
            return new StandardValuesCollection(new string[] { "水平", "垂直" });
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            String strValue = value as String;
            if (String.IsNullOrEmpty(strValue))
                return base.ConvertFrom(context, culture, value);

            if (strValue == "水平")
                return true;
            else
                return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(Boolean))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Boolean)
            {
                Boolean bValue = (Boolean)value;
                if (bValue)
                    return "水平";
                else
                    return "垂直";
            }
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
