/*
 * 对xml文件的读写的简单封装，方便后面对xml的操纵
 */
using System;
using System.Collections.Generic;
using System.Xml;

namespace SVCore
{
    public class SVXml
    {
        private XmlElement _RootElement;
        public XmlElement CurrentElement { get; set; }
        private XmlDocument _xmlDoc = null;

        public SVXml()
        {
            _xmlDoc = new XmlDocument(); 
            XmlDeclaration xmldecl = _xmlDoc.CreateXmlDeclaration("1.0", "gb2312", "yes");

            _xmlDoc.AppendChild(xmldecl);
        }

        /*
         * 加载XML文件
         * xmlFileName - xml文件名
         * 加载成功 - true, 加载失败 - false
         */
        public Boolean loadXml(String xmlFileName)
        {
            try
            {
                _xmlDoc.Load(xmlFileName);
            }
            catch (Exception )
            {
                return false;
            }

            return true;
        }

        /*
         * 根据输入名称初始化根节点
         * 成功-true  失败-false
         * */
        public Boolean initRootEle(String name)
        {
            _RootElement = _xmlDoc.DocumentElement;
            if (_RootElement == null)
                return false;

            CurrentElement = _RootElement;
            _xmlDoc.AppendChild(_RootElement);
            return true;
        }

        /**
         * 创建根节点
         * 返回：void
         */
        public void createRootEle(String name)
        {
            _RootElement = _xmlDoc.CreateElement(name);
            CurrentElement = _RootElement;
            _xmlDoc.AppendChild(_RootElement);
        }

        public XmlElement select(String nodeName)
        {
            return (XmlElement)CurrentElement.SelectSingleNode(nodeName);
        }

        public List<XmlElement> selectChilds()
        {
            List<XmlElement> list = new List<XmlElement>();
            XmlNodeList nls = CurrentElement.ChildNodes;
            foreach (var tmp in nls)
                list.Add(tmp as XmlElement);

            return list;
        }

        public XmlElement createNode(String nodeName)
        {
            XmlElement tmp = _xmlDoc.CreateElement(nodeName);
            CurrentElement.AppendChild(tmp);

            return tmp;
        }

        public XmlElement crateChildNode(String nodeName)
        {
            return _xmlDoc.CreateElement(nodeName);
        }

        public void setAttr(String key, String value)
        {
            CurrentElement.SetAttribute(key, value);
        }

        public void setAttr(Dictionary<String, String> dictValue)
        {
            foreach (var item in dictValue)
                CurrentElement.SetAttribute(item.Key, item.Value);
        }

        public void setValue(String value)
        {
            CurrentElement.InnerText = value;
        }

        public void writeXml(String xmlFileName)
        {
            _xmlDoc.Save(xmlFileName);
        }
    }
}
