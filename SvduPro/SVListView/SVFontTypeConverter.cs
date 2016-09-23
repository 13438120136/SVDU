using System;
using System.ComponentModel;
using System.Globalization;
using System.Drawing;

namespace SVControl
{
    [Serializable]
    public class SVFontTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "宋体:16", "宋体:24", "宋体:32" });
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

            String[] split = new String[] { ":" };
            String[] arg = str.Split(split, StringSplitOptions.RemoveEmptyEntries);
            Font font = new Font(arg[0], Int32.Parse(arg[1]));
            return font;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Font font = value as Font;
            if (font == null)
                return base.ConvertTo(context, culture, value, destinationType);

            return (font.Name + "," +font.Size + "pt");
        }
    }
}
