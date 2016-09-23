using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace SVCore
{
    public class SVSelectAlignProperty : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> strArray = new List<string>();
            strArray.Add("左对齐");
            strArray.Add("右对齐");
            strArray.Add("居中对齐");
            strArray.Add("水平和垂直居中");
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
            return str;
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
