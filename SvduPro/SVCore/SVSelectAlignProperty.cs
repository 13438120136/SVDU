using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace SVCore
{
    public class SVSelectAlignProperty : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        //public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        //{
        //    List<String> strArray = new List<string>();
        //    strArray.Add("左对齐");
        //    strArray.Add("右对齐");
        //    strArray.Add("居中对齐");
        //    strArray.Add("水平和垂直居中");
        //    return new StandardValuesCollection(strArray.ToArray());
        //}

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(Byte))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            String str = value as String;
            if (str == null)
                return base.ConvertFrom(context, culture, value);
            return str;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(Byte))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Byte str = (Byte)value;
            switch (str)
            {
                case 0:
                    return "左对齐";
                case 1:
                    return "右对齐";
                case 2:
                    return "居中对齐";
                case 3:
                    return "水平和垂直居中";
                default:
                    return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
