﻿using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Windows.Forms;
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
        /// 按钮对象执行序列化的过程中，需要重载的构造函数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SVButton(SerializationInfo info, StreamingContext context)
        {
            initButton();
            _attrib = (SVButtonProperties)info.GetValue("stream", typeof(SVButtonProperties));
        }

        /// <summary>
        /// 重写GetObjectData函数，为按钮的序列化服务
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
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
        /// <param name="pos">点坐标，控件的起始位置</param>
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
            this.IsMoved = !_attrib.Lock;

            if (_attrib.IsShowPic)
                _attrib.BackGround = "图片";
            else
                _attrib.BackGround = "颜色";

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
        /// <param name="xml">xml对象</param>
        /// <param name="isCreate">true-创建新的ID号，false-表示使用文件中的ID</param>
        public override void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement button = xml.CurrentElement;

            if (isCreate)
                createID();
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
            String fontFamily = button.GetAttribute("font");
            Single fontSize = float.Parse(button.GetAttribute("fontSize"));
            _attrib.Font = new Font(fontFamily, fontSize);
            _attrib.Comfirm = Boolean.Parse(button.GetAttribute("Confirm"));

            SVBtnTypeConverter vTmp = new SVBtnTypeConverter();  
            vTmp.Type = Byte.Parse(button.GetAttribute("ButtonType"));
            vTmp.PageID = UInt16.Parse(button.GetAttribute("ButtonTypeID"));
            vTmp.VarText = button.GetAttribute("ButtonTypeVar");
            vTmp.PageText = button.GetAttribute("ButtonTypeText");
            vTmp.EnVarText = button.GetAttribute("EnabledVar");
            vTmp.Enable = Boolean.Parse(button.GetAttribute("Enabled"));
            _attrib.BtnType = vTmp;
            //按钮图片数据
            _attrib.IsShowPic = Boolean.Parse(button.GetAttribute("IsShowPicture"));
            _attrib.BtnDownPic.ImageFileName = button.GetAttribute("BtnDownPicFile");
            _attrib.BtnDownPic.ShowName = button.GetAttribute("BtnDownPicShowName");
            _attrib.BtnUpPic.ImageFileName = button.GetAttribute("BtnUpPicFile");
            _attrib.BtnUpPic.ShowName = button.GetAttribute("BtnUpPicShowName");
        }

        /// <summary>
        /// 保存当前按钮内容到xml文件中
        /// </summary>
        /// <param name="xml">xml对象</param>
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
            button.SetAttribute("text", _attrib.Text.ToString());
            button.SetAttribute("font", _attrib.Font.Name.ToString());
            button.SetAttribute("fontSize", _attrib.Font.Size.ToString());
            button.SetAttribute("Confirm", _attrib.Comfirm.ToString());
            button.SetAttribute("ButtonType", _attrib.BtnType.Type.ToString());
            button.SetAttribute("ButtonTypeID", _attrib.BtnType.PageID.ToString());
            button.SetAttribute("ButtonTypeText", _attrib.BtnType.PageText.ToString());
            button.SetAttribute("ButtonTypeVar", _attrib.BtnType.VarText);
            //按钮使能
            button.SetAttribute("Enabled", _attrib.BtnType.Enable.ToString());
            button.SetAttribute("EnabledVar", _attrib.BtnType.EnVarText);
            //按钮图片数据
            button.SetAttribute("IsShowPicture", _attrib.IsShowPic.ToString());
            button.SetAttribute("BtnDownPicFile", _attrib.BtnDownPic.ImageFileName);
            button.SetAttribute("BtnDownPicShowName", _attrib.BtnDownPic.ShowName);
            button.SetAttribute("BtnUpPicFile", _attrib.BtnUpPic.ImageFileName);
            button.SetAttribute("BtnUpPicShowName", _attrib.BtnUpPic.ShowName);
        }

        public void buildControlToBin(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            _attrib.make(ref pageArrayBin, ref serialize);
        }

        /// <summary>
        /// 按钮按下去的显示状态
        /// </summary>
        /// <param name="e"></param>
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
        /// <param name="e"></param>
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
        /// <param name="e"></param>
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

            if (!Attrib.isValidFont())
            {
                String msg = String.Format("页面 {0} 中,按钮ID为:{1}, 设置的文本字体不合法", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }
        }
    }
}
