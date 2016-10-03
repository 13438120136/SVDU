using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SVControl;
using SVCore;
using SVSimulation;
using WeifenLuo.WinFormsUI.Docking;

namespace SvduPro
{
    /// <summary>
    /// 程序的主窗口界面
    /// 
    /// 程序的主要入口, 相当于main函数
    /// </summary>
    public partial class SVDockMainWindow : Form, SVInterfaceApplication
    {
        /// <summary>
        /// 资源类，用来国际化内部字符串
        /// </summary>
        ResourceManager _resources = new ResourceManager(typeof(Resource));

        private SVControlWindow _projectWindow;
        private SVControlWindow _ctlListViewWindow;
        private SVControlWindow _propertyWindow;
        private SVControlWindow _outPutWindow;
        private SVControlWindow _findWindow;
        private SVControlWindow _objectWindow;

        private DockPanel _dockPanel;

        /// <summary>
        /// 工程树
        /// </summary>
        private SVTreeView _stationTreeView;

        /// <summary>
        /// 控件窗口列表
        /// </summary>
        private SVListView _controlListView;

        /// <summary>
        /// 属性窗口
        /// </summary>
        private PropertyGrid _propertyGrid;

        /// <summary>
        /// 工程对象，记录当前工程内容相关信息
        /// </summary>
        private SVProject _svProject = new SVProject();

        /// <summary>
        /// 输出窗口控件
        /// </summary>
        private SVOutputTextBox _outPutText;

        /// <summary>
        /// 对象控件
        /// </summary>
        private SVObjTreeView _objTreeView;

        /// <summary>
        /// 查找文本控件
        /// </summary>
        private SVFindTextBox _findText;

        /// <summary>
        /// 当前工程树窗口对象
        /// </summary>
        public Control TreeProject
        {
            get
            {
                return _stationTreeView;
            }
        }

        /// <summary>
        /// 当前查找窗口
        /// </summary>
        public Control FindWindow 
        { 
            get
            {
                return _findText;
            }
        }

