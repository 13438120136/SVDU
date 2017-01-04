using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace SVControl
{
    [Serializable]
    public class SVFontTypeConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(Font))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Font font = value as Font;
            if (font == null)
                return base.ConvertFrom(context, culture, value);

            return font;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(Font))
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
