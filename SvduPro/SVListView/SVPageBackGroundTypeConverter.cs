using System;
using System.ComponentModel;
using System.Globalization;

namespace SVControl
{
    public class SVPageBackGroundTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Byte bValue = Convert.ToByte(value);
            switch (bValue)
            {
                case 0:
                    return "颜色显示";
                case 1:
                    {
                        var proper = context.Instance as SVPageWidget;
                        if (proper != null)
                            return proper.Attrib.PicIconData.ShowName;
                        else
                            return "图片显示";
                    }
                default:
                    return base.CanConvertTo(context, destinationType);
            }
        }
    }
}
