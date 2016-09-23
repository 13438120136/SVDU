using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using SVCore;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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

        public override void createID()
        {
            _attrib.ID = (UInt16)SVUniqueID.instance().newUniqueID();
        }

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

            Dictionary<String, String> config = new Dictionary<String, String>()
            {
                {"打开 or 关闭", "打开"},
                {"运行 or 停止", "运行"},
                {"1 or 0", "1"},
                {"是 or 否", "是"},
                {"真 or 假", "真"},
                {"正确 or 错误", "正确"},
                {"开 or 关", "开"}
            };
            this.Text = config[_attrib.Type];
        }

        override public void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement binary = xml.CurrentElement;

            if (isCreate)
                createID();
            else
                _attrib.ID = UInt16.Parse(binary.GetAttribute("ID"));

            int x = int.Parse(binary.GetAttribute("X"));
            int y = int.Parse(binary.GetAttribute("Y"));
            int width = int.Parse(binary.GetAttribute("Width"));
            int height = int.Parse(binary.GetAttribute("Height"));
            _attrib.Rect = new Rectangle(x, y, width, height);
            _attrib.TrueColor = Color.FromArgb(int.Parse(binary.GetAttribute("TrueColor")));
            _attrib.TrueBgColor = Color.FromArgb(int.Parse(binary.GetAttribute("TrueBgColor")));
            _attrib.FalseColor = Color.FromArgb(int.Parse(binary.GetAttribute("FalseColor")));
            _attrib.FalseBgColor = Color.FromArgb(int.Parse(binary.GetAttribute("FalseBgColor")));
            _attrib.ExceptionColor = Color.FromArgb(int.Parse(binary.GetAttribute("ExceptionColor")));
            _attrib.ExceptionBgColor = Color.FromArgb(int.Parse(binary.GetAttribute("ExceptionBgColor")));
            String fontFamily = binary.GetAttribute("Font");
            Single fontSize = Single.Parse(binary.GetAttribute("FontSize"));
            _attrib.Font = new Font(fontFamily, fontSize);
            _attrib.Type = binary.GetAttribute("Type");
        }

        override public void saveXML(SVXml xml)
        {
            XmlElement binary = xml.createNode(this.GetType().Name);

            binary.SetAttribute("ID", _attrib.ID.ToString());
            binary.SetAttribute("X", _attrib.Rect.X.ToString());
            binary.SetAttribute("Y", _attrib.Rect.Y.ToString());
            binary.SetAttribute("Width", _attrib.Rect.Width.ToString());
            binary.SetAttribute("Height", _attrib.Rect.Height.ToString());
            binary.SetAttribute("TrueColor", _attrib.TrueColor.ToArgb().ToString());
            binary.SetAttribute("TrueBgColor", _attrib.TrueBgColor.ToArgb().ToString());
            binary.SetAttribute("FalseColor", _attrib.FalseColor.ToArgb().ToString());
            binary.SetAttribute("FalseBgColor", _attrib.FalseBgColor.ToArgb().ToString());
            binary.SetAttribute("Font", _attrib.Font.Name.ToString());
            binary.SetAttribute("FontSize", _attrib.Font.Size.ToString());
            binary.SetAttribute("Type", _attrib.Type);
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
        }
    }
}
