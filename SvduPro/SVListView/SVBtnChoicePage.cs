using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SVControl
{
    [Serializable]
    public class SVBtnChoicePage : TypeConverter, ISerializable
    {
        UInt16 _pageID;                 ///页面ID号
        String _pageText = "";          ///页面名称显示


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

        /// <summary>
        /// 判断两个对象的值是否相同
        /// </summary>
        /// <param Name="other">比较的另外一个输入对象</param>
        /// <returns>true-相等  false-不想等</returns>
        public Boolean isEqual(SVBtnChoicePage other)
        {
            return true;
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
            SVBtnChoicePage str = value as SVBtnChoicePage;
            if (str == null)
                return base.ConvertTo(context, culture, value, destinationType);

            if (String.IsNullOrWhiteSpace(str.PageText))
                return "无跳转页面";
            
            return String.Format("跳转到: {0}", str.PageText);
        }

        /// <summary>
        /// 自定义按钮类型构造函数
        /// </summary>
        public SVBtnChoicePage()
        {
        }

        protected SVBtnChoicePage(SerializationInfo info, StreamingContext context)
        {
            PageID = (UInt16)info.GetValue("PageID", typeof(UInt16));
            PageText = (String)info.GetValue("PageText", typeof(String));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PageID", PageID);
            info.AddValue("PageText", PageText);
        }

        public SVBtnChoicePage cloneObject()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, this);
            stream.Position = 0;
            SVBtnChoicePage result = (SVBtnChoicePage)binFormat.Deserialize(stream);

            return result;
        }
    }
}
