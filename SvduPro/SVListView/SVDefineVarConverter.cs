using System;
using System.ComponentModel;
using System.Globalization;
using SVCore;

namespace SVControl
{
    public class SVDefineVarConverter : TypeConverter
    {
        /// <summary>
        /// 重写父类，这里不需要关注
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            SVVarDefine varDefine = value as SVVarDefine;
            if (varDefine == null)
                return base.ConvertTo(context, culture, value, destinationType);

            if (String.IsNullOrWhiteSpace(varDefine.VarName))
                return "None";

            return varDefine.VarName;
        }
    }
}
