using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace SVControl
{
    [Serializable]
    public class SVBinaryTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection collection = TypeDescriptor.GetProperties(value, true);

            List<PropertyDescriptor> list = new List<PropertyDescriptor>();
            list.Add(collection["ShowDetail"]);

            if ((value as SVBinaryProperties) != null)
            {
                list.Add(collection["Address1"]);
                list.Add(collection["Address2"]);
            }
            else
            {
                list.Add(collection["Summary"]);
            }

            return new PropertyDescriptorCollection(list.ToArray());
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Byte bValue = (Byte)value;
            switch (bValue)
            {
                case 0:
                    return "文本显示";
                case 1:
                    return "图片显示";
                default:
                    return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
