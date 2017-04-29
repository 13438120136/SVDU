using System;
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
            ///修正趋势图的宽度和高度
            this.SizeChangedEvent += new Action(SVCurve_SizeChanged);
        }

        /// <summary>
        /// 当趋势图的宽和高发生变化的时候，调整使其满足需要
        /// </summary>
        void SVCurve_SizeChanged()
        {
            correctionWindow();
        }

        /// <summary>
        /// 修正窗口宽度和高度
        /// </summary>
        private void correctionWindow()
        {
            const Int32 minWidth = 200;
            const Int32 maxWidth = 730;
            const Int32 minHeight = 200;
            const Int32 maxHeight = 540;

            Int32 tmpWidth = this.Width - 70;

            if (tmpWidth > maxWidth)
                tmpWidth = maxWidth;

            if (tmpWidth < minWidth)
                tmpWidth = minWidth;

            Int32 beishu = tmpWidth % 10;
            if (beishu != 0)
            {
                tmpWidth -= beishu;
            }

            if (tmpWidth > _attrib.Interval * 10)
                tmpWidth = _attrib.Interval * 10;

            this.Width = tmpWidth + 70;


            Int32 tmpHeight = this.Height - 60;

            if (tmpHeight > maxHeight)
                tmpHeight = maxHeight;

            if (tmpHeight < minHeight)
                tmpHeight = minHeight;

            Int32 heightB = tmpHeight % 10;
            if (heightB != 0)
            {
                tmpHeight -= heightB;
            }
            this.Height = tmpHeight + 60;
        }

        override public void initalizeRedoUndo()
        {
            this.RedoUndo.UpdateOperator += () =>
            {
                refreshPropertyToPanel();
            };

            ///如果是属性发生了变化
            _attrib.UpdateControl += new UpdateControl((item) =>
            {
                RedoUndo.recordOper(item);
            });
        }

        /// <summary>
        /// 创建ID
        /// </summary>
        public override void newID()
        {
            _attrib.ID = base.createID();
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

        /// <summary>
        /// 将控件属性更新到界面上
        /// </summary>
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
            this.Refresh();
        }

        /// <summary>
        /// 自绘制趋势图
        /// </summary>
        /// <param Name="e">绘制图形对象</param>
        protected void drawCurve(PaintEventArgs e)
        {
            ///横纵标尺间隔
            Int32 scaleStepX = 100;
            ///纵轴标尺间隔
            Int32 scaleStepY = 50;

            ///绘制趋势图的背景
            Graphics gh = e.Graphics;
            SolidBrush brush = new SolidBrush(Attrib.BackgdColor);
            gh.FillRectangle(brush, this.ClientRectangle);

            ///趋势图内部图形
            Rectangle rect = new Rectangle(60, 30, this.Width - 10 - 60, this.Height - 30 - 30);
            gh.DrawRectangle(new Pen(Attrib.FrontColor), rect);

            ///解决除数如果为0，出现异常情况
            if (rect.Width <= 0)
                rect.Width = 1;       

            ///绘制颜色
            SolidBrush fontBrush = new SolidBrush(Attrib.FrontColor);

            if (rect.Width < 10)
                rect.Width = 10;

            scaleStepX = rect.Width / 10;
            for (int i = 0; i <= rect.Width; i += scaleStepX)
            {
                ///横轴刻度
                int xPos = rect.X + i;
                int yPos = rect.Y + rect.Height;
                gh.DrawLine(new Pen(Attrib.FrontColor), new Point(xPos, yPos), new Point(xPos, yPos + 6));

                if (i == 0 || i == rect.Width)
                {
                    ///刻度文本
                    String text = (i * Attrib.Interval / rect.Width).ToString();
                    Int32 start = (Int32)gh.MeasureString(text, this.Font).Width / 2;
                    gh.DrawString(text, Attrib.Font, fontBrush, new Point(xPos - start, yPos + 6));
                }
            }

            ///纵轴刻度
            scaleStepY = rect.Height / 10;
            for (int i = 0; i <= rect.Height; i += scaleStepY)
            {
                int xPos = rect.X;
                int yPos = rect.Y + rect.Height - i;
                gh.DrawLine(new Pen(Attrib.FrontColor), new Point(xPos - 6, yPos), new Point(xPos, yPos));
            }

            ///绘制纵轴刻度文本
            gh.DrawString(Attrib.Max.ToString(), Attrib.Font, fontBrush, new Point(rect.X - 59, rect.Y - 10));
            gh.DrawString(Attrib.Min.ToString(), Attrib.Font, fontBrush, new Point(rect.X - 59, rect.Y + rect.Height - 8));

            ///绘制纵轴背景刻度
            for (int x = scaleStepX; x < rect.Width; x += scaleStepX)
            {
                int xPos = rect.X + x;
                gh.DrawLine(new Pen(Color.DarkGray), new Point(xPos, rect.Y + 1), new Point(xPos, rect.Y + rect.Height - 1));
            }

            ///绘制横轴背景刻度
            for (int x = scaleStepY; x < rect.Height; x += scaleStepY)
            {
                int yPos = rect.Y + rect.Height - x;
                gh.DrawLine(new Pen(Color.DarkGray), new Point(rect.X + 1, yPos), new Point(rect.X + rect.Width - 1, yPos));
            }
        }

        /// <summary>
        /// 覆盖窗体的原有绘制
        /// </summary>
        /// <param Name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            drawCurve(e);
        }

        /// <summary>
        /// 从文件中加载数据
        /// </summary>
        /// <param Name="xml">xml文件对象</param>
        /// <param Name="isCreate"></param>
        override public void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement curve = xml.CurrentElement;

            if (isCreate)
                newID();
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

            Int32 count = Int32.Parse(curve.GetAttribute("Count"));
            for (int i = 0; i < count; i++)
            {
                SVCurveProper proper = new SVCurveProper();

                XmlNodeList nls = curve.GetElementsByTagName("VarName");
                XmlElement vElement = (XmlElement)nls[i];
                proper.Var.VarName = vElement.GetAttribute("Value");

                XmlNodeList varType = curve.GetElementsByTagName("VarType");
                XmlElement tElement = (XmlElement)varType[i];
                proper.Var.VarBlockType = Byte.Parse(tElement.GetAttribute("Value"));

                XmlNodeList colorList = curve.GetElementsByTagName("VarColor");
                XmlElement cElement = (XmlElement)colorList[i];
                String value = cElement.GetAttribute("Value");
                proper.Color = Color.FromArgb(int.Parse(value));

                XmlNodeList enabledList = curve.GetElementsByTagName("varEnabled");
                XmlElement eElement = (XmlElement)enabledList[i];
                proper.Enabled = Boolean.Parse(eElement.GetAttribute("Value"));

                _attrib.Variable.Add(proper);
            }
        }

        /// <summary>
        /// 保存数据到xml文件中
        /// </summary>
        /// <param Name="xml">xml文件对象</param>
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

            curve.SetAttribute("Count", _attrib.Variable.Count.ToString());

            ///保存变量的字段
            foreach (var varTmp in _attrib.Variable)
            {
                XmlElement nameList = xml.crateChildNode("VarName");
                curve.AppendChild(nameList);
                nameList.SetAttribute("Value", varTmp.Var.VarName);

                XmlElement typeList = xml.crateChildNode("VarType");
                curve.AppendChild(typeList);
                typeList.SetAttribute("Value", varTmp.Var.VarBlockType.ToString());

                XmlElement colorList = xml.crateChildNode("VarColor");
                curve.AppendChild(colorList);
                colorList.SetAttribute("Value", varTmp.Color.ToArgb().ToString());

                XmlElement enableList = xml.crateChildNode("varEnabled");
                curve.AppendChild(enableList);
                enableList.SetAttribute("Value", varTmp.Enabled.ToString());
            }
        }

        /// <summary>
        /// 执行当前对象的编译过程
        /// </summary>
        /// <param Name="pageArrayBin"></param>
        /// <param Name="serialize"></param>
        public void buildControlToBin(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            _attrib.make(ref pageArrayBin, ref serialize);
        }

        /// <summary>
        /// 编译的时候进行的合法性检查
        /// </summary>
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

            if (this._attrib.Variable.Count == 0)
            {
                String msg = String.Format("页面 {0} 中,趋势图ID为:{1}, 未关联变量", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }
        }
    }
}
