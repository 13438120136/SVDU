using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVAnalog : SVPanel, SVInterfacePanel, ISerializable, SVInterfaceBuild
    {
        SVAnalogProperties _attrib = new SVAnalogProperties();

        public SVAnalogProperties Attrib
        {
            set { _attrib = value;}
            get { return _attrib; }
        }

        public SVAnalog()
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

        protected SVAnalog(SerializationInfo info, StreamingContext context)
        {
            _attrib = (SVAnalogProperties)info.GetValue("stream", typeof(SVAnalogProperties));
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
            SVAnalog result = (SVAnalog)binFormat.Deserialize(stream);

            result.refreshPropertyToPanel();

            return result;
        }

        /// <summary>
        /// 重载父类,用来更新控件外观显示
        /// </summary>
        public override void refreshPropertyToPanel()
        {
            this.Id = _attrib.ID;
            this.Width = _attrib.Rect.Width;
            this.Height = _attrib.Rect.Height;
            this.Location = new Point(_attrib.Rect.X, _attrib.Rect.Y);
            this.ForeColor = _attrib.NormalColor;
            this.BackColor = _attrib.NormalBgColor;
            this.IsMoved = !_attrib.Lock;
            this.Font = _attrib.Font;            
            double num = 0.0;
            this.Text = num.ToString(String.Format("f{0}", _attrib.DecNum));
        }

        override public void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement analog = xml.CurrentElement;

            if (isCreate)
                createID();
            else
                _attrib.ID = UInt16.Parse(analog.GetAttribute("ID"));

            int x = int.Parse(analog.GetAttribute("X"));
            int y = int.Parse(analog.GetAttribute("Y"));
            int width = int.Parse(analog.GetAttribute("Width"));
            int height = int.Parse(analog.GetAttribute("Height"));
            _attrib.Var = analog.GetAttribute("Variable");

            _attrib.Rect = new Rectangle(x, y, width, height);
            _attrib.NormalColor = Color.FromArgb(int.Parse(analog.GetAttribute("NormalColor")));
            _attrib.OverMaxColor = Color.FromArgb(int.Parse(analog.GetAttribute("OverMaxColor")));
            _attrib.OverMinColor = Color.FromArgb(int.Parse(analog.GetAttribute("OverMinColor")));
            _attrib.NormalBgColor = Color.FromArgb(int.Parse(analog.GetAttribute("NormalBackcolor")));
            _attrib.OverMaxBgColor = Color.FromArgb(int.Parse(analog.GetAttribute("OverMaxBgColor")));
            _attrib.OverMinBgColor = Color.FromArgb(int.Parse(analog.GetAttribute("OverMinBgColor")));
            _attrib.ExceptionColor = Color.FromArgb(int.Parse(analog.GetAttribute("ExceptionColor")));
            _attrib.ExceptionBgColor = Color.FromArgb(int.Parse(analog.GetAttribute("ExceptionBgColor")));

            _attrib.DecNum = Byte.Parse(analog.GetAttribute("DecNumber"));
            _attrib.IsExponent = Boolean.Parse(analog.GetAttribute("IsExponent"));
            String fontFamily = analog.GetAttribute("Font");
            Single fontSize = Single.Parse(analog.GetAttribute("FontSize"));
            _attrib.Font = new Font(fontFamily, fontSize);
            _attrib.Max = Single.Parse(analog.GetAttribute("Max"));
            _attrib.Min = Single.Parse(analog.GetAttribute("Min"));
        }

        override public void saveXML(SVXml xml)
        {
            XmlElement analog = xml.createNode(this.GetType().Name);

            analog.SetAttribute("ID", _attrib.ID.ToString());
            analog.SetAttribute("X", _attrib.Rect.X.ToString());
            analog.SetAttribute("Y", _attrib.Rect.Y.ToString());
            analog.SetAttribute("Width", _attrib.Rect.Width.ToString());
            analog.SetAttribute("Height", _attrib.Rect.Height.ToString());
            analog.SetAttribute("Variable", _attrib.Var);

            analog.SetAttribute("NormalColor", _attrib.NormalColor.ToArgb().ToString());
            analog.SetAttribute("OverMaxColor", _attrib.OverMaxColor.ToArgb().ToString());
            analog.SetAttribute("OverMinColor", _attrib.OverMinColor.ToArgb().ToString());
            analog.SetAttribute("NormalBackcolor", _attrib.NormalBgColor.ToArgb().ToString());
            analog.SetAttribute("OverMaxBgColor", _attrib.OverMaxBgColor.ToArgb().ToString());
            analog.SetAttribute("OverMinBgColor", _attrib.OverMinBgColor.ToArgb().ToString());
            analog.SetAttribute("ExceptionColor", _attrib.ExceptionColor.ToArgb().ToString());
            analog.SetAttribute("ExceptionBgColor", _attrib.ExceptionBgColor.ToArgb().ToString());
            analog.SetAttribute("DecNumber", _attrib.DecNum.ToString());
            analog.SetAttribute("IsExponent", _attrib.IsExponent.ToString());
            analog.SetAttribute("Font", _attrib.Font.Name.ToString());
            analog.SetAttribute("FontSize", _attrib.Font.Size.ToString());
            analog.SetAttribute("Max", _attrib.Max.ToString());
            analog.SetAttribute("Min", _attrib.Min.ToString());
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
                String msg = String.Format("页面 {0} 中,模拟量ID为:{1}, ID值已经超出最大范围[{2} - {3}]", pageName, Attrib.ID, 0, uniqueObj.MaxID);
                throw new SVCheckValidException(msg);
            }

            if (Attrib.Min >= Attrib.Max)
            {
                String msg = String.Format("页面 {0} 中, 模拟量ID为:{1}, 最小值 >= 最大值", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (!this.Parent.ClientRectangle.Contains(this.Bounds))
            {
                String msg = String.Format("页面 {0} 中, 模拟量ID为:{1}, 已经超出页面显示范围", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            var varInstance = SVVaribleType.instance();
            if (!varInstance.isOpen())
            {
                String msg = String.Format("数据库打开失败，请检查！");
                throw new SVCheckValidException(msg);
            }

            var address = varInstance.strToAddress(Attrib.Var);
            if (address == 0)
            {
                String msg = String.Format("页面 {0} 中, 模拟量ID为:{1}, 未正确设置变量", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            var type = varInstance.strToType(Attrib.Var);
            if (type == -1)
            {
                String msg = String.Format("页面 {0} 中, 模拟量ID为:{1}, 变量类型不满足条件", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }
        }
    }
}
