using System;
using System.Drawing;
using System.Xml;
using SVCore;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace SVControl
{
    [Serializable]
    public class SVCurve : SVPanel, SVInterfacePanel, ISerializable, SVInterfaceBuild
    {
        SVCurveProperties _attrib = new SVCurveProperties();

        public SVCurveProperties Attrib
        {
            set
            {
                _attrib = value;
            }
            get { return _attrib; }
        }

        public SVCurve()
        {
        }

        override public void initalizeRedoUndo()
        {
            this.RedoUndo.UpdateOperator += () =>
            {
                refreshPropertyToPanel();
            };

            _attrib.UpdateControl += new UpdateControl((item) =>
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

        protected SVCurve(SerializationInfo info, StreamingContext context)
        {
            _attrib = (SVCurveProperties)info.GetValue("stream", typeof(SVCurveProperties));
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
            SVCurve result = (SVCurve)binFormat.Deserialize(stream);

            result.refreshPropertyToPanel();

            return result;
        }

        public override void refreshPropertyToPanel()
        {
            this.Id = _attrib.ID;
            this.Width = _attrib.Rect.Width;
            this.Height = _attrib.Rect.Height;
            this.Location = new Point(_attrib.Rect.X, _attrib.Rect.Y);
            this.BackColor = _attrib.BackgdColor;
            this.ForeColor = _attrib.FrontColor;
            this.Font = _attrib.Font;
            this.IsMoved = !_attrib.Lock;
        }

        protected void drawCurve(PaintEventArgs e)
        {
            Graphics gh = e.Graphics;
            SolidBrush brush = new SolidBrush(Attrib.BackgdColor);
            gh.FillRectangle(brush, this.ClientRectangle);

            Rectangle rect = new Rectangle(50, 30, this.Width - 50 - 20, this.Height - 30 - 30);
            gh.DrawRectangle(new Pen(Attrib.FrontColor), rect);

            for (int i = 0; i <= rect.Width; i += 120)
            {
                int xPos = rect.X + i;
                int yPos = rect.Y + rect.Height;
                gh.DrawLine(new Pen(Attrib.FrontColor), new Point(xPos, yPos), new Point(xPos, yPos + 6));
                SolidBrush fontBrush = new SolidBrush(Attrib.FrontColor);
                String text = (i * Attrib.Interval / rect.Width).ToString();
                gh.DrawString(text, Attrib.Font, fontBrush, new Point(xPos, yPos + 6));
            }

            for (int i = 0; i <= rect.Height; i += 60)
            {
                int xPos = rect.X;
                int yPos = rect.Y + rect.Height - i;
                gh.DrawLine(new Pen(Attrib.FrontColor), new Point(xPos - 6, yPos), new Point(xPos, yPos));
                SolidBrush fontBrush = new SolidBrush(Attrib.FrontColor);
                String text = ((UInt32)(i * (Attrib.Max - Attrib.Min) / rect.Height)).ToString();
                gh.DrawString(text, Attrib.Font, fontBrush, new Point(xPos - 50, yPos));
            }

            for (int i = 120; i < rect.Width; i += 120)
            {
                int xPos = rect.X + i;
                int yPos = rect.Y + rect.Height;
                gh.DrawLine(new Pen(Color.DarkGray), new Point(xPos, rect.Y), new Point(xPos, yPos));
            }

            for (int i = 60; i < rect.Height; i += 60)
            {
                int xPos = rect.X;
                int yPos = rect.Y + rect.Height - i;
                gh.DrawLine(new Pen(Color.DarkGray), new Point(xPos, yPos), new Point(xPos + rect.Width, yPos));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            drawCurve(e);
        }

        override public void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement curve = xml.CurrentElement;

            if (isCreate)
                createID();
            else
                _attrib.ID = UInt16.Parse(curve.GetAttribute("ID"));

            int x = int.Parse(curve.GetAttribute("X"));
            int y = int.Parse(curve.GetAttribute("Y"));
            int width = int.Parse(curve.GetAttribute("Width"));
            int height = int.Parse(curve.GetAttribute("Height"));
            _attrib.Rect = new Rectangle(x, y, width, height);
            _attrib.FrontColor = Color.FromArgb(int.Parse(curve.GetAttribute("FrontColor")));
            _attrib.BackgdColor = Color.FromArgb(int.Parse(curve.GetAttribute("BackgdColor")));
            _attrib.Interval = UInt16.Parse(curve.GetAttribute("Interval"));
            String fontFamily = curve.GetAttribute("Font");
            Single fontSize = Single.Parse(curve.GetAttribute("FontSize"));
            _attrib.Font = new Font(fontFamily, fontSize);
            _attrib.Max = Single.Parse(curve.GetAttribute("Max"));
            _attrib.Min = Single.Parse(curve.GetAttribute("Min"));
        }

        override public void saveXML(SVXml xml)
        {
            XmlElement curve = xml.createNode(this.GetType().Name);

            curve.SetAttribute("ID", _attrib.ID.ToString());
            curve.SetAttribute("X", _attrib.Rect.X.ToString());
            curve.SetAttribute("Y", _attrib.Rect.Y.ToString());
            curve.SetAttribute("Width", _attrib.Rect.Width.ToString());
            curve.SetAttribute("Height", _attrib.Rect.Height.ToString());
            curve.SetAttribute("FrontColor", _attrib.FrontColor.ToArgb().ToString());
            curve.SetAttribute("BackgdColor", _attrib.BackgdColor.ToArgb().ToString());
            curve.SetAttribute("Interval", _attrib.Interval.ToString());
            curve.SetAttribute("Font", _attrib.Font.Name.ToString());
            curve.SetAttribute("FontSize", _attrib.Font.Size.ToString());
            curve.SetAttribute("Max", _attrib.Max.ToString());
            curve.SetAttribute("Min", _attrib.Min.ToString());
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
                String msg = String.Format("页面 {0} 中,趋势图ID为:{1}, ID值已经超出最大范围[{2} - {3}]", pageName, Attrib.ID, 0, uniqueObj.MaxID);
                throw new SVCheckValidException(msg);
            }

            if (Attrib.Min >= Attrib.Max)
            {
                String msg = String.Format("页面 {0} 中,趋势图ID为:{1}, 起始坐标大于结束坐标", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (!this.Parent.ClientRectangle.Contains(this.Bounds))
            {
                String msg = String.Format("页面 {0} 中,趋势图ID为:{1}, 已经超出页面显示范围", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }
        }
    }
}
