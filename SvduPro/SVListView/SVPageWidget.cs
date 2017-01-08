using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using SVCore;

namespace SVControl
{
    /// <summary>
    /// 页面控件窗口
    /// </summary>
    public class SVPageWidget : Control, SVInterfacePanel, ICustomTypeDescriptor, SVInterfaceBuild
    {
        private static SVPageWidget _mainPageWidget;
        ///保存当前页面的背景图片数据
        private static Dictionary<String, UInt32> _backGroundPicData = new Dictionary<String, UInt32>();

        /// <summary>
        /// 设置或者获取当前起始页面对象
        /// 
        /// </summary>
        public static SVPageWidget MainPageWidget
        {
            get { return SVPageWidget._mainPageWidget; }
            set 
            {
                if (SVPageWidget._mainPageWidget != null)
                    SVPageWidget._mainPageWidget.IsMainPage = false;

                SVPageWidget._mainPageWidget = value; 
            }
        }

        Boolean _isMainPage = false;

        /// <summary>
        /// 设置或获取当前页面是否为启动页面
        /// 
        /// true:启动页面
        /// false:不是启动页面
        /// </summary>
        public Boolean IsMainPage
        {
            get { return _isMainPage; }
            set 
            {
                if (value == true)
                    MainPageWidget = this;

                _isMainPage = value;
                IsModify = true;
            }
        }

        class SVControlCollection : Control.ControlCollection
        {
            private SVRedoUndo _redoUndoObj;

            public SVRedoUndo RedoUndo
            {
                get { return _redoUndoObj; }
                set { _redoUndoObj = value; }
            }

            public SVControlCollection(Control owner)
                : base(owner)
            {
            }

            public override void Remove(Control value)
            {
                base.Remove(value);

                if (value is SVPanelNode)
                    return;

                //记录重做操作
                SVRedoUndoItem recordItem = new SVRedoUndoItem();
                recordItem.ReDo = () => 
                {
                    SVPanel panel = value as SVPanel;
                    RedoUndo.setEnabled(false);
                    panel.Selected = false;
                    base.Remove(value);
                    RedoUndo.setEnabled(true);
                };
                recordItem.UnDo = () => 
                {
                    SVPanel panel = value as SVPanel;
                    RedoUndo.setEnabled(false);
                    panel.Selected = false;
                    base.Add(value);
                    RedoUndo.setEnabled(true);
                };
                RedoUndo.recordOper(recordItem);
            }
        }

        //当前页面所有相关属性
        private SVPageProperties _attrib = new SVPageProperties();
        private Rectangle _rect = new Rectangle();
        private Boolean _isShowRect = false;
        private SVRedoUndo _redoUndo = new SVRedoUndo();
        private Boolean _isModify;  //当前页面是否被修改

        public event MouseEventHandler MouseSelectEvent;

        public SVRedoUndo RedoUndo
        {
            get { return _redoUndo; }
            set { _redoUndo = value; }
        }

        public SVPageProperties Attrib
        {
            set { _attrib = value;}
            get { return _attrib; }
        }

        /// <summary>
        /// 设置或获取当前页面是否被修改的状态
        /// 
        /// true:表示当前页面有内容被修改
        /// false:无内容被修改
        /// </summary>
        public Boolean IsModify
        {
            get { return _isModify; }
            set
            {
                if (value)
                {
                    SVControlWindow controlWin = this.Parent as SVControlWindow;
                    if (controlWin != null)
                    {
                        String text = controlWin.Text;
                        int index = text.IndexOf(" ");
                        if (index == -1)
                            controlWin.Text = controlWin.Text + " _*";
                    }
                }
                else
                {
                    SVControlWindow controlWin = this.Parent as SVControlWindow;
                    if (controlWin != null)
                    {
                        String text = controlWin.Text;
                        int index = text.IndexOf(" ");
                        if (index > 0)
                            controlWin.Text = text.Remove(index);
                    }
                }

                _isModify = value;
            }
        }

        /// <summary>
        /// 当前页面的名称
        /// 
        /// 当前在树节点中的显示名称
        /// </summary>
        public String PageName { get; set; }