        /// <summary>
        /// 返回得到数据库操作对象
        /// </summary>
        public SVSqlDataBase DataBase
        {
            get
            {
                return _svProject.sqlDataBase();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SVDockMainWindow(String[] args)
        {
            ///注册当前对象
            SVApplication.Instance = this;
            ///界面显示
            InitializeComponent();
            initDockPanel();         
            initControlObject();
            ///导航树操作
            initTreeViewOperation();
            ///最近打开列表
            initRecentMenuItem();
            ///定时保存
            initTimeSaves();
            ///注册查找窗口事件
            initFindWindowEvent();
            ///初始化
            init();

            ///参数
            commandLine(args);
            //Environment.Exit(0);            
            
            ///获取屏幕分辨率
            Rectangle ScreenArea = Screen.GetWorkingArea(this);
            this.Location = ScreenArea.Location;
            this.Width = ScreenArea.Width;
            this.Height = ScreenArea.Height;
        }

        /// <summary>
        /// 处理命令行参数命令
        /// 
        /// 1、不带任何参数，执行打开软件
        /// 2、-p file, 打开指定的工程文件
        /// 3、-c file, 编译指定的工程文件
        /// 4、-n proName, 在当前目录创建工程，如果已经创建就打开
        /// </summary>
        /// <param name="args">输入参数</param>
        void commandLine(String[] args)
        {
            if (args.Length == 0)
                return;

            ///创建工程
            if (args[0] == "-n")
            {
                if (args.Length < 2)
                    return;

                if (String.IsNullOrWhiteSpace(args[1]))
                    return;

                String proName = args[1];
                openProject(".", proName);
                return;
            }

            ///打开软件的同时，打开工程
            if (args[0] == "-p")
            {
                if (args.Length < 2)
                    return;

                if (String.IsNullOrWhiteSpace(args[1]))
                    return;

                String path = Path.GetDirectoryName(args[1]);
                String proPath = Directory.GetParent(path).FullName;
                String proName = Path.GetFileNameWithoutExtension(args[1]);
                openProject(proPath, proName);
                return;
            }

            ///打开软件的同时，编译工程
            if (args[0] == "-c")
            {
                if (args.Length < 2)
                    return;

                if (String.IsNullOrWhiteSpace(args[1]))
                    return;

                String path = Path.GetDirectoryName(args[1]);
                String proPath = Directory.GetParent(path).FullName;
                String proName = Path.GetFileNameWithoutExtension(args[1]);
                openProject(proPath, proName);

                try
                {
                    SVCheckBeforeBuild check = new SVCheckBeforeBuild();
                    check.checkAll();
                    buildDownLoadFiles();
                    return;
                }
                catch (SVCheckValidException ex)
                {
                    SVLog.WinLog.Info(ex.Message);
                    _outPutWindow.Activate();
                }
            }
        }

        /// <summary>
        /// 注册查找窗口事件
        /// </summary>
        void initFindWindowEvent()
        {
            _findText.MouseDoubleClick += new MouseEventHandler((sender, e) =>
            {
                SVPageWidget widget = _findText.getMarkObject() as SVPageWidget;
                if (widget == null)
                    return;

                ///双击事件打开对应页面
                if (_svProject.isExist(widget.PageName))
                    openPage(widget);
            });
        }

        /*
         * 初始化最近打开列表
         * */
        void initRecentMenuItem()
        {
            var listFils = SVConfig.instance().RectFileItems;
            最近打开ToolStripMenuItem.DropDownItems.Clear();

            foreach (String str in listFils)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(str);
                最近打开ToolStripMenuItem.DropDownItems.Add(item);
                item.Click += new EventHandler((sender, e) => 
                {
                    String file = item.Text;
                    String path = Path.GetDirectoryName(file);
                    String proPath = Directory.GetParent(path).FullName;
                    String proName = Path.GetFileNameWithoutExtension(file);
                    openProject(proPath, proName);
                });
            }
        }

        /// <summary>
        /// 初始化定时保存功能
        /// 保存间隔以分钟为单位
        /// </summary>
        void initTimeSaves()
        {
            Timer tm = new Timer();
            tm.Interval = SVConfig.instance().SaveInterval * 60 * 1000;
            tm.Start();
            tm.Tick += new EventHandler((sender, e) =>
            {
                savePro();
            });
        }

        /// <summary>
        /// 在工作区中打开页面
        /// </summary>
        /// <param name="backPageWidget">页面窗口对象</param>
        void addPageToWorkSpace(SVPageWidget widget)
        {
            foreach (SVControlWindow item in _dockPanel.Documents)
            {
                if (item.Controls.Contains(widget))
                {
                    item.Activate();
                    return;
                }
            }

            widget.Parent = this;
            SVControlWindow workWindow = new SVControlWindow(widget);
            widget.Dock = DockStyle.None;
            workWindow.Text = widget.PageName;
            workWindow.ToolTipText = widget.pageFileName;
            workWindow.Show(_dockPanel, DockState.Document);
            workWindow.Activate();
        }

        /// <summary>
        /// 初始化各个主要窗口的摆放位置
        /// </summary>
        void initControlObject()
        {
            this.toolStripContainer1.ContentPanel.Controls.Add(_dockPanel);

            _stationTreeView = new SVTreeView();

            _propertyGrid = new PropertyGrid();
            _propertyGrid.Font = new Font(_propertyGrid.Font.Name, _propertyGrid.Font.Size + 2);
            _propertyGrid.Size = new System.Drawing.Size(180, 180);
            _propertyWindow = new SVControlWindow(_propertyGrid);
            _propertyWindow.Text = _resources.GetString("属性窗口");
            _propertyWindow.Show(_dockPanel, DockState.DockRight);

            _objTreeView = new SVObjTreeView();
            _objectWindow = new SVControlWindow(_objTreeView);
            _objectWindow.Text = _resources.GetString("对象窗口");
            _objectWindow.Show(_dockPanel, DockState.DockRight);
            _propertyWindow.Activate();

            _projectWindow = new SVControlWindow(_stationTreeView);
            _projectWindow.Text = _resources.GetString("工程窗口");
            _projectWindow.Show(_dockPanel, DockState.DockLeft);
            _projectWindow.Activate();

            _controlListView = new SVControl.SVListView();
            _ctlListViewWindow = new SVControlWindow(_controlListView);
            _ctlListViewWindow.Text = _resources.GetString("控件窗口");
            _ctlListViewWindow.Show(_dockPanel, DockState.DockLeft);
            //工程窗口优先显示
            _projectWindow.Show();

            _outPutText = SVOutputTextBox.instance();
            _outPutWindow = new SVControlWindow(_outPutText);
            _outPutWindow.Text = _resources.GetString("输出窗口");
            _outPutWindow.Show(_dockPanel, DockState.DockBottom);

            _findText = new SVFindTextBox();
            _findText.Multiline = true;
            _findText.ReadOnly = true;
            _findText.BackColor = Color.White;
            _findWindow = new SVControlWindow(_findText);
            _findWindow.Text = _resources.GetString("查找窗口");
            _findWindow.Show(_dockPanel, DockState.DockBottom);
            _outPutWindow.Activate();

            _dockPanel.UpdateDockWindowZOrder(DockStyle.Left, true);
            _dockPanel.UpdateDockWindowZOrder(DockStyle.Right, true);
        }

        /// <summary>
        /// 初始化dockPanel
        /// </summary>
        void initDockPanel()
        {
            _dockPanel = new DockPanel();
            _dockPanel.ActiveAutoHideContent = null;
            _dockPanel.Dock = DockStyle.Fill;
            _dockPanel.DockBackColor = SystemColors.ActiveBorder;
            _dockPanel.DockBottomPortion = 150.0;
            _dockPanel.DockLeftPortion = 300.0;
            _dockPanel.DockRightPortion = 300.0;
            _dockPanel.DockTopPortion = 150.0;
            _dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            _dockPanel.Parent = this;

            _dockPanel.ActiveDocumentChanged += new EventHandler(_dockPanel_ActiveDocumentChanged);
            //_dockPanel.DockAreas = (DockAreas.Document | DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom);
            //this._dockPanel.Location = new Point(0, 49);
            //this._dockPanel.Name = "dockPanel";
            //this._dockPanel.RightToLeftLayout = true;
            //this._dockPanel.Size = new Size(579, 338);
            //this._dockPanel.TabIndex = 6;
            this.Controls.Add(_dockPanel);
        }

        /// <summary>
        /// 只要进行切换就去掉选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            SVSelectPanelObjs.clearSelectControls();
        }


        /// <summary>
        /// 工程节点注册事件
        /// 
        /// 执行双击节点打开对应页面
        /// </summary>
        void initTreeViewOperation()
        {
            initStationMenu();

            //鼠标单击事件后操作
            this._stationTreeView.AfterSelect += new TreeViewEventHandler(_stationTreeView_AfterSelect);
            //双击节点
            _stationTreeView.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(_stationTreeView_NodeMouseDoubleClick);
        }

