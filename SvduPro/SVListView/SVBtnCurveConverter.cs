using System;
using System.ComponentModel;
using System.Globalization;

namespace SVControl
{
    class SVBtnCurveConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
                return base.ConvertTo(context, culture, value, destinationType);

            String id = value as String;
            if (String.IsNullOrWhiteSpace(id))
                return "无";

            return "趋势图:" + id;
        }
    }
}
