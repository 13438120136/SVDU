using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using SVCore;

namespace SVControl
{
    /// <summary>
    /// 控件线条类
    /// </summary>
    [Serializable]
    public class SVLine : SVPanel, SVInterfacePanel, ISerializable, SVInterfaceBuild
    {
        /// <summary>
        /// 直线控件对应的属性字段
        /// </summary>
        SVLineProperties _attrib = new SVLineProperties();

        /// <summary>
        /// 直线属性
        /// </summary>
        public SVLineProperties Attrib
        {
            get { return _attrib; }
            set { _attrib = value; }
        }

        /// <summary>
        /// 线条的构造函数
        /// </summary>
        public SVLine()
        {
            ///透明
            this.BTransparent = true;
            ///用户改变后刷新界面
            this.SizeChangedEvent = new Action(changeEvent);
        }

        /// <summary>
        /// 当线条的宽度和长度通过用户操作发生改变后
        /// 需要更新对应的属性字段
        /// </summary>
        void changeEvent()
        {
            if (_attrib.ShowType)
            {
                _attrib.LineWidth = (Byte)this.Height;
                _attrib.LineLength = (UInt16)this.Width;
            }
            else
            {
                _attrib.LineLength = (UInt16)this.Height;
                _attrib.LineWidth = (Byte)this.Width;
            }

            _attrib.start = this.Location;
        }

        /// <summary>
        /// 重写父类的initalizeRedoUndo
        /// </summary>
        override public void initalizeRedoUndo()
        {
            this.RedoUndo.UpdateOperator += () =>
            {
                refreshPropertyToPanel();
            };

            Attrib.UpdateControl += new UpdateControl((item) =>
            {
                RedoUndo.recordOper(item);
            });
        }

        /// <summary>
        /// 重写父类的方法
        /// 这里不需要关注
        /// </summary>
        public override void createID()
        {
            _attrib.ID = (UInt16)SVUniqueID.instance().newUniqueID();
        }

        /// <summary>
        /// 重写父类的方法
        /// 这里不需要关注
        /// </summary>
        public override void delID()
        {
            SVUniqueID.instance().delUniqueID((Int16)_attrib.ID);
        }

        /// <summary>
        /// 重写父类的复制构造函数
        /// 这里不需要具体关注
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SVLine(SerializationInfo info, StreamingContext context)
        {
            _attrib = (SVLineProperties)info.GetValue("stream", typeof(SVLineProperties));
        }

        /// <summary>
        /// 重写父类的方法，用来执行序列化操作的复制
        /// 这里不需要具体关注
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("stream", _attrib);
        }

        /// <summary>
        /// 创建一个副本
        /// </summary>
        /// <returns>新的副本对象</returns>
        override public object cloneObject()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, this);
            stream.Position = 0;
            SVLine result = (SVLine)binFormat.Deserialize(stream);

            result.refreshPropertyToPanel();

            return result;
        }

        /// <summary>
        /// 设置当前控件起始位置
        /// </summary>
        /// <param name="pos"></param>
        public override void setStartPos(Point pos)
        {
            _attrib.start = pos;
        }

        /// <summary>
        /// 获取属性对象
        /// </summary>
        /// <returns>线条属性对象</returns>
        public override object property()
        {
            return _attrib;
        }

        /// <summary>
        /// 工作区中刷新线条的显示效果
        /// </summary>
        public override void refreshPropertyToPanel()
        {
            this.Id = _attrib.ID;

            if (_attrib.ShowType)
            {
                setNodeType(SVNodesType.Horizontal);
                this.Height = _attrib.LineWidth;
                this.Width = _attrib.LineLength;
            }
            else
            {
                setNodeType(SVNodesType.Vertical);
                this.Height = _attrib.LineLength;
                this.Width = _attrib.LineWidth;
            }

            this.BackColor = _attrib.LineColor;
            this.Location = _attrib.start;
            this.IsMoved = !_attrib.Lock;
        }

        /// <summary>
        /// 从xml文件中加载线条的数据
        /// </summary>
        /// <param name="xml">xml对象</param>
        /// <param name="isCreate">true-创建新ID false-使用文件中的ID</param>
        public override void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement lineXml = xml.CurrentElement;

            if (isCreate)
                createID();
            else
                _attrib.ID = UInt16.Parse(lineXml.GetAttribute("id"));

            int x = int.Parse(lineXml.GetAttribute("x"));
            int y = int.Parse(lineXml.GetAttribute("y"));
            _attrib.start = new Point(x, y);
            _attrib.LineLength = UInt16.Parse(lineXml.GetAttribute("length"));
            _attrib.LineWidth = Byte.Parse(lineXml.GetAttribute("width"));
            _attrib.ShowType = Boolean.Parse(lineXml.GetAttribute("type"));
            _attrib.LineColor = Color.FromArgb(int.Parse(lineXml.GetAttribute("color")));
        }

        /// <summary>
        /// 保存线条控件数据到xml文件中
        /// </summary>
        /// <param name="xml">xml对象</param>
        public override void saveXML(SVXml xml)
        {
            XmlElement lineXml = xml.createNode(this.GetType().Name);
            lineXml.SetAttribute("id", _attrib.ID.ToString());
            lineXml.SetAttribute("x", _attrib.start.X.ToString());
            lineXml.SetAttribute("y", _attrib.start.Y.ToString());
            lineXml.SetAttribute("width", _attrib.LineWidth.ToString());
            lineXml.SetAttribute("length", _attrib.LineLength.ToString());
            lineXml.SetAttribute("color", _attrib.LineColor.ToArgb().ToString());
            lineXml.SetAttribute("type", _attrib.ShowType.ToString());
        }

        /// <summary>
        /// 生成下装文件
        /// </summary>
        /// <param name="pageArrayBin">二进制的配置数据</param>
        /// <param name="serialize">二进制的图片数据</param>
        public void buildControlToBin(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            _attrib.make(ref pageArrayBin, ref serialize);
        }

        /// <summary>
        /// 线条合法性检查
        /// </summary>
        public override void checkValid()
        {
            SVPageWidget pageWidget = this.Parent as SVPageWidget;
            String pageName = pageWidget.PageName;

            SVUniqueID uniqueObj = SVUniqueID.instance();
            if (Attrib.ID <= 0 || Attrib.ID >= uniqueObj.MaxID)
            {
                String msg = String.Format("页面 {0} 中,线条ID为:{1}, ID值已经超出最大范围[{2} - {3}]", pageName, Attrib.ID, 0, uniqueObj.MaxID);
                throw new SVCheckValidException(msg);
            }

            if (!this.Parent.ClientRectangle.Contains(this.Bounds))
            {
                String msg = String.Format("页面 {0} 中,线条控件ID为:{1}, 已经超出页面显示范围", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }
        }
    }
}
