using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace SVControl
{
    public class SVCurveVarConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            List<SVCurveProper> proper = value as List<SVCurveProper>;
            if (proper == null)
                return base.ConvertTo(context, culture, value, destinationType);

            return proper.Count.ToString();
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }
    }
}
