using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace SVControl
{
    [Serializable]
    public class SVBinaryTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> strArray = new List<string>();
            strArray.Add("打开:关闭");
            strArray.Add("运行:停止");
            strArray.Add("1:0");
            strArray.Add("是:否");
            strArray.Add("真:假");
            strArray.Add("正确:错误");
            strArray.Add("开:关");
            strArray.Add("自定义");
            return new StandardValuesCollection(strArray.ToArray());
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            String str = value as String;
            if (str == null)
                return base.ConvertFrom(context, culture, value);

            if (str == "自定义")
                return str;

            String[] split = new String[] { ":" };
            String[] arg = str.Split(split, StringSplitOptions.RemoveEmptyEntries);
            String result = arg[0] +" or "+ arg[1];
            return result;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            String str = value as String;
            if (str == null)
                return base.ConvertTo(context, culture, value, destinationType);

            return str;
        }
    }
}
