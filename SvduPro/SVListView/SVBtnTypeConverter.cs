using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SVControl
{
    [Serializable]
    public class SVBtnTypeConverter : TypeConverter, ISerializable
    {
        Byte _type;                     ///当前按钮类型,0-表示切换页面 1-为操作设备
        UInt16 _pageID;                 ///页面ID号
        String _pageText = "";          ///页面名称显示
        String _varText = "";           ///变量名称显示
        Boolean _enable = false;        ///是否使能，如果为true表示使能，否则为禁用
        String _enVarText = "";         ///使能关联变量显示名称
        Byte _enVarTextType;
        Byte _varTextType;

        public Byte VarTextType
        {
            get { return _varTextType; }
            set { _varTextType = value; }
        }

        public Byte EnVarTextType
        {
            get { return _enVarTextType; }
            set { _enVarTextType = value; }
        }

        public String EnVarText
        {
            get { return _enVarText; }
            set { _enVarText = value; }
        }

        /// <summary>
        /// 是否使能 
        /// true-表示使能 false-禁用
        /// </summary>
        public Boolean Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        public UInt16 PageID
        {
            get { return _pageID; }
            set { _pageID = value; }
        }        

        public String PageText
        {
            get { return _pageText; }
            set { _pageText = value; }
        }        

        public String VarText
        {
            get { return _varText; }
            set { _varText = value; }
        }

        public Byte Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 判断两个对象的值是否相同
        /// </summary>
        /// <param Name="other">比较的另外一个输入对象</param>
        /// <returns>true-相等  false-不想等</returns>
        public Boolean isEqual(SVBtnTypeConverter other)
        {
            if (Type == other.Type && PageID == other.PageID
                && PageText == other.PageText
                && VarText == other.VarText
                && _enable == other._enable
                && _enVarText == other._enVarText)
                return true;

            return false;
        }

        /// <summary>
        /// 重写ConvertTo函数，用来自定义显示属性内容
        /// </summary>
        /// <param Name="context"></param>
        /// <param Name="culture"></param>
        /// <param Name="value"></param>
        /// <param Name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            SVBtnTypeConverter str = value as SVBtnTypeConverter;
            if (str == null)
                return base.ConvertTo(context, culture, value, destinationType);

            if (str.Type == 0)
                return String.Format("跳转到: {0}", str.PageText);
            else
                return String.Format("操作变量: {0}", str.VarText);
        }

        /// <summary>
        /// 自定义按钮类型构造函数
        /// </summary>
        public SVBtnTypeConverter()
        {
        }

        protected SVBtnTypeConverter(SerializationInfo info, StreamingContext context)
        {
            Type = (Byte)info.GetValue("Type", typeof(Byte));
            PageID = (UInt16)info.GetValue("PageID", typeof(UInt16));
            VarText = (String)info.GetValue("VarText", typeof(String));
            PageText = (String)info.GetValue("PageText", typeof(String));
            Enable = (Boolean)info.GetValue("Enable", typeof(Boolean));
            EnVarText = (String)info.GetValue("EnVarText", typeof(String));
            EnVarTextType = (Byte)info.GetValue("EnVarTextType", typeof(Byte));
            VarTextType = (Byte)info.GetValue("VarTextType", typeof(Byte)); 
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Type", Type);
            info.AddValue("PageID", PageID);
            info.AddValue("VarText", VarText);
            info.AddValue("PageText", PageText);
            info.AddValue("Enable", Enable);
            info.AddValue("EnVarText", EnVarText);
            info.AddValue("EnVarTextType", EnVarTextType);
            info.AddValue("VarTextType", VarTextType);
        }

        public SVBtnTypeConverter cloneObject()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, this);
            stream.Position = 0;
            SVBtnTypeConverter result = (SVBtnTypeConverter)binFormat.Deserialize(stream);

            return result;
        }
    }
}