        /// <summary>
        /// 双击工程树节点，打开页面的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _stationTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SVPageNode node = _stationTreeView.SelectedNode as SVPageNode;
            if (node == null)
                return;

            SVPageWidget widget = node.Addtionobj as SVPageWidget;
            if (widget == null)
                return;

            //打开页面
            openPage(widget);
        }

        /// <summary>
        /// 工程节点单击后的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _stationTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_stationTreeView.SelectedNode == null)
                return;
        }

        /// <summary>
        /// 获取当前激活的页面窗口对象
        /// </summary>
        /// <returns>获取当前激活的页面窗口对象</returns>
        SVPageWidget currentPageWidget()
        {
            SVControlWindow controlWindow = _dockPanel.ActiveDocument as SVControlWindow;
            if (controlWindow == null)
                return null;

            SVPageWidget widget = null;
            foreach (Control child in controlWindow.Controls)
            {
                widget = child as SVPageWidget;
                if (widget == null)
                    return null;

                break;
            }

            return widget;
        }

        /// <summary>
        /// 初始化通用鼠标移动和点击事件
        /// </summary>
        void initMouseClickEvent()
        {
            //状态栏显示复制对象
            SVSelectPanelObjs.CopyEvents = (c) => 
            {
                String text = String.Format("已复制{0}个对象.", c.Count);
                this.undoStatusLabel.Text = text;
            };

            //状态栏显示选中对象
            SVSelectPanelObjs.SelectEvents = new SelectControlEvents((v) =>
            {
                this._propertyGrid.SelectedObjects = v.ToArray();
                String text = String.Format("已选中{0}个对象.", v.Count);
                this.undoStatusLabel.Text = text;
            });

            //执行粘贴操作
            SVSelectPanelObjs.PasteEvent = ((c) =>
            {
                sortControlList(c);

                SVPageWidget widget = currentPageWidget();
                if (widget == null)
                    return;

                /// 如果复制的位置超出屏幕范围，就在距离左上角(20,20)的位置摆放所有控件
                /// 否则就在鼠标位置摆放所有控件
                if (checkRange(c, widget, widget.PointToClient(MousePosition)))
                    paste(c, widget.PointToClient(MousePosition));
                else
                    paste(c, new Point(20, 20));
            });

            //属性窗口发生变化后
            this._propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler((sender1, ev) =>
            {
                foreach (var item in this._propertyGrid.SelectedObjects)
                {
                    SVInterfacePanel window = item as SVInterfacePanel;
                    if (window == null)
                        continue ;

                    window.refreshPropertyToPanel();
                }
            });
        }

        /// <summary>
        /// 检查当前要被粘贴的对象队列是否在粘贴点超出页面范围
        /// 
        /// 返回：true表示在页面范围内
        ///       false超出页面显示范围
        /// </summary>
        /// <param name="vList">粘贴控件对象队列</param>
        /// <param name="backPageWidget">页面窗口对象</param>
        /// <param name="point">要检查的粘贴点</param>
        /// <returns>true和false</returns>
        Boolean checkRange(List<Control> vList, SVPageWidget backPageWidget, Point point)
        {
            foreach (var item in vList)
            {
                Rectangle rect = new Rectangle(point, item.Size);
                if (!backPageWidget.ClientRectangle.Contains(rect))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 粘贴事件发生的时候执行具体操作
        /// </summary>
        /// <param name="vList">执行粘贴操作的控件队列</param>
        /// <param name="point">页面中要粘贴的位置点</param>
        void paste(List<Control> vList, Point point)
        {
            if (vList.Count == 0)
                return;

            SVPageWidget widget = currentPageWidget();
            if (widget == null)
                return;

            Control first = vList[0];
            Int32 disx = point.X - first.Location.X;
            Int32 disy = point.Y - first.Location.Y;

            foreach (var item in vList)
            {
                SVPanel svPanel = item as SVPanel;
                ///不合法的控件将被过滤
                if (svPanel == null)
                    continue;

                ///设置撤销恢复对象
                svPanel.setRedoUndoObject(widget.RedoUndo);
                ///设置父窗口ID，这里的父窗口为页面窗口
                svPanel.setParentID(widget.Attrib.id);
                ///新建当前控件ID号
                svPanel.createID();

                ///设置相对位置
                svPanel.Location = new Point(svPanel.Location.X + disx, svPanel.Location.Y + disy);
                svPanel.setStartPos(svPanel.Location);

                ///添加控件到页面中
                widget.Controls.Add(svPanel);
                ///将控件显示位置置顶
                svPanel.BringToFront();
            }
        }

        /// <summary>
        /// 对当前控件队列进行排序
        /// </summary>
        /// <param name="vList">作为输入和输出，输入控件队列。输出排序后的队列</param>
        void sortControlList(List<Control> vList)
        {
            vList.Sort((one, two) =>
            {
                Point a = one.Location;
                Point b = two.Location;

                int x = a.X * a.X + a.Y * a.Y;
                int y = b.X * b.X + b.Y * b.Y;

                return x.CompareTo(y);
            });
        }

        /// <summary>
        /// 创建页面节点
        /// backPageWidget - 页面控件对象
        /// 根据页面控件对象来初始化页面节点,这里必须保证页面对象完整创建
        /// </summary>
        /// <param name="widget">当前页面控件对象</param>
        /// <returns>返回新创建的页面节点</returns>
        SVPageNode newPageFromWidget(SVPageWidget widget)
        {
            SVPageNode pageNode = new SVPageNode(widget.PageName);

            ///如果编辑后的节点为当前页面节点,就进行重命名
            _stationTreeView.AfterLabelEdit += new NodeLabelEditEventHandler((sender, e)=>
            {
                if (pageNode.Equals(e.Node))
                {
                    if (String.IsNullOrWhiteSpace(e.Node.Text) ||
                        String.IsNullOrWhiteSpace(e.Label))
                    {
                        e.CancelEdit = true;
                        return;
                    }

                    _svProject.renamePageName(pageNode.Parent.Text, e.Node.Text, e.Label);
                    widget.PageName = e.Label;
                }
            });

            ///设置节点的图标
            pageNode.ImageKey = "Page";
            pageNode.SelectedImageKey = "Page";
            ///记录当前节点对应页面控件
            pageNode.Addtionobj = widget;

            SVGlobalData.addPage(widget.PageName, widget);

            widget.MouseMove += new MouseEventHandler((sender, e)=>
            {
                String text = String.Format("X:{0}, Y:{1} ", e.X, e.Y);
                this.statusLabel.Text = text;
            });

            widget.MouseSelectEvent += new MouseEventHandler((sder, ev) =>
            {
                //if (SVSelectPanelObjs._VK_Ctrl == true)
                //    return;

                //this._propertyGrid.SelectedObject = sder;
                //this._objTreeView.setPageWidget(widget);
            });

            widget.MouseDown += new MouseEventHandler((sender, e) =>
            {
                this.undoStatusLabel.Text = "";
                this._propertyGrid.SelectedObject = widget;
                this._objTreeView.setPageWidget(widget);
            });

            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripItem startItem = menu.Items.Add(_resources.GetString("设为启动页面"));
            startItem.Click += new EventHandler((sender, e) =>
            {
                pageNode.setMainNode();
                widget.IsMainPage = true;
            });

            ToolStripItem openItem = menu.Items.Add(_resources.GetString("打开"));
            openItem.Click += new EventHandler((sender, e) =>
            {
                openPage(widget);
            });

            ToolStripItem closeItem = menu.Items.Add(_resources.GetString("关闭"));
            closeItem.Click += new EventHandler((sender, e) =>
            {
                closePage(widget);
            });

            ToolStripItem removeItem = menu.Items.Add(_resources.GetString("移除"));
            removeItem.Click += new EventHandler((sender, e) =>
            {
                removePage(pageNode);
            });

            menu.Items.Add(new ToolStripSeparator());

            ToolStripItem renameItem = menu.Items.Add(_resources.GetString("重命名"));
            renameItem.Click += new EventHandler((sender, e) =>
            {
                _stationTreeView.renameTreeNode(pageNode);
            });

            menu.Items.Add(new ToolStripSeparator());
            ToolStripItem checkItem = menu.Items.Add(_resources.GetString("页面检查"));
            checkItem.Click += new EventHandler((sender, e) =>
            {
                checkPageValid();
            });

            menu.Items.Add(new ToolStripSeparator());
            ToolStripItem templateItem = menu.Items.Add(_resources.GetString("保存为模板"));
            templateItem.Click += new EventHandler((sender, e) =>
            {
                exportToTemplate(widget);
            });

            menu.Items.Add(new ToolStripSeparator());
            ToolStripItem printItem = menu.Items.Add(_resources.GetString("打印"));
            printItem.Click += new EventHandler((sender, e) =>
            {
                Bitmap ctlbitmap = new Bitmap(widget.Width, widget.Height);
                widget.DrawToBitmap(ctlbitmap, widget.ClientRectangle);

                //打印
                SVPrinter printPixmap = new SVPrinter();
                printPixmap.printBmp(ctlbitmap);
            });

            menu.Items.Add(new ToolStripSeparator());
            ToolStripItem delItem = menu.Items.Add(_resources.GetString("删除"));
            delItem.Click += new EventHandler((sender, e) =>
            {
                delPage(pageNode);
            });

            menu.Opened += new EventHandler((sender, e) =>
            {
                startItem.Enabled = !widget.Equals(SVPageWidget.MainPageWidget);
            });
            pageNode.ContextMenuStrip = menu;

            return pageNode;
        }

        /// <summary>
        /// 工程树节点的右键菜单
        /// </summary>
        void initStationMenu()
        {
            ///////////////////////////工程节点//////////////////////////////
            ContextMenuStrip proMenu = new ContextMenuStrip();
            ToolStripItem newClassItem = proMenu.Items.Add(_resources.GetString("新建页面分类"));
            newClassItem.Click += new EventHandler((sender, e)=>
            {
                createPageClass("页面分类");
            });

            _stationTreeView.ProContextMenu = proMenu;
        }

        /// <summary>
        /// 添加页面新分类
        /// 根据名称创建新的分类节点
        /// </summary>
        /// <param name="className">分类名称</param>
        /// <returns>返回新的页面分类节点</returns>
        private TreeNode createPageClass(String className)
        {
            TreeNode pageTreeNode = _stationTreeView.newClassNode(className);

            ///如果修改的是当前节点，就需要重新命名分类名称
            _stationTreeView.AfterLabelEdit += new NodeLabelEditEventHandler((sender, e)=>
            {
                if (pageTreeNode.Equals(e.Node))
                {
                    if (String.IsNullOrWhiteSpace(e.Node.Text)
                         || String.IsNullOrWhiteSpace(e.Label))
                    {
                        e.CancelEdit = true;
                        return;
                    }

                    _svProject.renamePageClassName(e.Node.Text, e.Label);
                }
            });

            ///以下是添加当前分类的所有右键菜单
            ContextMenuStrip menu = new ContextMenuStrip();
            pageTreeNode.ContextMenuStrip = menu;

            ToolStripItem newPageItem = menu.Items.Add(_resources.GetString("新建页面"));
            newPageItem.Click += new EventHandler((sender, e) =>
            {
                createPage(pageTreeNode);
            });

            ToolStripItem importItem = menu.Items.Add(_resources.GetString("导入页面"));
            importItem.Click += new EventHandler((sender, e) =>
            {
                importPage(pageTreeNode);
            });

            ///加入分隔符
            menu.Items.Add(new ToolStripSeparator());

            ToolStripItem renameItem = menu.Items.Add(_resources.GetString("重命名"));
            renameItem.Click += new EventHandler((sender, e) =>
            {
                _stationTreeView.renameTreeNode(pageTreeNode);
            });

            ///加入分隔符
            menu.Items.Add(new ToolStripSeparator());
            ToolStripItem delItem = menu.Items.Add(_resources.GetString("删除分类"));
            delItem.Click += new EventHandler((sender, e) =>
            {
                SVMessageBox msgBox = new SVMessageBox();
                msgBox.content(Resource.提示, Resource.确定删除分类);
                DialogResult result = msgBox.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.No)
                    return;

                ///遍历移除所有的页面中的控件元素
                foreach (var item in pageTreeNode.Nodes)
                {
                    SVPageNode node = item as SVPageNode;
                    if (node == null)
                        continue;

                    removePage(node);
                }

                ///移除树节点
                _stationTreeView.Nodes.Remove(pageTreeNode);
                ///移除配置信息
                _svProject.removeClassName(pageTreeNode.Text);
            });

            return pageTreeNode;
        }

        /// <summary>
        /// init
        /// </summary>
        void init()
        {
            initShortCuts();
            initMouseClickEvent();
            initMenuItem();
        }

        /// <summary>
        /// 初始日志及数据库
        /// </summary>
        void initLogAndDataBase()
        {
            ///日志
            SVTextBoxTraceListenter textBoxListener = new SVTextBoxTraceListenter(_outPutText);
            SVLog.WinLog.Listeners().Add(textBoxListener);
            SVLog.WinLog.Info("日志系统启动成功");
        }

        /// <summary>
        /// 窗口菜单的事件
        /// </summary>
        void initMenuItem()
        {
            //用户单击退出按钮
            this.exitMenuItem.Click += new EventHandler((sender, e) =>
            {
                closeProject();
                Close();
            });

            //窗体关闭
            this.FormClosing += new FormClosingEventHandler((sender, e) =>
            {
                e.Cancel = false;
                closeProject();
            });
        }

        /// <summary>
        /// 打开系统快捷键触发
        /// </summary>
        void initShortCuts()
        {
            this.KeyPreview = true;
        }

        /// <summary>
        /// 移除页面
        /// </summary>
        /// <param name="pageNode">页面节点</param>
        void removePage(SVPageNode pageNode)
        {
            SVPageWidget widget = pageNode.Addtionobj as SVPageWidget;
            if (widget == null)
                return;

            widget.delID();
            SVGlobalData.removePage(pageNode.Text);

            closePage(pageNode);
            pageNode.Parent.Nodes.Remove(pageNode);
            _svProject.removePageNode(pageNode.Text);
        }

        /// <summary>
        /// 删除页面
        /// </summary>
        /// <param name="pageNode">页面节点</param>
        void delPage(SVPageNode pageNode)
        {
            SVPageWidget widget = pageNode.Addtionobj as SVPageWidget;
            if (widget == null)
                return;

            SVMessageBox msgBox = new SVMessageBox();
            msgBox.content(Resource.提示, Resource.确定删除页面 + String.Format(" {0}?", widget.PageName));

            var result = msgBox.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                if (File.Exists(widget.pageFileName))
                    File.Delete(widget.pageFileName);

                removePage(pageNode);
            }
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param name="backPageWidget">页面节点</param>
        void closePage(SVPageWidget widget)
        {
            //添加属性
            this._propertyGrid.SelectedObject = null;
            //清除对象窗口
            this._objTreeView.clearAllNodes();
            //关闭工作区
            SVControlWindow win = widget.Parent as SVControlWindow;
            if (win != null)
                win.Close();
        }

        /*
         *  选中当前树形导航节点的页面节点，执行打开。
         *  参数：选中的当前节点
         */
        void openPage(SVPageWidget widget)
        {
            //工作区中显示
            addPageToWorkSpace(widget);
            //属性窗口中显示属性
            _propertyGrid.SelectedObject = widget.Attrib;
        }

        /*
         * 选中当前节点并关闭
         * 参数：选中的节点
         */
        void closePage(SVPageNode node)
        {
            //添加属性
            this._propertyGrid.SelectedObject = null;
            //清除对象窗口
            this._objTreeView.clearAllNodes();

            //关闭工作区
            SVPageWidget page = currentPageWidget();
            if (page != null)
            {
                SVControlWindow win = page.Parent as SVControlWindow;
                if (win != null)
                    win.Close();
            }
        }

        /// <summary>
        /// 加载当前工程中的所有页面数据
        /// </summary>
        public void loadAllPageData()
        {
            var data = _svProject.getDictData();
            if (data.Count == 0)
                return;

            foreach (var classItem in data)
            {
                ///分类节点
                TreeNode treeNode = createPageClass(classItem.Key);
                ///拷贝一份页面值对象,目的是删除配置信息不发生异常
                var pageValue = new Dictionary<String, String>(classItem.Value);

                ///遍历页面节点
                foreach (var item in pageValue)
                {
                    String tmpPageFile = Path.Combine(Directory.GetCurrentDirectory(), item.Value);
                    ///检查页面文件是否存在，如果不存在就提示
                    if (!File.Exists(tmpPageFile))
                    {
                        String str = String.Format("{0}", item.Value) + Resource.页面不存在;
                        SVLog.WinLog.Warning(str);
                        classItem.Value.Remove(item.Key);
                        continue;
                    }

                    SVPageWidget widget = new SVPageWidget(item.Key, item.Value);
                    widget.ClassText = treeNode.Text;
                    widget.loadSelf();

                    SVPageNode pageNode = newPageFromWidget(widget);
                    treeNode.Nodes.Add(pageNode);

                    ///如果是启动页面
                    if (widget.IsMainPage)
                    {
                        pageNode.setMainNode();
                    }
                }
            }

            _stationTreeView.ExpandAll();
        }

        /// <summary>
        /// 根据输入名称打开工程,如果工程不存在则执行创建
        /// </summary>
        /// <param name="path">工程路径</param>
        /// <param name="name">工程名</param>
        void openProject(String path, String name)
        {
            Directory.SetCurrentDirectory(path);
            ///初始化日志系统及数据库
            initLogAndDataBase();

            closeProject();
            String msg = null;

            if (!_svProject.openProject(path, name))
            {
                _svProject.createProject(path, name);
            }

            _stationTreeView.initTree();
            _stationTreeView.proNode().Text = name;
            //初始化站节点
            loadAllPageData();

            //加载最近打开文件
            initRecentMenuItem();

            msg = String.Format("打开工程: [{0}]", SVProData.FullProPath);
            SVLog.WinLog.Info(msg);
        }

        /// <summary>
        /// 关闭工程
        /// </summary>
        void closeProject()
        {
            if (_stationTreeView.Nodes.Count == 0)
                return;

            savePro();
            this._propertyGrid.SelectedObject = null;
            this._stationTreeView.Nodes.Clear();

            //关闭工作区的所有窗口
            foreach (var item in _dockPanel.Documents)
            {
                SVControlWindow w = (SVControlWindow)item;
                w.Close();
            }

            String msg = String.Format("关闭工程: [{0}]", SVProData.FullProPath);
            SVLog.WinLog.Info(msg);
        }

        /// <summary>
        /// 保存当前工程数据
        /// </summary>
        void savePro()
        {
            SVConfig.instance().saveConfig();
            _stationTreeView.saveAllPageData();
            _svProject.saveProject();
        }

        /// <summary>
        /// 页面检查操作
        /// </summary>
        void checkPageValid()
        {
            String str = String.Format("\r\n-------- 已启动编译检查-------------");
            SVLog.WinLog.Info(str);

            try
            {
                SVCheckBeforeBuild check = new SVCheckBeforeBuild();
                check.checkAll();
            }
            catch (SVCheckValidException ex)
            {
                SVLog.WinLog.Info(ex.Message);
                _outPutWindow.Activate();
                return;
            }

            SVLog.WinLog.Info("页面检查成功");
        }

        private void 属性窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _propertyWindow.Show();
        }

        private void 对象窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _objectWindow.Show();
        }

        private void 输出窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _outPutWindow.Show();
        }

        private void 查找窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _findWindow.Show();
        }

        private void 工程管理窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _projectWindow.Show();
        }

        private void 控件窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ctlListViewWindow.Show();
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            savePro();
        }

        private void saveAllMenuItem_Click(object sender, EventArgs e)
        {
            savePro();
        }

        private void 仿真ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ///检查文件是否存在，确保仿真读取的文件正确
            String file = Path.Combine(SVProData.DownLoadFile, @"svducfg.bin");            
            if (!File.Exists(file))
            {
                MessageBox.Show(String.Format("{0}文件不存在或者目录不正确!", file), "提示");
                return;
            }

            SVSimulationWindow win = new SVSimulationWindow();
            win.load(file);
            win.Show();
        }

        private void openProMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "工程文件|*.svduproj";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String file = openFileDialog.FileName;
                String path = Path.GetDirectoryName(file);
                String proPath = Directory.GetParent(path).FullName;
                String proName = Path.GetFileNameWithoutExtension(file);
                openProject(proPath, proName);
            }
        }

        private void closeProMenuItem_Click(object sender, EventArgs e)
        {
            closeProject();
        }

        private void newPageMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void importMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = _stationTreeView.SelectedNode;
            if (node == null)
                return;

            ///如果不是分类节点，就返回
            if (node.Level != 1)
                return;

            importPage(node);
        }

        private void removeMenuItem_Click(object sender, EventArgs e)
        {
            SVPageNode node = _stationTreeView.SelectedNode as SVPageNode;
            if (node == null)
                return;

            ///执行移除
            removePage(node);
        }

        private void delMenuItem_Click(object sender, EventArgs e)
        {
            SVPageNode node = _stationTreeView.SelectedNode as SVPageNode;
            if (node == null)
                return;

            delPage(node);
        }

        private void closeMenuItem_Click(object sender, EventArgs e)
        {
            SVPageNode node = _stationTreeView.SelectedNode as SVPageNode;
            if (node == null)
                return;

            closePage((SVPageWidget)node.Addtionobj);
        }

        private void 页面检查ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkPageValid();
        }

        private void leftAlignMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.leftAlign();
        }

        private void rightAlignMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.rightAlign();
        }

        private void topAlignMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.topAlign();
        }

        private void bottomAlignMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.bottomAlign();
        }

        private void vCenterMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.vCenterAlign();
        }

        private void hCenterMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.hCenterAlign();
        }

        private void vEqualMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.vEqual();
        }

        private void hEqualMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.hEqual();
        }

        private void copyMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.copyOperator();
        }

        private void cutMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.cutOperator();
        }

        private void pasteMenuItem_Click(object sender, EventArgs e)
        {
            SVSelectPanelObjs.pasteOperator();
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //执行撤销
            SVPageWidget widget = currentPageWidget();
            if (widget != null)
                widget.RedoUndo.Undo();            
        }

        private void 恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //执行恢复
            SVPageWidget widget = currentPageWidget();
            if (widget != null)
                widget.RedoUndo.Redo();      
        }

        private void selectAllMenuItem_Click(object sender, EventArgs e)
        {
            SVPageWidget widget = currentPageWidget();
            if (widget != null)
                widget.selectAll(true);
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVPageWidget pageWidget = currentPageWidget();
            if (pageWidget == null)
                return;

            Bitmap ctlbitmap = new Bitmap(pageWidget.Width, pageWidget.Height);
            pageWidget.DrawToBitmap(ctlbitmap, pageWidget.ClientRectangle);

            //打印
            SVPrinter printPixmap = new SVPrinter();
            printPixmap.printBmp(ctlbitmap);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Delete:
                    SVSelectPanelObjs.removeOperator();
                    break;
                case Keys.Up:
                    SVSelectPanelObjs.up();
                    break;
                case Keys.Down:
                    SVSelectPanelObjs.down();
                    break;
                case Keys.Left:
                    SVSelectPanelObjs.left();
                    break;
                case Keys.Right:
                    SVSelectPanelObjs.right();
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            switch (m.Msg)
            {
                case 256:
                    SVSelectPanelObjs._VK_Ctrl = true;
                    return true;
                case 257:
                    SVSelectPanelObjs._VK_Ctrl = false;
                    return true;
            }

            return base.ProcessKeyPreview(ref m);
        }

        /// <summary>
        /// 编译操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 编译ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SVCheckBeforeBuild check = new SVCheckBeforeBuild();
                check.checkAll();
                buildDownLoadFiles();
            }
            catch (SVCheckValidException ex)
            {
                SVLog.WinLog.Info(ex.Message);
                _outPutWindow.Activate();
                return;
            }
        }

        /// <summary>
        /// 新建页面,目前有两种方式新建页面
        /// 1 - 以模板方式
        /// 2 - 以空白页面方式
        /// </summary>
        /// <param name="node">当前页面的节点对象</param>
        void createPage(TreeNode node)
        {
            String msg = null;

            ///打开新建页面对话框
            SVCreatePageForm form = new SVCreatePageForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                SVPageWidget widget = null;

                ///页面存在就退出
                if (_svProject.isExist(form.PageName))
                {
                    msg = String.Format("需要创建的页面 {0} 已经存在!", form.PageName);
                    SVLog.WinLog.Info(msg);

                    SVMessageBox msgBox = new SVMessageBox();
                    msgBox.content(" ", Resource.页面已经存在);
                    msgBox.Show();
                    return;
                }

                ///页面分类
                _svProject.addPageNode(node.Text, form.PageName);

                //如果为模板
                if (form.IsTempate)
                {
                    String destFile = _svProject.name2Path(form.PageName);
                    String srcFile = Path.Combine(form.TemplatePath, form.TemplateName);

                    //从模板中将文件拷贝到当前目录
                    File.Copy(srcFile, destFile, true);

                    widget = new SVPageWidget(form.PageName, destFile);
                    widget.loadSelf(true);
                }
                else
                {
                    ///创建页面
                    String pageFile = _svProject.name2Path(form.PageName);
                    widget = new SVPageWidget(form.PageName, pageFile);
                    widget.createID();
                }

                ///新建页面
                SVPageNode pageNode = newPageFromWidget(widget);
                widget.ClassText = node.Text;
                node.Nodes.Add(pageNode);
                _stationTreeView.ExpandAll();

                ///记录日志
                msg = String.Format("页[{0}] 创建成功!", form.PageName);
                SVLog.WinLog.Info(msg);
            }
        }

        /// <summary>
        /// 从指定目录中导入页面文件具体操作
        /// </summary>
        /// <param name="node">要添加的页面节点对象</param>
        void importPage(TreeNode node)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "页面文件|*.page";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                String file = openFileDialog.FileName;
                String pageName = Path.GetFileNameWithoutExtension(file);

                if (!_svProject.addPageNode(node.Text, pageName))
                    return;

                SVPageWidget widget = new SVPageWidget(pageName, file);
                widget.ClassText = node.Text;
                widget.loadSelf(true);

                SVPageNode pageNode = newPageFromWidget(widget);
                node.Nodes.Add(pageNode);
                _stationTreeView.ExpandAll();

                String msg = String.Format("页面文件{0} 导入成功!", file);
                SVLog.WinLog.Info(msg);
            }
        }

        /// <summary>
        /// 将当前页面保存为模板
        /// </summary>
        /// <param name="backPageWidget">页面窗口</param>
        void exportToTemplate(SVPageWidget widget)
        {
            SVExportTemplateForm form = new SVExportTemplateForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                //保存文件
                SVXml pageXML = new SVXml();
                pageXML.createRootEle("Page");
                widget.saveXML(pageXML);
                pageXML.writeXml(form.TemplateFile);

                //保存缩略图
                Bitmap ctlbitmap = new Bitmap(widget.Width, widget.Height);
                widget.DrawToBitmap(ctlbitmap, widget.ClientRectangle);
                ctlbitmap.Save(form.TemplateFile + ".jpg");

                //记录日志
                SVLog.WinLog.Info(String.Format("模板{0}保存成功", form.TemplateFile));

                SVMessageBox msgBox = new SVMessageBox();
                msgBox.content(Resource.提示, Resource.模板保存成功);
                msgBox.Show();
            }
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVSettingWindow setWin = new SVSettingWindow();
            setWin.ShowDialog();
        }

        private void 图元管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVBitmapManagerWindow win = new SVBitmapManagerWindow();
            win.ShowDialog();
        }

        private void findMenuItem_Click(object sender, EventArgs e)
        {
            SVFindWindow win = new SVFindWindow();
            if (win.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                //激活查找窗口
                _findWindow.Activate();
            }
        }

        /// <summary>
        /// 生成下装文件数据
        /// </summary>
        private void buildDownLoadFiles()
        {
            ///这里是固定下装文件的名称
            String downLoadName = @"svducfg.bin";

            String file = Path.Combine(SVProData.DownLoadFile, downLoadName);

            //启动页面
            SVPageWidget fristWidget = SVPageWidget.MainPageWidget;

            //序列化图片数据
            SVSerialize picBuffer = new SVSerialize();
            //页面数据
            PageArrayBin pageArrayBin = new PageArrayBin();

            //启动项
            fristWidget.buildControlToBin(ref pageArrayBin, ref picBuffer);
            foreach (var item in SVGlobalData.PageContainer)
            {
                if (fristWidget.Equals(item.Value))
                    continue;

                item.Value.buildControlToBin(ref pageArrayBin, ref picBuffer);
            }

            //将当前数据转换为下装数据
            byte[] result = null;
            SVBinData.structToByteArray(pageArrayBin, ref result);

            //计算当前还空多少个页面
            int size = (SVLimit.MAX_PAGE_NUMBER - (Int32)pageArrayBin.pageCount) * Marshal.SizeOf(typeof(PageBin));
            size = Marshal.SizeOf(typeof(PageArrayBin)) - size;

            ///保存当前生成的下装文件
            SVBuildFile buildFile = new SVBuildFile(file);
            byte[] resultAll = new byte[size + picBuffer.Length];
            buildFile.setImageOffsetAndLen((UInt32)size, (UInt32)picBuffer.Length);
            Array.Copy(result, resultAll, size);
            Array.Copy(picBuffer.ToArray(), 0, resultAll, size, picBuffer.Length);
            buildFile.setDataByteArray(resultAll);
            buildFile.write();

            //提示生成成功信息
            String outMsg = String.Format("在指定目录生成文件:\r\n\t 文件路径: {0}.", file);
            SVLog.WinLog.Info(outMsg);
        }

        /// <summary>
        /// 执行变量名管理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 变量名管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVVarWindow win = new SVVarWindow();
            win.ShowDialog();
        }

        /// <summary>
        /// 执行模板管理事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 模板管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVTemplateWindow win = new SVTemplateWindow();
            win.ShowDialog();
        }

        /// <summary>
        /// 执行中文环境语言切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVConfig currInstance = SVConfig.instance();
            currInstance.Language = "zh";
            currInstance.saveConfig();

            SVMessageBox msgBox = new SVMessageBox();
            msgBox.content(_resources.GetString("提示"), _resources.GetString("重启提示"));
            DialogResult result = msgBox.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Application.ExitThread();
                Process.Start(Application.ExecutablePath, "-p " + SVProData.FullProPath);
            }
        }

        /// <summary>
        /// 执行英文环境语言切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 英文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVConfig currInstance = SVConfig.instance();
            currInstance.Language = "en";
            currInstance.saveConfig();

            SVMessageBox msgBox = new SVMessageBox();
            msgBox.content(_resources.GetString("提示"), _resources.GetString("重启提示"));
            DialogResult result = msgBox.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Application.ExitThread();
                Process.Start(Application.ExecutablePath, "-p " + SVProData.FullProPath);
            }
        }

        /// <summary>
        /// 打开关于对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVAboutWindow win = new SVAboutWindow();
            win.ShowDialog();
        }

        /// <summary>
        /// 打开帮助文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 主题帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String helpFile = @"Help.chm";
            if (!File.Exists(helpFile))
            {
                SVMessageBox messageBox = new SVMessageBox();
                messageBox.content(Resource.提示, Resource.帮助提示);
                messageBox.Show();
                return ;
            }

            Help.ShowHelp(this, helpFile);
        }
    }
}
