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
    public delegate void paintEvent(PaintEventArgs e);

    [Serializable]
    public class SVButton : SVPanel, SVInterfacePanel, ISerializable, SVInterfaceBuild
    {
        SVButtonProperties _attrib = new SVButtonProperties();
        protected paintEvent _paintEvent;

        /// <summary>
        /// 当前按钮的属性对象
        /// </summary>
        public SVButtonProperties Attrib
        {
            set { _attrib = value; }
            get { return _attrib; }
        }

        /// <summary>
        /// 按钮的构造函数
        /// </summary>
        public SVButton()
        {
            initButton();
        }

        /// <summary>
        /// 初始化按钮的外观
        /// </summary>
        void initButton()
        {
            this.TextAlign = ContentAlignment.MiddleCenter;
            _paintEvent = drawButtonNormal;

            this.SizeChanged += new EventHandler((sender, e)=>
            {
                if (this.Width < 21)
                    this.Width = 21;

                if (this.Height < 21)
                    this.Height = 21;
            });
        }

        /// <summary>
        /// 重写函数initalizeRedoUndo
        /// 当按钮属性被修改的时候，执行记录
        /// 撤销和重做操作执行的时候，主窗口中控件刷新
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
        /// 创建一个新的ID号
        /// </summary>
        public override void newID()
        {   
            _attrib.ID = base.createID();
        }

        /// <summary>
        /// 按钮对象执行序列化的过程中，需要重载的构造函数
        /// </summary>
        /// <param Name="info"></param>
        /// <param Name="context"></param>
        protected SVButton(SerializationInfo info, StreamingContext context)
        {
            initButton();
            _attrib = (SVButtonProperties)info.GetValue("stream", typeof(SVButtonProperties));
        }

        /// <summary>
        /// 重写GetObjectData函数，为按钮的序列化服务
        /// </summary>
        /// <param Name="info"></param>
        /// <param Name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("stream", _attrib);
        }

        /// <summary>
        /// 通过序列化的机制来克隆一个当前对象并返回
        /// </summary>
        /// <returns>一个新的按钮对象</returns>
        override public object cloneObject()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, this);
            stream.Position = 0;

            SVButton result = (SVButton)binFormat.Deserialize(stream);
            result.refreshPropertyToPanel();

            return result;
        }

        /// <summary>
        /// 设置当前按钮对象的起始位置
        /// </summary>
        /// <param Name="pos">点坐标，控件的起始位置</param>
        public override void setStartPos(Point pos)
        {
            _attrib.Rect = new Rectangle(pos.X, pos.Y, this.Width, this.Height);
        }

        /// <summary>
        /// 重写父类方法
        /// 返回当前按钮对象中的属性对象
        /// </summary>
        /// <returns>属性对象</returns>
        public override object property()
        {
            _attrib.reRefresh();
            return _attrib;
        }

        /// <summary>
        /// 根据当前按钮属性对象中的值，更新按钮外观显示
        /// </summary>
        public override void refreshPropertyToPanel()
        {
            this.Id = _attrib.ID;
            this.Width = _attrib.Rect.Width;
            this.Height = _attrib.Rect.Height;
            this.Location = new Point(_attrib.Rect.X, _attrib.Rect.Y);
            this.Text = _attrib.Text;
            this.Font = _attrib.Font;
            this.ForeColor = _attrib.FrontColorground;
            this.BackColor = _attrib.BackColorground;
            this.IsMoved = !_attrib.Lock;

            /// 设置背景图片
            this.BackgroundImage = null;
            if (_attrib.IsShowPic)
            {
                do
                {
                    ///首先设置弹起图片
                    if (Attrib.BtnUpPic.isValidShow())
                    {
                        String file = Path.Combine(SVProData.IconPath, _attrib.BtnUpPic.ImageFileName);
                        if (!File.Exists(file))
                            break;

                        SVPixmapFile pixmapFile = new SVPixmapFile();
                        pixmapFile.readPixmapFile(file);
                        this.BackgroundImage = pixmapFile.getBitmapFromData();
                        break;
                    }

                    ///如果弹起图片不存在，设置按下图片
                    if (Attrib.BtnDownPic.isValidShow())
                    {
                        String file = Path.Combine(SVProData.IconPath, _attrib.BtnDownPic.ImageFileName);
                        if (!File.Exists(file))
                            break;

                        SVPixmapFile pixmapFile = new SVPixmapFile();
                        pixmapFile.readPixmapFile(file);
                        this.BackgroundImage = pixmapFile.getBitmapFromData();
                        break;
                    }

                } while (false);
            }
        }

        /// <summary>
        /// 从xml文件中加载当前按钮对象内容
        /// </summary>
        /// <param Name="xml">xml对象</param>
        /// <param Name="isCreate">true-创建新的ID号，false-表示使用文件中的ID</param>
        public override void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement button = xml.CurrentElement;

            if (isCreate)
                newID();
            else
                _attrib.ID = UInt16.Parse(button.GetAttribute("id"));

            int x = int.Parse(button.GetAttribute("x"));
            int y = int.Parse(button.GetAttribute("y"));
            int width = int.Parse(button.GetAttribute("width"));
            int height = int.Parse(button.GetAttribute("height"));
            _attrib.Rect = new Rectangle(x, y, width, height);
            _attrib.FrontColorground = Color.FromArgb(int.Parse(button.GetAttribute("color")));
            _attrib.BackColorground = Color.FromArgb(int.Parse(button.GetAttribute("backcolor")));
            _attrib.BackColorgroundDown = Color.FromArgb(int.Parse(button.GetAttribute("backgdcolor")));
            _attrib.Text = button.GetAttribute("text");
            _attrib.FText = button.GetAttribute("falseText");

            String fontFamily = button.GetAttribute("font");
            Single fontSize = float.Parse(button.GetAttribute("fontSize"));
            _attrib.Font = new Font(fontFamily, fontSize);
            _attrib.Comfirm = Boolean.Parse(button.GetAttribute("Confirm"));

            _attrib.ButtonType = Byte.Parse(button.GetAttribute("ButtonType"));
            _attrib.ButtonPage.PageID = UInt16.Parse(button.GetAttribute("ButtonTypeID"));
            _attrib.ButtonPage.PageText = button.GetAttribute("ButtonTypeText");

            _attrib.BtnVarText.VarName = button.GetAttribute("ButtonTypeVar");
            _attrib.BtnVarText.VarType = Byte.Parse(button.GetAttribute("ButtonTypeVarType"));

            _attrib.EnVarText.VarName = button.GetAttribute("EnabledVar");
            _attrib.EnVarText.VarType = Byte.Parse(button.GetAttribute("EnabledVarType"));
            _attrib.BtnEnable = Boolean.Parse(button.GetAttribute("Enabled"));

            //按钮图片数据
            _attrib.IsShowPic = Boolean.Parse(button.GetAttribute("IsShowPicture"));
            _attrib.BtnDownPic.ImageFileName = button.GetAttribute("BtnDownPicFile");
            _attrib.BtnDownPic.ShowName = button.GetAttribute("BtnDownPicShowName");
            _attrib.BtnUpPic.ImageFileName = button.GetAttribute("BtnUpPicFile");
            _attrib.BtnUpPic.ShowName = button.GetAttribute("BtnUpPicShowName");
            //读取趋势图关联
            _attrib.CurveObj = button.GetAttribute("CurveObj");
        }

        /// <summary>
        /// 保存当前按钮内容到xml文件中
        /// </summary>
        /// <param Name="xml">xml对象</param>
        override public void saveXML(SVXml xml)
        {
            XmlElement button = xml.createNode(this.GetType().Name);
            button.SetAttribute("id", _attrib.ID.ToString());
            button.SetAttribute("x", _attrib.Rect.X.ToString());
            button.SetAttribute("y", _attrib.Rect.Y.ToString());
            button.SetAttribute("width", _attrib.Rect.Width.ToString());
            button.SetAttribute("height", _attrib.Rect.Height.ToString());
            button.SetAttribute("color", _attrib.FrontColorground.ToArgb().ToString());
            button.SetAttribute("backcolor", _attrib.BackColorground.ToArgb().ToString());
            button.SetAttribute("backgdcolor", _attrib.BackColorgroundDown.ToArgb().ToString());
            button.SetAttribute("text", _attrib.Text);
            button.SetAttribute("falseText", _attrib.FText);

            button.SetAttribute("font", _attrib.Font.Name.ToString());
            button.SetAttribute("fontSize", _attrib.Font.Size.ToString());
            button.SetAttribute("Confirm", _attrib.Comfirm.ToString());
            button.SetAttribute("ButtonType", _attrib.ButtonType.ToString());
            button.SetAttribute("ButtonTypeID", _attrib.ButtonPage.PageID.ToString());
            button.SetAttribute("ButtonTypeText", _attrib.ButtonPage.PageText);
            button.SetAttribute("ButtonTypeVar", _attrib.BtnVarText.VarName);
            button.SetAttribute("ButtonTypeVarType", _attrib.BtnVarText.VarType.ToString());
            //按钮使能
            button.SetAttribute("Enabled", _attrib.BtnEnable.ToString());
            button.SetAttribute("EnabledVar", _attrib.EnVarText.VarName);
            button.SetAttribute("EnabledVarType", _attrib.EnVarText.VarType.ToString());
            //按钮图片数据
            button.SetAttribute("IsShowPicture", _attrib.IsShowPic.ToString());
            button.SetAttribute("BtnDownPicFile", _attrib.BtnDownPic.ImageFileName);
            button.SetAttribute("BtnDownPicShowName", _attrib.BtnDownPic.ShowName);
            button.SetAttribute("BtnUpPicFile", _attrib.BtnUpPic.ImageFileName);
            button.SetAttribute("BtnUpPicShowName", _attrib.BtnUpPic.ShowName);
            //趋势图关联
            button.SetAttribute("CurveObj", _attrib.CurveObj);
        }

        public void buildControlToBin(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            _attrib.make(ref pageArrayBin, ref serialize);
        }

        /// <summary>
        /// 按钮按下去的显示状态
        /// </summary>
        /// <param Name="e"></param>
        protected void drawButtonDown(PaintEventArgs e)
        {
            Graphics gh = e.Graphics;
            SolidBrush brush = new SolidBrush(Attrib.BackColorgroundDown);
            gh.FillRectangle(brush, this.ClientRectangle);

            int startX = 0;
            int startY = 0;
            int endX = this.Width + startX;
            int endY = this.Height + startY;
            //
            gh.DrawLine(new Pen(Color.FromArgb(0x80, 0x80, 0x80), 1), new Point(startX, startY), new Point(endX, startY));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(startX + 1, startY + 1), new Point(endX - 1, startY + 1));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(startX + 1, startY + 2), new Point(endX - 1, startY + 2));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(startX + 1, startY + 3), new Point(endX - 1, startY + 3));

            gh.DrawLine(new Pen(Color.FromArgb(0x80, 0x80, 0x80), 1), new Point(startX, startY), new Point(startX, endY));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(startX + 1, startY + 2), new Point(startX + 1, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(startX + 2, startY + 2), new Point(startX + 2, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(startX + 3, startY + 2), new Point(startX + 3, endY - 1));

            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(startX + 1, endY), new Point(endX, endY));
            gh.DrawLine(new Pen(Color.FromArgb(0xc0, 0xc0, 0xc0), 1), new Point(startX + 2, endY - 1), new Point(endX - 1, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0xc0, 0xc0, 0xc0), 1), new Point(startX + 3, endY - 2), new Point(endX - 1, endY - 2));
            gh.DrawLine(new Pen(Color.FromArgb(0xc0, 0xc0, 0xc0), 1), new Point(startX + 4, endY - 3), new Point(endX - 1, endY - 3));

            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(endX, startY), new Point(endX, endY));
            gh.DrawLine(new Pen(Color.FromArgb(0xc0, 0xc0, 0xc0), 1), new Point(endX - 1, startY + 2), new Point(endX - 1, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0xc0, 0xc0, 0xc0), 1), new Point(endX - 2, startY + 3), new Point(endX - 2, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0xc0, 0xc0, 0xc0), 1), new Point(endX - 3, startY + 4), new Point(endX - 3, endY - 1));
        }

        /// <summary>
        /// 按钮的正常显示状态
        /// </summary>
        /// <param Name="e"></param>
        protected void drawButtonNormal(PaintEventArgs e)
        {
            Graphics gh = e.Graphics;
            SolidBrush brush = new SolidBrush(Attrib.BackColorground);
            gh.FillRectangle(brush, this.ClientRectangle);

            int startX = 0;
            int startY = 0;
            int endX = this.Width + startX;
            int endY = this.Height + startY;

            gh.DrawRectangle(new Pen(Color.Black), this.ClientRectangle);

            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(startX + 1, startY + 1), new Point(endX - 1, startY + 1));
            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(startX + 1, startY + 2), new Point(endX - 1, startY + 2));
            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(startX + 1, startY + 3), new Point(endX - 1, startY + 3));
            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(startX + 1, startY + 1), new Point(startX + 1, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(startX + 2, startY + 1), new Point(startX + 2, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0xff, 0xff, 0xff), 1), new Point(startX + 3, startY + 1), new Point(startX + 3, endY - 1));

            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(startX + 1, endY - 1), new Point(endX - 1, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(startX + 2, endY - 2), new Point(endX - 1, endY - 2));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(startX + 3, endY - 3), new Point(endX - 1, endY - 3));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(endX - 1, startY + 1), new Point(endX - 1, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(endX - 2, startY + 2), new Point(endX - 2, endY - 1));
            gh.DrawLine(new Pen(Color.FromArgb(0x55, 0x55, 0x55), 1), new Point(endX - 3, startY + 3), new Point(endX - 3, endY - 1));
        }

        /// <summary>
        /// 为保持与下位机显示效果一致性
        /// 这里对按钮的外观进行绘制
        /// </summary>
        /// <param Name="e"></param>
        override protected void panelOnPaint(PaintEventArgs e)
        {
            _paintEvent(e);
        }

        /// <summary>
        /// 检查当前按钮对象中内容是否合法
        /// 
        /// 不合法-抛出SVCheckValidException异常
        /// </summary>
        public override void checkValid()
        {
            SVPageWidget pageWidget = this.Parent as SVPageWidget;
            String pageName = pageWidget.PageName;

            SVUniqueID uniqueObj = SVUniqueID.instance();

            if (Attrib.ID <= 0 || Attrib.ID >= uniqueObj.MaxID)
            {
                String msg = String.Format("页面 {0} 中,按钮ID为:{1}, ID值已经超出最大范围[{2} - {3}]", pageName, Attrib.ID, 0, uniqueObj.MaxID);
                throw new SVCheckValidException(msg);
            }

            if (!isHasParent())
            {
                String msg = String.Format("页面 {0} 中,按钮ID为:{1}, 没有在页面控件中", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (this.Parent == null)
            {
                String msg = String.Format("页面 {0} 中,按钮ID为:{1}, 没有父控件", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (!this.Parent.ClientRectangle.Contains(this.Bounds))
            {
                String msg = String.Format("页面 {0} 中,按钮ID为:{1}, 已经超出页面显示范围", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            //if (!Attrib.isValidFont())
            //{
            //    String msg = String.Format("页面 {0} 中,按钮ID为:{1}, 设置的文本字体不合法", pageName, Attrib.ID);
            //    throw new SVCheckValidException(msg);
            //}

            //var varInstance = SVVaribleType.instance();
            //if (!varInstance.isOpen())
            //{
            //    String msg = String.Format("数据库打开失败，请检查！");
            //    throw new SVCheckValidException(msg);
            //}

            //var address = varInstance.strToAddress(Attrib.ButtonPage.EnVarText, Attrib.ButtonPage.EnVarTextType);
            //if (address == 0)
            //{
            //    String msg = String.Format("页面 {0} 中, 按钮ID为:{1}, 使能变量未正确设置", pageName, Attrib.ID);
            //    throw new SVCheckValidException(msg);
            //}

            //var varAddress = varInstance.strToAddress(Attrib.ButtonPage.VarText, Attrib.ButtonPage.VarTextType);
            //if (varAddress == 0)
            //{
            //    String msg = String.Format("页面 {0} 中, 按钮ID为:{1}, 按钮关联变量未正确设置", pageName, Attrib.ID);
            //    throw new SVCheckValidException(msg);
            //}

            if (Attrib.IsShowPic)
            {
                if (Attrib.BtnUpPic.bitmap() == null)
                {
                    String msg = String.Format("页面 {0} 中, 按钮ID为:{1}, 按钮弹起图片未设置", pageName, Attrib.ID);
                    throw new SVCheckValidException(msg);
                }

                if (Attrib.BtnDownPic.bitmap() == null)
                {
                    String msg = String.Format("页面 {0} 中, 按钮ID为:{1}, 按钮按下图片未设置", pageName, Attrib.ID);
                    throw new SVCheckValidException(msg);
                }
            }
        }
    }
}
