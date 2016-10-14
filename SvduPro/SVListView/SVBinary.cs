using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVBinary : SVPanel, SVInterfacePanel, ISerializable, SVInterfaceBuild
    {
        SVBinaryProperties _attrib = new SVBinaryProperties();

        public SVBinaryProperties Attrib
        {
            set{ _attrib = value;}
            get { return _attrib; }
        }

        public SVBinary()
        {
            this.SizeChanged += new EventHandler((sender, e) =>
            {
                if (this.Width < 21)
                    this.Width = 21;

                if (this.Height < 21)
                    this.Height = 21;
            });
        }

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
        /// 创建ID
        /// </summary>
        public override void createID()
        {
            _attrib.ID = (UInt16)SVUniqueID.instance().newUniqueID();
        }

        /// <summary>
        /// 回收ID号
        /// </summary>
        public override void delID()
        {
            SVUniqueID.instance().delUniqueID((Int16)_attrib.ID);
        }

        public override void setStartPos(Point pos)
        {
            _attrib.Rect = new Rectangle(pos.X, pos.Y, this.Width, this.Height);
        }

        public override object property()
        {
            return _attrib;
        }

        protected SVBinary(SerializationInfo info, StreamingContext context)
        {
            _attrib = (SVBinaryProperties)info.GetValue("stream", typeof(SVBinaryProperties));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("stream", _attrib);
        }

        //建立一个副本
        override public object cloneObject()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, this);
            stream.Position = 0;
            SVBinary result = (SVBinary)binFormat.Deserialize(stream);

            result.refreshPropertyToPanel();

            return result;
        }

        public override void refreshPropertyToPanel()
        {
            this.Id = _attrib.ID;
            this.Width = _attrib.Rect.Width;
            this.Height = _attrib.Rect.Height;
            this.Location = new Point(_attrib.Rect.X, _attrib.Rect.Y);
            this.Font = _attrib.Font;
            this.ForeColor = _attrib.TrueColor;
            this.BackColor = _attrib.TrueBgColor;
            this.IsMoved = !_attrib.Lock;

            Dictionary<Byte, String> config = new Dictionary<Byte, String>()
            {
                {0, "打开|关闭"},
                {1, "运行|停止"},
                {2, "1|0"},
                {3, "是|否"},
                {4, "真|假"},
                {5, "正确|错误"},
                {6, "开|关"},
                {7, "自定义"}
            };

            if (_attrib.Type == 7)
                this.Text = String.Format("{0}|{1}", Attrib.CustomTrueText, Attrib.CustomFlaseText);
            else
                this.Text = config[_attrib.Type];
        }

        override public void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement binary = xml.CurrentElement;

            if (isCreate)
                createID();
            else
                _attrib.ID = UInt16.Parse(binary.GetAttribute("ID"));

            ///读取尺寸
            int x = int.Parse(binary.GetAttribute("X"));
            int y = int.Parse(binary.GetAttribute("Y"));
            int width = int.Parse(binary.GetAttribute("Width"));
            int height = int.Parse(binary.GetAttribute("Height"));
            _attrib.Rect = new Rectangle(x, y, width, height);

            ///读取变量
            _attrib.Var = binary.GetAttribute("Variable");
            ///读取自定义名称
            _attrib.CustomTrueText = binary.GetAttribute("CustomTrueText");
            _attrib.CustomFlaseText = binary.GetAttribute("CustomFalseText");
            
            _attrib.TrueColor = Color.FromArgb(int.Parse(binary.GetAttribute("TrueColor")));
            _attrib.TrueBgColor = Color.FromArgb(int.Parse(binary.GetAttribute("TrueBgColor")));
            _attrib.FalseColor = Color.FromArgb(int.Parse(binary.GetAttribute("FalseColor")));
            _attrib.FalseBgColor = Color.FromArgb(int.Parse(binary.GetAttribute("FalseBgColor")));
            _attrib.ExceptionColor = Color.FromArgb(int.Parse(binary.GetAttribute("ExceptionColor")));
            _attrib.ExceptionBgColor = Color.FromArgb(int.Parse(binary.GetAttribute("ExceptionBgColor")));
            String fontFamily = binary.GetAttribute("Font");
            Single fontSize = Single.Parse(binary.GetAttribute("FontSize"));
            _attrib.Font = new Font(fontFamily, fontSize);
            _attrib.Type = Byte.Parse(binary.GetAttribute("Type"));
        }

        override public void saveXML(SVXml xml)
        {
            XmlElement binary = xml.createNode(this.GetType().Name);

            ///写入尺寸
            binary.SetAttribute("ID", _attrib.ID.ToString());
            binary.SetAttribute("X", _attrib.Rect.X.ToString());
            binary.SetAttribute("Y", _attrib.Rect.Y.ToString());
            binary.SetAttribute("Width", _attrib.Rect.Width.ToString());
            binary.SetAttribute("Height", _attrib.Rect.Height.ToString());

            ///写入变量
            binary.SetAttribute("Variable", _attrib.Var);

            ///写入自定义名称
            binary.SetAttribute("CustomTrueText", _attrib.CustomTrueText);
            binary.SetAttribute("CustomFalseText", _attrib.CustomFlaseText);

            binary.SetAttribute("TrueColor", _attrib.TrueColor.ToArgb().ToString());
            binary.SetAttribute("TrueBgColor", _attrib.TrueBgColor.ToArgb().ToString());
            binary.SetAttribute("FalseColor", _attrib.FalseColor.ToArgb().ToString());
            binary.SetAttribute("FalseBgColor", _attrib.FalseBgColor.ToArgb().ToString());
            binary.SetAttribute("Font", _attrib.Font.Name.ToString());
            binary.SetAttribute("FontSize", _attrib.Font.Size.ToString());
            binary.SetAttribute("Type", _attrib.Type.ToString());
            binary.SetAttribute("ExceptionColor", _attrib.ExceptionColor.ToArgb().ToString());
            binary.SetAttribute("ExceptionBgColor", _attrib.ExceptionBgColor.ToArgb().ToString());
        }

        public void buildControlToBin(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            _attrib.make(ref pageArrayBin, ref serialize);
        }

        public override void checkValid()
        {
            SVPageWidget pageWidget = this.Parent as SVPageWidget;
            String pageName = pageWidget.PageName;

            SVUniqueID uniqueObj = SVUniqueID.instance();

            if (Attrib.ID <= 0 || Attrib.ID >= uniqueObj.MaxID)
            {
                String msg = String.Format("页面 {0} 中,开关ID为:{1}, ID值已经超出最大范围[{2} - {3}]", pageName, Attrib.ID, 0, uniqueObj.MaxID);
                throw new SVCheckValidException(msg);
            }

            if (!this.Parent.ClientRectangle.Contains(this.Bounds))
            {
                String msg = String.Format("页面 {0} 中,开关量ID为:{1}, 已经超出页面显示范围", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            return;

            var varInstance = SVVaribleType.instance();
            if (!varInstance.isOpen())
            {
                String msg = String.Format("数据库打开失败，请检查！");
                throw new SVCheckValidException(msg);
            }

            var address = varInstance.strToAddress(Attrib.Var);
            if (address == 0)
            {
                String msg = String.Format("页面 {0} 中, 开关量ID为:{1}, 未正确设置变量", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            var type = varInstance.strToType(Attrib.Var);
            if (type == -1)
            {
                String msg = String.Format("页面 {0} 中, 开关量ID为:{1}, 变量类型不满足条件", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }
        }
    }
}