        /// <summary>
        /// 当前页面对应的具体文件名称
        /// </summary>
        public String pageFileName { get; set; }
        public String ClassText { get; set; }

        /// <summary>
        /// 页面控件加载数据
        /// bCreate - true: 创建新ID号
        ///          false: 使用文件中ID号
        /// </summary>
        /// <param Name="bCreate">是否创建新的ID号</param>
        public Boolean loadSelf(Boolean bCreate = false)
        {
            SVXml xml = new SVXml();
            if (!xml.loadXml(pageFileName))
                return false;

            xml.initRootEle("Page");

            ///从xml文件中加载
            loadXML(xml, bCreate);
            ///将数据更新到界面显示
            refreshPropertyToPanel();

            return true;
        }

        /// <summary>
        /// 保存当前页面数据到xml中
        /// </summary>
        public void saveSelf()
        {
            if (!_isModify)
                return;

            SVXml xml = new SVXml();
            xml.createRootEle("Page");
            saveXML(xml);
            xml.writeXml(pageFileName);
        }

        /// <summary>
        /// 页面默认值初始化
        /// </summary>
        void initalize()
        {
            this._isModify = true;
            ///允许接收拖拽
            this.AllowDrop = true;
            ///初始化框选功能
            initRectSelect();
            ///刷新控件
            refreshPropertyToPanel();

            /////////////////////启用双缓冲///////////////////////
            SetStyle(ControlStyles.UserPaint, true);
            /// 禁止擦除背景.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            ///当有撤销和恢复操作发生的时候
            ///将当前页面设置为已经被修改状态
            RedoUndo.operChanged += new OperatorChanged(() =>
            {
                IsModify = true;
            });

            //只要是执行撤销和恢复操作，就更新界面显示
            RedoUndo.UpdateOperator += new DoFunction(() =>
            {
                refreshPropertyToPanel();
            });

            //只要属性值发生变化，就记录，将来进行恢复操作
            Attrib.UpdateControl += new UpdateControl((item) =>
            {
                RedoUndo.recordOper(item);
                //refreshPropertyToPanel();
            });

            ///鼠标拖入控件事件
            ///判断并过滤不是预期的控件类型
            this.DragEnter += new DragEventHandler((sender, e) =>
            {
                if (!e.Data.GetDataPresent(typeof(SVSelectItem)))
                    return;

                e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
            });

            ///页面失去焦点
            ///页面中的所有控件将不再被选中
            //this.LostFocus += new EventHandler((sender, e)=>
            //{
            //    selectAll(false);
            //});
        }

        /// <summary>
        /// 设置当前页面为启动页面
        /// </summary>
        public void setToMainPageWidget()
        {
            MainPageWidget = this;
        }

        /// <summary>
        /// 页面控件初始化
        /// </summary>
        public SVPageWidget()
        {
            initalize();
        }
        
        /// <summary>
        /// 页面控件初始化
        /// </summary>
        /// <param Name="pageName">页面显示名称</param>
        /// <param Name="pageFile">页面对应的实际数据文件</param>
        public SVPageWidget(String pageName, String pageFile)
        {
            initalize();
            PageName = pageName;
            pageFileName = pageFile;
        }

        /// <summary>
        /// 创建页面ID号
        /// </summary>
        public void createID()
        {
            var instance = SVUniqueID.instance();
            _attrib.id = instance.newUniqueID();
            instance.saveFile();
        }

        /// <summary>
        /// 回收页面ID号
        /// </summary>
        public void delID()
        {
            SVUniqueID.instance().delUniqueID(_attrib.id);
        }

