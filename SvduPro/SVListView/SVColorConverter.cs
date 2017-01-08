using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace SVControl
{
    public class SVColorConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(Color))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Color color = (Color)value;
            if (color == null)
                return base.ConvertFrom(context, culture, value);

            return color;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(Color))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Color color = (Color)value;
            if (color == null)
                return base.ConvertTo(context, culture, value, destinationType);

            return color.Name;
        }
    }
}
