using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace SVControl
{
    public class SVButtonTypeConverter : TypeConverter
    {
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
                    return "跳转页面";
                case 1:
                    return "打开设备";
                case 2:
                    return "关闭设备";
                case 3:
                    return "变量翻转";
                case 4:
                    return "模拟量递增";
                case 5:
                    return "模拟量递减";
                case 6:
                    return "前进";
                case 7:
                    return "当前";
                case 8:
                    return "后退";
                default:
                    return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