        /// <summary>
        /// 控件拖入页面窗口中的事件操作
        /// </summary>
        /// <param Name="e"></param>
        protected override void OnDragDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(SVSelectItem)))
                return;

            ///禁用记录
            _redoUndo.setEnabled(false);

            SVSelectItem item = (SVSelectItem)e.Data.GetData(typeof(SVSelectItem));
            SVPanel btn = (SVPanel)(item._obj);

            btn.setRedoUndoObject(_redoUndo);
            btn.setParentID(Attrib.id);            
            btn.refreshPropertyToPanel();

            btn.Location = this.PointToClient(new Point(e.X, e.Y));
            btn.Location = new Point(btn.Location.X - btn.Width / 2, btn.Location.Y - btn.Height / 2);
            btn.setStartPos(btn.Location);

            this.Controls.Add(btn);
            btn.newID();

            btn.MouseDown += new MouseEventHandler((sder, ev) =>
            {
                MouseSelectEvent(sder, ev);
            });

            btn.MouseUp += new MouseEventHandler((sder, ev) =>
            {
                MouseSelectEvent(sder, ev);
            });


            ///启用记录
            _redoUndo.setEnabled(true);

            ///记录重做操作
            ///这里只记录控件的添加和删除操作，其他属性的改变并不会被记录下来。
            SVRedoUndoItem recordItem = new SVRedoUndoItem();
            recordItem.ReDo = () =>
            {
                _redoUndo.setEnabled(false);
                btn.Selected = false;
                this.Controls.Add(btn);                
                _redoUndo.setEnabled(true);
            };
            recordItem.UnDo = () =>
            {
                _redoUndo.setEnabled(false);
                btn.Selected = false;
                this.Controls.Remove(btn);
                _redoUndo.setEnabled(true);
            };

            ///执行记录
            _redoUndo.recordOper(recordItem);
        }

        protected override Control.ControlCollection CreateControlsInstance()
        {
            SVControlCollection collection = new SVControlCollection(this);
            collection.RedoUndo = _redoUndo;
            return collection;
        }

        /// <summary>
        /// 重载控件的背景绘制函数
        /// </summary>
        /// <param Name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            drawBackGroundGrid(e);
        }

        /// <summary>
        /// 绘制页面控件的背景网格
        /// </summary>
        /// <param Name="e"></param>
        void drawBackGroundGrid(PaintEventArgs e)
        {
            int value = this.BackColor.ToArgb();
            Color color = Color.FromArgb(value ^ 0xFFFFFF);

            Pen pen = new Pen(color, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            pen.DashPattern = new float[] { 2, 7 };

            int width = this.Width;
            int height = this.Height;

            int distance = 100;
            for (int i = distance; i < width; i += distance)
                e.Graphics.DrawLine(pen, new Point(i, 0), new Point(i, height));

            for (int i = distance; i < width; i += distance)
                e.Graphics.DrawLine(pen, new Point(0, i), new Point(width, i));
        }

        /// <summary>
        /// 初始化页面的框选操作
        /// </summary>
        void initRectSelect()
        {
            ///记录鼠标按下位置
            Point recordStartPos = new Point();

            this.MouseDown += new MouseEventHandler((sender, e) =>
            {
                ///如果不是按下鼠标左键,就不记录
                if (e.Button != MouseButtons.Left)
                    return;

                SVSelectPanelObjs._VK_Ctrl = false;
                ///鼠标按下，清空所有控件的选中状态
                selectAll(false);
                ///记录鼠标按下时候的坐标点
                recordStartPos = new Point(e.X, e.Y);
            });


            this.MouseMove += new MouseEventHandler((sender, e) =>
            {
                if (e.Button != MouseButtons.Left)
                    return;

                _rect.Size = new Size(Math.Abs(e.X - recordStartPos.X), Math.Abs(e.Y - recordStartPos.Y));

                int tmpWidth = e.X - recordStartPos.X;
                int tmpHeight = e.Y - recordStartPos.Y;

                if (tmpWidth > 0)
                {
                    if (tmpHeight < 0)
                    {
                        _rect.Location = new Point(recordStartPos.X, recordStartPos.Y + tmpHeight);
                    }
                    else
                    {
                        _rect.Location = recordStartPos;                        
                    }
                }
                else
                {
                    if (tmpHeight < 0)
                    {
                        _rect.Location = new Point(recordStartPos.X + tmpWidth, recordStartPos.Y + tmpHeight);
                    }
                    else
                    {
                        _rect.Location = new Point(recordStartPos.X + tmpWidth, recordStartPos.Y);
                    }
                }

                _isShowRect = true;
                this.Refresh();
            });

            ///是否显示边框
            this.Paint += new PaintEventHandler((sender, e)=>
            {
                int value = this.BackColor.ToArgb();
                Color c = Color.FromArgb(value ^ 0xFFFFFF);

                if (_isShowRect)
                {
                    e.Graphics.DrawRectangle(new Pen(c, 1), _rect);
                }
            });

            ///鼠标弹起执行检查,设置选中控件的状态
            this.MouseUp += new MouseEventHandler((sender, e)=>
            {
                if (e.Button != MouseButtons.Left)
                    return;

                ///将在边框范围内的控件选中
                List<Control> vList = new List<Control>();
                foreach (Control item in this.Controls)
                {
                    SVPanel vPanel = item as SVPanel;
                    if (vPanel == null)
                        continue;

                    if (_rect.Contains(vPanel.Bounds))
                        vList.Add(vPanel);
                }

                foreach (var item in vList)
                    ((SVPanel)item).Selected = true;

                _isShowRect = false;
                this.Refresh();
            });
        }

        /// <summary>
        /// 设置当前页面所有控件的选中状态
        /// </summary>
        /// <param Name="enabled">true表示所有控件选中,false表示所有控件反选</param>
        public void selectAll(Boolean enabled)
        {
            List<SVPanel> vList = new List<SVPanel>();
            foreach (Control item in this.Controls)
            {
                SVPanel panel = item as SVPanel;
                if (panel == null)
                    continue;

                if (enabled)
                {
                    if (panel.IsMoved)
                        vList.Add(panel);
                }
                else
                    vList.Add(panel);
            }

            RedoUndo.setEnabled(false);
            foreach (var item in vList)
                item.Selected = enabled;

            RedoUndo.setEnabled(true);
            this.Refresh();
        }

        /// <summary>
        /// 重载，获取当前页面的属性
        /// </summary>
        /// <returns></returns>
        public object property()
        {
            return _attrib;
        }

        //设置当前控件的默认属性
        public void refreshPropertyToPanel()
        {
            this.Width = _attrib.Width;
            this.Height = _attrib.Height;

            if (_attrib.BackGroundType == 0)
            {
                this.BackColor = _attrib.BackColor;
                this.BackgroundImage = null;
            }
            else
            {
                //设置背景
                this.BackgroundImageLayout = ImageLayout.Stretch;
                this.BackgroundImage = Attrib.PicIconData.bitmap();
            }
        }
        
        /*
         * 通过XML文件来加载页面控件对象
         * 
         * xml - 一个页面对应的xml对象
         * isCreate - true: 创建新的ID号
         *            false: 直接使用文件中的ID号 
         * */
        public void loadXML(SVXml xml, Boolean isCreate = false)
        {
            _redoUndo.setEnabled(false);

            XmlElement page = xml.CurrentElement;

            if (isCreate)
            {
                var instance = SVUniqueID.instance();
                _attrib.id = instance.newUniqueID();
                instance.saveFile();
            }
            else
                _attrib.id = UInt16.Parse(page.GetAttribute("ID"));

            XmlElement width = xml.select("Width");
            _attrib.Width = int.Parse(width.InnerText);

            XmlElement height = xml.select("Height");
            _attrib.Height = int.Parse(height.InnerText);

            XmlElement back = xml.select("Backcolor");
            _attrib.BackColor = Color.FromArgb(int.Parse(back.InnerText));

            XmlElement mainPage = xml.select("MainPage");
            IsMainPage = Boolean.Parse(mainPage.InnerText);

            XmlElement backGroundType = xml.select("BackGroundType");
            _attrib.BackGroundType = Byte.Parse(backGroundType.InnerText);

            XmlElement picShowName = xml.select("PicShowName");
            _attrib.PicIconData.ShowName = picShowName.InnerText;

            XmlElement picFile = xml.select("PicFile");
            _attrib.PicIconData.ImageFileName = picFile.InnerText;

            //读取当前页面的所有子控件
            List<XmlElement> listNodes = xml.selectChilds();
            foreach (XmlElement node in listNodes)
            {
                SVPanel svPanel = SVNameToObject.getInstance(node.Name) as SVPanel;
                if (svPanel == null)
                    continue;

                svPanel.setRedoUndoObject(_redoUndo);
                svPanel.setParentID(_attrib.id);

                svPanel.MouseDown += new MouseEventHandler((sder, ev) =>
                {
                    MouseSelectEvent(sder, ev);
                });

                svPanel.MouseUp += new MouseEventHandler((sder, ev) =>
                {
                    MouseSelectEvent(sder, ev);
                });

                this.Controls.Add(svPanel);
                xml.CurrentElement = node;
                svPanel.loadXML(xml, isCreate);
                svPanel.refreshPropertyToPanel();
            }

            _redoUndo.setEnabled(true);
        }

        public void saveXML(SVXml xml)
        {
            //保存后修改状态。
            IsModify = false;

            xml.setAttr("ID", _attrib.id.ToString());

            XmlElement width = xml.createNode("Width");
            width.InnerText = _attrib.Width.ToString();

            XmlElement height = xml.createNode("Height");
            height.InnerText = _attrib.Height.ToString();

            XmlElement back = xml.createNode("Backcolor");
            back.InnerText = _attrib.BackColor.ToArgb().ToString();

            XmlElement mainPage = xml.createNode("MainPage");
            mainPage.InnerText = IsMainPage.ToString();

            XmlElement backGroundType = xml.createNode("BackGroundType");
            backGroundType.InnerText = _attrib.BackGroundType.ToString();

            XmlElement picShowName = xml.createNode("PicShowName");
            picShowName.InnerText = _attrib.PicIconData.ShowName;

            XmlElement picFile = xml.createNode("PicFile");
            picFile.InnerText = _attrib.PicIconData.ImageFileName;

            //保存当前页面的所有子控件
            foreach (Control ctrl in this.Controls)
            {
                SVPanel svPanel = ctrl as SVPanel;
                if (svPanel == null)
                    continue;

                svPanel.saveXML(xml);
            }
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<PropertyDescriptor> resultList = new List<PropertyDescriptor>();

            PropertyDescriptorCollection tmp = TypeDescriptor.GetProperties(property(), attributes);
            foreach (PropertyDescriptor des in tmp)
                resultList.Add(des);
            return new PropertyDescriptorCollection(resultList.ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return property();
        }

        void copyDestByteArray(byte[] src, byte[] dest)
        {
            int minLen = src.Length > dest.Length ? dest.Length : src.Length;
            Array.Copy(src, dest, minLen);
        }

        /// <summary>
        /// 清除缓冲的背景图片信息
        /// </summary>
        public static void clearBackGroundData()
        {
            _backGroundPicData.Clear();
        }

        public void buildControlToBin(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            if (pageArrayBin.pageArray == null)
                pageArrayBin.pageArray = new PageBin[SVLimit.MAX_PAGE_NUMBER];

            UInt32 nCount = pageArrayBin.pageCount;
            PageBin pageBin = pageArrayBin.pageArray[nCount];

            pageBin.id = _attrib.id;
            pageBin.bgSet = 0;

            pageBin.bgClr = (UInt32)_attrib.BackColor.ToArgb();
            pageBin.index = (UInt16)pageArrayBin.pageCount;
            pageBin.pointAlign = _attrib.IsAlignment;           

            var address = _attrib.PicIconData.bitmap24Data(this.Width, this.Height);
            if (address != null)
            {
                //设置标志
                pageBin.bgSet = 1;

                //背景图标位置
                if (_backGroundPicData.ContainsKey(_attrib.PicIconData.ShowName))
                {
                    pageBin.bgClr = _backGroundPicData[_attrib.PicIconData.ShowName];
                }
                else
                {
                    pageBin.bgClr = (UInt32)serialize.ToArray().Length;
                    _backGroundPicData.Add(_attrib.PicIconData.ShowName, pageBin.bgClr);
                    serialize.pack(address);
                }
            }

            pageArrayBin.pageArray[nCount] = pageBin;
            //遍历所有子控件
            foreach (Control ctrl in this.Controls)
            {
                SVInterfaceBuild bin = ctrl as SVInterfaceBuild;
                if (bin == null)
                    continue;
                bin.buildControlToBin(ref pageArrayBin, ref serialize);
            }
            
            pageArrayBin.pageCount++;
        }
    }
}
