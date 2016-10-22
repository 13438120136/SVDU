using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVLabel : SVPanel, SVInterfacePanel, ISerializable, SVInterfaceBuild
    {
        SVLabelProperties _attrib = new SVLabelProperties();

        /// <summary>
        /// 文本属性
        /// </summary>
        public SVLabelProperties Attrib
        {
            set { _attrib = value;}
            get { return _attrib; }
        }

        /// <summary>
        /// 文本构造函数
        /// </summary>
        public SVLabel()
        {
            this.SizeChanged += new EventHandler((sender, e) =>
            {
                if (this.Width < 21)
                    this.Width = 21;

                if (this.Height < 21)
                    this.Height = 21;
            });
        }

        /// <summary>
        /// 重写initalizeRedoUndo
        /// </summary>
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

        /// <summary>
        /// 创建一个新的ID号
        /// </summary>
        public override void createID()
        {
            _attrib.ID = (UInt16)SVUniqueID.instance().newUniqueID();
        }

        /// <summary>
        /// 回收当前按钮对象的ID号
        /// </summary>
        public override void delID()
        {
            SVUniqueID.instance().delUniqueID((Int16)_attrib.ID);
        }

        /// <summary>
        /// 设置当前对象的起始位置
        /// </summary>
        /// <param Name="pos">点坐标，控件的起始位置</param>
        public override void setStartPos(Point pos)
        {
            _attrib.Rect = new Rectangle(pos.X, pos.Y, this.Width, this.Height);
        }

        /// <summary>
        /// 重写父类方法
        /// 返回文本对象中的属性
        /// </summary>
        /// <returns>属性对象</returns>
        public override object property()
        {
            return _attrib;
        }

        protected SVLabel(SerializationInfo info, StreamingContext context)
        {
            _attrib = (SVLabelProperties)info.GetValue("stream", typeof(SVLabelProperties));
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
            SVLabel result = (SVLabel)binFormat.Deserialize(stream);

            result.refreshPropertyToPanel();

            return result;
        }

        /// <summary>
        /// 更新文本控件在工作区中的显示
        /// </summary>
        public override void refreshPropertyToPanel()
        {
            this.Id = _attrib.ID;
            this.Width = _attrib.Rect.Width;
            this.Height = _attrib.Rect.Height;
            this.Location = new Point(_attrib.Rect.X, _attrib.Rect.Y);
            this.Text = _attrib.Text;            
            this.Font = _attrib.Font;

            this.BTransparent = _attrib.Transparent;
            this.IsMoved = !_attrib.Lock;

            if (BTransparent) ///透明背景显示
            {
                this.BackColor = Color.Transparent;
                this.ForeColor = _attrib.FrontColorground;
            }
            else  ///非透明背景
            {
                this.BackColor = _attrib.BackColorground;
                this.ForeColor = _attrib.FrontColorground;
            }

            Dictionary<String, ContentAlignment> alignDict = new Dictionary<String, ContentAlignment>();
            alignDict.Add("左对齐", ContentAlignment.TopLeft);
            alignDict.Add("右对齐", ContentAlignment.TopRight);
            alignDict.Add("居中对齐", ContentAlignment.TopCenter);
            alignDict.Add("水平和垂直居中", ContentAlignment.MiddleCenter);
            this.TextAlign = alignDict[_attrib.Align];
        }

        private void drawLabel(PaintEventArgs e)
        {
            Graphics gh = e.Graphics;
            SolidBrush brush = new SolidBrush(Attrib.BackColorground);
            gh.FillRectangle(brush, this.ClientRectangle);

            int startX = 0;
            int startY = 0;
            int endX = this.Width + startX - 1;
            int endY = this.Height + startY - 1;

            gh.DrawLine(new Pen(Color.FromArgb(0x80, 0x80, 0x80), 1), new Point(startX, startY), new Point(endX, startY));
            gh.DrawLine(new Pen(Color.FromArgb(0x80, 0x80, 0x80), 1), new Point(startX, startY + 1), new Point(startX, startY));

            gh.DrawLine(new Pen(Color.FromArgb(0x00, 0x00, 0x00), 1), new Point(startX + 1, startY + 1), new Point(endX - 1, startY + 1));
            gh.DrawLine(new Pen(Color.FromArgb(0x00, 0x00, 0x00), 1), new Point(startX + 1, startY + 2), new Point(startX + 1, endY - 1));

            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(startX + 1, endY), new Point(endX, endY));
            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(endX, startY + 1), new Point(endX, endY));

            gh.DrawLine(new Pen(Color.FromArgb(0xc0, 0xc0, 0xc0), 1), new Point(startX + 2, endY - 1), new Point(endX - 1, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0xc0, 0xc0, 0xc0), 1), new Point(endX - 1, startY + 2), new Point(endX - 1, endY - 1));
        }

        /// <summary>
        /// 自定义绘制Label
        /// </summary>
        /// <param Name="e"></param>
        protected override void panelOnPaint(PaintEventArgs e)
        {
            drawLabel(e);
        }

        override public void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement label = xml.CurrentElement;

            if (isCreate)
                createID();
            else
                _attrib.ID = UInt16.Parse(label.GetAttribute("ID"));

            int x = int.Parse(label.GetAttribute("X"));
            int y = int.Parse(label.GetAttribute("Y"));
            int width = int.Parse(label.GetAttribute("Width"));
            int height = int.Parse(label.GetAttribute("Height"));
            _attrib.Rect = new Rectangle(x, y, width, height);
            _attrib.FrontColorground = Color.FromArgb(int.Parse(label.GetAttribute("Color")));
            _attrib.BackColorground = Color.FromArgb(int.Parse(label.GetAttribute("Backcolor")));
            _attrib.Text = label.GetAttribute("Text");
            _attrib.Align = label.GetAttribute("TextAlign");
            _attrib.Transparent = Boolean.Parse(label.GetAttribute("Transparent"));
            String fontFamily = label.GetAttribute("Font");
            Single fontSize = float.Parse(label.GetAttribute("FontSize"));
            _attrib.Font = new Font(fontFamily, fontSize);
        }

        override public void saveXML(SVXml xml)
        {
            XmlElement label = xml.createNode(this.GetType().Name);

            label.SetAttribute("ID", _attrib.ID.ToString());
            label.SetAttribute("X", _attrib.Rect.X.ToString());
            label.SetAttribute("Y", _attrib.Rect.Y.ToString());
            label.SetAttribute("Width", _attrib.Rect.Width.ToString());
            label.SetAttribute("Height", _attrib.Rect.Height.ToString());
            label.SetAttribute("Color", _attrib.FrontColorground.ToArgb().ToString());
            label.SetAttribute("Backcolor", _attrib.BackColorground.ToArgb().ToString());
            label.SetAttribute("TextAlign", _attrib.Align);
            label.SetAttribute("Text", _attrib.Text);
            label.SetAttribute("Font", _attrib.Font.Name.ToString());
            label.SetAttribute("FontSize", _attrib.Font.Size.ToString());
            label.SetAttribute("Transparent", _attrib.Transparent.ToString());
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
                String msg = String.Format("页面 {0} 中,文本ID为:{1}, ID值已经超出最大范围[{2} - {3}]", pageName, Attrib.ID, 0, uniqueObj.MaxID);
                throw new SVCheckValidException(msg);
            }

            if (!isHasParent())
            {
                String msg = String.Format("页面 {0} 中,文本ID为:{1}, 没有在页面控件中", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (this.Parent == null)
            {
                String msg = String.Format("页面 {0} 中,文本ID为:{1}, 没有父控件", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (String.IsNullOrEmpty(Attrib.Text))
            {
                String msg = String.Format("页面 {0} 中,文本ID为:{1}, 文本内容不能为空", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (!this.Parent.ClientRectangle.Contains(this.Bounds))
            {
                String msg = String.Format("页面 {0} 中,文本ID为:{1}, 已经超出页面显示范围", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }
        }
    }
}
