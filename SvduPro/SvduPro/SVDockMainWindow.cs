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
using System.Threading;

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
            try
            {
                commandLine(args);
            }
            catch (ArgumentException)
            {
                SVMessageBox msgBox = new SVMessageBox();
                msgBox.content(" ", "命令行打开参数错误!");
                msgBox.ShowDialog();
                Environment.Exit(-1);
            }
            finally
            {
                ///获取屏幕分辨率，并设置界面宽和高
                Rectangle ScreenArea = Screen.GetWorkingArea(this);
                this.Location = ScreenArea.Location;
                this.Width = ScreenArea.Width;
                this.Height = ScreenArea.Height;
            }
        }

        /// <summary>
        /// 处理命令行参数命令
        /// 
        /// 1、不带任何参数，执行打开软件，不打开任何工程
        /// 2、-e projectFile -s stationNum -u userName -p passwd -ip databaseIP 打开指定的工程
        /// 3、-c projectFile -s stationNum -u userName -p passwd -ip databaseIP 编译指定的工程文件
        /// 4、-n projectName -s stationNum -u userName -p passwd -ip databaseIP 在当前目录创建工程，如果已经创建就打开
        /// </summary>
        /// <param Name="args">输入参数</param>
        void commandLine(String[] args)
        {
            if (args.Length == 0)
                return;

            ///工程文件路径
            String proFile = null;
            ///当前的操作模式，
            /// 0 表示没有输入任何参数
            /// 1 表示当前为新建工程
            /// 2 表示打开当前工程
            /// 3 表示编译当前工程
            Int32 operMode = 0;

            Dictionary<String, Action<String>> dictAction = new Dictionary<String, Action<String>>();
            dictAction.Add("-e", (str) => 
            {
                if (String.IsNullOrWhiteSpace(str))
                    throw new ArgumentException();

                operMode = 2;
                proFile = str;
            });

            dictAction.Add("-c", (str) =>
            {
                if (String.IsNullOrWhiteSpace(str))
                    throw new ArgumentException();

                operMode = 3;
                proFile = str;
            });

            dictAction.Add("-n", (str) =>
            {
                if (String.IsNullOrWhiteSpace(str))
                    throw new ArgumentException();

                operMode = 1;
                proFile = str;
            });

            dictAction.Add("-s", (str) =>
            {
                Int32 outValue = 0;
                if (Int32.TryParse(str, out outValue))
                    SVProData.stationID = outValue;
                else
                    throw new ArgumentException();
            });

            dictAction.Add("-u", (str) =>
            {
                if (String.IsNullOrWhiteSpace(str))
                    throw new ArgumentException();

                SVProData.user = str;
            });

            dictAction.Add("-p", (str) =>
            {
                if (String.IsNullOrWhiteSpace(str))
                    throw new ArgumentException();

                SVProData.passwd = str;
            });

            dictAction.Add("-ip", (str) =>
            {
                if (String.IsNullOrWhiteSpace(str))
                    throw new ArgumentException();

                SVProData.dbIp = str;
            });

            ///循环遍历输入的每一个参数
            for (int i = 0; i < args.Length; i += 2)
            {
                if (!dictAction.ContainsKey(args[i]))
                    throw new ArgumentException();

                try
                {
                    dictAction[args[i]](args[i + 1]);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new ArgumentException();
                }
            }

            switch (operMode)
            {
                case 1:
                    {
                        openProject(".", proFile);
                        break;
                    }
                case 2:
                    {
                        String path = Path.GetDirectoryName(proFile);
                        String proPath = Directory.GetParent(path).FullName;
                        String proName = Path.GetFileNameWithoutExtension(proFile);
                        openProject(proPath, proName);
                        break;
                    }
                case 3:
                    {
                        String path = Path.GetDirectoryName(proFile);
                        String proPath = Directory.GetParent(path).FullName;
                        String proName = Path.GetFileNameWithoutExtension(proFile);
                        openProject(proPath, proName);

                        try
                        {
                            SVCheckBeforeBuild check = new SVCheckBeforeBuild();
                            check.checkAll();
                            buildDownLoadFiles(new SVWPFProgressBar());
                            Environment.Exit(0);
                            return;
                        }
                        catch (SVCheckValidException ex)
                        {
                            SVLog.WinLog.Info(ex.Message);
                            _outPutWindow.Activate();
                        }

                        break;
                    }
            }
        }

        /// <summary>
        /// 如果程序出现崩溃现象，将保存工程并退出
        /// </summary>
        /// <param oldName="ex"></param>
        public void captureExceptionAndSaveProject(Object ex)
        {
            Exception baseException = ex as Exception;

            ///记录日志
            if (baseException != null)
                SVLog.TextLog.Exception(baseException);

            ///保存工程
            savePro();

            ///提示用户
            MessageBox.Show("程序出现异常，工程已保存。确认退出!");
            Application.ExitThread();
        }

        /// <summary>
        /// 注册查找窗口事件
        /// </summary>
        void initFindWindowEvent()
        {
            _findText.MouseDoubleClick += new MouseEventHandler((sender, e) =>
            {
                var obj = _findText.getMarkObject();
                if (obj is SVPageWidget)
                {
                    openPage((SVPageWidget)obj);
                }
                else
                {
                    SVPanel panel = obj as SVPanel;
                    if (panel == null)
                        return;

                    SVPageWidget pageWidget = panel.Parent as SVPageWidget;
                    if (pageWidget == null)
                        return;

                    ///双击事件打开对应页面
                    if (_svProject.isExist(pageWidget.PageName))
                    {
                        openPage(pageWidget);
                        panel.Selected = true;
                    }
                }
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
            System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
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
        /// <param Name="backPageWidget">页面窗口对象</param>
        void addPageToWorkSpace(SVPageWidget widget)
        {
            SVControlWindow win = widget.Parent as SVControlWindow;
            if (win != null)
            {
                ///如果已经打开就执行激活
                win.Activate();
                return;
            }

            widget.Parent = this;
            SVControlWindow workWindow = new SVControlWindow(widget);
            widget.Dock = DockStyle.None;
            workWindow.Text = widget.PageName;
            Bitmap image = (Bitmap)Resource.page;
            workWindow.ShowIcon = true;
            workWindow.Icon = Icon.FromHandle(image.GetHicon());
            workWindow.ToolTipText = widget.pageFileName;
            workWindow.Show(_dockPanel, DockState.Document);
            workWindow.Activate();

            workWindow.HideEventer += new EventHandler((sender, e) =>
            {
                var pageWidget = workWindow.CoreControl as SVPageWidget;

                if (pageWidget != null)
                {
                    if (pageWidget.IsModify)
                    {
                        SVMessageBox msgBox = new SVMessageBox();
                        msgBox.content("提示", String.Format("当前页面未保存"));
                        msgBox.ShowDialog();
                        return;
                    }

                    _propertyGrid.SelectedObject = null;
                    this._objTreeView.clearAllNodes();
                    workWindow.Hide();
                }
            });
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

            showProjectWindow();

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
        /// 显示工程窗口
        /// </summary>
        public void showProjectWindow()
        {
            if (_projectWindow == null)
            {
                _projectWindow = new SVControlWindow(_stationTreeView);
                _projectWindow.Text = _resources.GetString("工程窗口");
                _projectWindow.Show(_dockPanel, DockState.DockLeft);
                _projectWindow.Activate();
            }
            else
            {
                _stationTreeView.ExpandAll();
                _projectWindow.setControl(_stationTreeView);
            }
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
            _dockPanel.ActivePaneChanged += new EventHandler((sender, e)=>
            {
                SVControlWindow win = _dockPanel.ActiveContent as SVControlWindow;
                if ((_projectWindow == win)
                    || (win == _ctlListViewWindow)
                    || (win == _propertyWindow)
                    || (win == _ctlListViewWindow)
                    || (win == _outPutWindow)
                    || (win == _findWindow)
                    || (win == _objectWindow))
                    return;
                
                //SVSelectPanelObjs.clearSelectControls();
            });
            
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
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void _dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            SVSelectPanelObjs.clearSelectControls();

            SVControlWindow win = currentControlWindow();
            if (win == null)
                return;

            ///如果是页面窗口
            var pageWidget = win.CoreControl as SVPageWidget;
            if (pageWidget != null)
            {
                _propertyGrid.SelectedObject = pageWidget;
                //添加对象窗口中的内容
                this._objTreeView.setPageWidget(pageWidget);
            }
        }


        /// <summary>
        /// 工程节点注册事件
        /// 
        /// 执行双击节点打开对应页面
        /// </summary>
        void initTreeViewOperation()
        {
            initStationMenu();

            this._stationTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler((sender, e) => 
            {
                SVProjectProperty tmpProperty = new SVProjectProperty();
                tmpProperty.StationID = SVProData.stationID.ToString();
                tmpProperty.StationName = SVProData.ProName;
                tmpProperty.DataBaseIP = SVProData.dbIp;
                tmpProperty.DataBaseName = SVProData.user;
                tmpProperty.StationPath = SVProData.ProPath;

                this._propertyGrid.SelectedObject = tmpProperty;
            });
            //鼠标单击事件后操作
            this._stationTreeView.AfterSelect += new TreeViewEventHandler(_stationTreeView_AfterSelect);
            //双击节点
            _stationTreeView.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(_stationTreeView_NodeMouseDoubleClick);
        }

        /// <summary>
        /// 双击工程树节点，打开页面的事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void _stationTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SVPageNode node = _stationTreeView.SelectedNode as SVPageNode;
            if (node == null)
                return;

            SVPageWidget widget = node.Addtionobj as SVPageWidget;
            if (widget == null)
                return;

            this._objTreeView.setPageWidget(widget);
            //打开页面
            openPage(widget);
        }

        /// <summary>
        /// 工程节点单击后的事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void _stationTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode currNode = _stationTreeView.SelectedNode;
            if (currNode.Level == 0)
            {
                toolStripButton4.Enabled = false;
                toolStripSeparator11.Enabled = false;
                toolStripButton7.Enabled = false;
                toolStripButton8.Enabled = false;
                toolStripButton19.Enabled = false;
                toolStripButton20.Enabled = false;
                toolStripButton21.Enabled = false;
                toolStripButton22.Enabled = false;
                toolStripButton23.Enabled = false;
                toolStripButton12.Enabled = false;
                toolStripSeparator12.Enabled = false;
                toolStripButton13.Enabled = false;
                toolStripButton14.Enabled = false;
                toolStripButton15.Enabled = false;
                toolStripButton16.Enabled = false;
                toolStripButton17.Enabled = false;
                toolStripButton18.Enabled = false;

                newPageMenuItem.Enabled = false;
                importMenuItem.Enabled = false;
                removeMenuItem.Enabled = false;
                delMenuItem.Enabled = false;
                closeMenuItem.Enabled = false;

                copyMenuItem.Enabled = false;
                cutMenuItem.Enabled = false;
                leftAlignMenuItem.Enabled = false;
                rightAlignMenuItem.Enabled = false;
                topAlignMenuItem.Enabled = false;
                bottomAlignMenuItem.Enabled = false;
                vCenterMenuItem.Enabled = false;
                hCenterMenuItem.Enabled = false;
                vEqualMenuItem.Enabled = false;
                hEqualMenuItem.Enabled = false;
            }
            if (currNode.Level == 1)
            {
                toolStripButton4.Enabled = true;
                toolStripSeparator11.Enabled = true;
                toolStripButton7.Enabled = false;
                toolStripButton8.Enabled = false;
                toolStripButton19.Enabled = false;
                toolStripButton20.Enabled = false;
                toolStripButton21.Enabled = false;
                toolStripButton22.Enabled = false;
                toolStripButton23.Enabled = false;
                toolStripButton12.Enabled = false;
                toolStripSeparator12.Enabled = false;
                toolStripButton13.Enabled = false;
                toolStripButton14.Enabled = false;
                toolStripButton15.Enabled = false;
                toolStripButton16.Enabled = false;
                toolStripButton17.Enabled = false;
                toolStripButton18.Enabled = false;

                newPageMenuItem.Enabled = true;
                importMenuItem.Enabled = true;
                removeMenuItem.Enabled = false;
                delMenuItem.Enabled = false;
                closeMenuItem.Enabled = false;

                copyMenuItem.Enabled = false;
                cutMenuItem.Enabled = false;
                leftAlignMenuItem.Enabled = false;
                rightAlignMenuItem.Enabled = false;
                topAlignMenuItem.Enabled = false;
                bottomAlignMenuItem.Enabled = false;
                vCenterMenuItem.Enabled = false;
                hCenterMenuItem.Enabled = false;
                vEqualMenuItem.Enabled = false;
                hEqualMenuItem.Enabled = false;
            }
            if (currNode.Level == 2)
            {
                toolStripButton4.Enabled = false;
                toolStripSeparator11.Enabled = false;
                toolStripButton7.Enabled = true;
                toolStripButton8.Enabled = true;
                toolStripButton19.Enabled = false;
                toolStripButton20.Enabled = false;
                toolStripButton21.Enabled = false;
                toolStripButton22.Enabled = false;
                toolStripButton23.Enabled = false;
                toolStripButton12.Enabled = false;
                toolStripSeparator12.Enabled = false;
                toolStripButton13.Enabled = false;
                toolStripButton14.Enabled = false;
                toolStripButton15.Enabled = false;
                toolStripButton16.Enabled = false;
                toolStripButton17.Enabled = false;
                toolStripButton18.Enabled = false;

                newPageMenuItem.Enabled = false;
                importMenuItem.Enabled = false;
                removeMenuItem.Enabled = true;
                delMenuItem.Enabled = true;
                closeMenuItem.Enabled = true;

                copyMenuItem.Enabled = false;
                cutMenuItem.Enabled = false;
                leftAlignMenuItem.Enabled = false;
                rightAlignMenuItem.Enabled = false;
                topAlignMenuItem.Enabled = false;
                bottomAlignMenuItem.Enabled = false;
                vCenterMenuItem.Enabled = false;
                hCenterMenuItem.Enabled = false;
                vEqualMenuItem.Enabled = false;
                hEqualMenuItem.Enabled = false;
            }
        }

        /// <summary>
        /// 获取当前激活的页面窗口对象
        /// </summary>
        /// <returns>获取当前激活的页面窗口对象</returns>
        SVControlWindow currentControlWindow()
        {
            SVControlWindow controlWindow = _dockPanel.ActiveContent as SVControlWindow;
            if (controlWindow == null)
                return null;

            return controlWindow;
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

                if (v.Count == 0)
                {
                    toolStripButton4.Enabled = false;
                    toolStripSeparator11.Enabled = false;
                    toolStripButton7.Enabled = false;
                    toolStripButton8.Enabled = false;
                    toolStripButton19.Enabled = false;
                    toolStripButton20.Enabled = false;
                    toolStripButton21.Enabled = false;
                    toolStripButton22.Enabled = true;
                    toolStripButton23.Enabled = true;
                    toolStripButton12.Enabled = false;
                    toolStripSeparator12.Enabled = false;
                    toolStripButton13.Enabled = false;
                    toolStripButton14.Enabled = false;
                    toolStripButton15.Enabled = false;
                    toolStripButton16.Enabled = false;
                    toolStripButton17.Enabled = false;
                    toolStripButton18.Enabled = false;

                    copyMenuItem.Enabled = false;
                    cutMenuItem.Enabled = false;
                    leftAlignMenuItem.Enabled = false;
                    rightAlignMenuItem.Enabled = false;
                    topAlignMenuItem.Enabled = false;
                    bottomAlignMenuItem.Enabled = false;
                    vCenterMenuItem.Enabled = false;
                    hCenterMenuItem.Enabled = false;
                    vEqualMenuItem.Enabled = false;
                    hEqualMenuItem.Enabled = false;
                }

                if (v.Count > 0)
                {
                    toolStripButton4.Enabled = false;
                    toolStripSeparator11.Enabled = false;
                    toolStripButton7.Enabled = false;
                    toolStripButton8.Enabled = false;
                    toolStripButton19.Enabled = true;
                    toolStripButton20.Enabled = true;
                    toolStripButton21.Enabled = false;
                    toolStripButton22.Enabled = true;
                    toolStripButton23.Enabled = true;
                    toolStripButton12.Enabled = false;
                    toolStripSeparator12.Enabled = false;
                    toolStripButton13.Enabled = false;
                    toolStripButton14.Enabled = false;
                    toolStripButton15.Enabled = false;
                    toolStripButton16.Enabled = false;
                    toolStripButton17.Enabled = false;
                    toolStripButton18.Enabled = false;

                    copyMenuItem.Enabled = true;
                    cutMenuItem.Enabled = true;
                    leftAlignMenuItem.Enabled = false;
                    rightAlignMenuItem.Enabled = false;
                    topAlignMenuItem.Enabled = false;
                    bottomAlignMenuItem.Enabled = false;
                    vCenterMenuItem.Enabled = false;
                    hCenterMenuItem.Enabled = false;
                    vEqualMenuItem.Enabled = false;
                    hEqualMenuItem.Enabled = false;
                }
                if (v.Count > 1)
                {
                    toolStripButton4.Enabled = false;
                    toolStripSeparator11.Enabled = false;
                    toolStripButton7.Enabled = false;
                    toolStripButton8.Enabled = false;
                    toolStripButton19.Enabled = true;
                    toolStripButton20.Enabled = true;
                    toolStripButton21.Enabled = false;
                    toolStripButton22.Enabled = true;
                    toolStripButton23.Enabled = true;
                    toolStripButton12.Enabled = true;
                    toolStripSeparator12.Enabled = true;
                    toolStripButton13.Enabled = true;
                    toolStripButton14.Enabled = true;
                    toolStripButton15.Enabled = true;
                    toolStripButton16.Enabled = true;
                    toolStripButton17.Enabled = true;
                    toolStripButton18.Enabled = true;

                    copyMenuItem.Enabled = true;
                    cutMenuItem.Enabled = true;
                    leftAlignMenuItem.Enabled = true;
                    rightAlignMenuItem.Enabled = true;
                    topAlignMenuItem.Enabled = true;
                    bottomAlignMenuItem.Enabled = true;
                    vCenterMenuItem.Enabled = true;
                    hCenterMenuItem.Enabled = true;
                    vEqualMenuItem.Enabled = true;
                    hEqualMenuItem.Enabled = true;
                }
            });

            //执行粘贴操作
            SVSelectPanelObjs.PasteEvent = ((c) =>
            {
                sortControlList(c);

                var window = currentControlWindow();
                if (window == null)
                    return;

                SVPageWidget widget = window.CoreControl as SVPageWidget;
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
                    this._propertyGrid.Refresh();
                }
            });
        }

        /// <summary>
        /// 检查当前要被粘贴的对象队列是否在粘贴点超出页面范围
        /// 
        /// 返回：true表示在页面范围内
        ///       false超出页面显示范围
        /// </summary>
        /// <param Name="vList">粘贴控件对象队列</param>
        /// <param Name="backPageWidget">页面窗口对象</param>
        /// <param Name="point">要检查的粘贴点</param>
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
        /// <param Name="vList">执行粘贴操作的控件队列</param>
        /// <param Name="point">页面中要粘贴的位置点</param>
        void paste(List<Control> vList, Point point)
        {
            if (vList.Count == 0)
                return;

            var window = currentControlWindow();
            if (window == null)
                return;

            SVPageWidget widget = window.CoreControl as SVPageWidget;
            if (widget == null)
                return;

            Control first = vList[0];
            Int32 disx = point.X - first.Location.X;
            Int32 disy = point.Y - first.Location.Y;

            widget.selectAll(false);

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

                ///设置相对位置
                svPanel.Location = new Point(svPanel.Location.X + disx, svPanel.Location.Y + disy);
                svPanel.setStartPos(svPanel.Location);

                ///添加控件到页面中
                widget.Controls.Add(svPanel);
                ///新建当前控件ID号
                svPanel.newID();
                svPanel.refreshPropertyToPanel();
                ///将控件显示位置置顶
                svPanel.BringToFront();
                svPanel.Selected = true;

                SVRedoUndoItem recordItem = new SVRedoUndoItem();
                recordItem.ReDo = () =>
                {
                    widget.RedoUndo.setEnabled(false);
                    widget.Controls.Add(svPanel);
                    widget.RedoUndo.setEnabled(true);
                };
                recordItem.UnDo = () =>
                {
                    widget.RedoUndo.setEnabled(false);
                    svPanel.Selected = false;
                    widget.Controls.Remove(svPanel);
                    widget.RedoUndo.setEnabled(true);
                };
                widget.RedoUndo.recordOper(recordItem);
            }
        }

        /// <summary>
        /// 对当前控件队列进行排序
        /// </summary>
        /// <param Name="vList">作为输入和输出，输入控件队列。输出排序后的队列</param>
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
        /// <param Name="panel">当前页面控件对象</param>
        /// <returns>返回新创建的页面节点</returns>
        SVPageNode newPageFromWidget(SVPageWidget widget)
        {
            SVPageNode pageNode = new SVPageNode(widget.PageName);

            ///如果编辑后的节点为当前页面节点,就进行重命名
            _stationTreeView.AfterLabelEdit += new NodeLabelEditEventHandler((sender, e)=>
            {
                if (pageNode.Equals(e.Node))
                {
                    ///名字不合法创建不成功
                    if (String.IsNullOrWhiteSpace(e.Node.Text) ||
                        String.IsNullOrWhiteSpace(e.Label))
                    {
                        e.CancelEdit = true;
                        return;
                    }

                    String label = e.Label.Trim();

                    ///名字已经存在
                    if (_svProject.isExist(label))
                    {
                        e.CancelEdit = true;
                        return;
                    }

                    _svProject.renamePageName(pageNode.Parent.Text, e.Node.Text, label);
                    widget.PageName = e.Label;
                }
            });

            ///设置节点的图标
            pageNode.ImageKey = "Page";
            pageNode.SelectedImageKey = "Page";
            ///记录当前节点对应页面控件
            pageNode.Addtionobj = widget;

            SVGlobalData.addPage(widget.PageName, widget);

            ///定义一个刷新对象窗口的临时函数
            EventHandler refreshObjWindow = (sender, e) => 
            {
                this.undoStatusLabel.Text = "";
                this._propertyGrid.SelectedObject = widget;
                this._propertyGrid.Refresh();
                this._objTreeView.setPageWidget(widget);
            };

            ///添加控件修改对象窗口
            widget.ChildAddEvent += refreshObjWindow;
            ///删除控件修改对象窗口
            widget.ChildRemoveEvent += refreshObjWindow;
            widget.MouseDown += new MouseEventHandler((sender, e) =>
            {
                this._propertyGrid.SelectedObject = widget;
                if (widget == sender)
                    return;
                refreshObjWindow(sender, e);
            });
            ///刷新属性窗口
            widget.RedoUndo.UpdateOperator += new DoFunction(() =>
            {
                this._propertyGrid.Refresh();
            });

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
                //this._objTreeView.setPageWidget(panel);
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
        /// <param Name="className">分类名称</param>
        /// <returns>返回新的页面分类节点</returns>
        private TreeNode createPageClass(String className)
        {
            TreeNode pageTreeNode = _stationTreeView.newClassNode(className);
            _svProject.addPageNode(className, null);

            ///如果修改的是当前节点，就需要重新命名分类名称
            _stationTreeView.AfterLabelEdit += new NodeLabelEditEventHandler((sender, e)=>
            {
                if (pageTreeNode.Equals(e.Node))
                {
                    if (String.IsNullOrWhiteSpace(e.Label))
                    {
                        e.CancelEdit = true;
                        return;
                    }

                    String label = e.Label.Trim();

                    if (String.IsNullOrWhiteSpace(e.Node.Text)
                        || _svProject.getDictData().ContainsKey(label))
                    {
                        e.CancelEdit = true;
                        return;
                    }

                    _svProject.renamePageClassName(e.Node.Text, label);
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
                String text = String.Format("{0} 分类名称:{1}", Resource.确定删除分类, className);
                msgBox.content(Resource.提示, text);
                DialogResult result = msgBox.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.No)
                    return;

                List<TreeNode> tmpNode = new List<TreeNode>();
                foreach (TreeNode item in pageTreeNode.Nodes)
                    tmpNode.Add(item);

                ///遍历移除所有的页面中的控件元素
                foreach (var item in tmpNode)
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
        /// <param Name="pageNode">页面节点</param>
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
        /// <param Name="pageNode">页面节点</param>
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
                removePage(pageNode);
            }
        }

        /// <summary>
        /// 关闭页面
        /// </summary>
        /// <param Name="backPageWidget">页面节点</param>
        void closePage(SVPageWidget widget)
        {
            if (widget.IsModify)
            {
                SVMessageBox msgBox = new SVMessageBox();
                msgBox.content("提示", String.Format("当前页面未保存"));
                msgBox.ShowDialog();
                return;
            }

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
            _propertyGrid.SelectedObject = widget;
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
            SVPageWidget widget = node.Addtionobj as SVPageWidget;
            if (widget != null)
            {
                if (widget.IsModify)
                {
                    SVMessageBox msgBox = new SVMessageBox();
                    msgBox.content("提示", String.Format("当前页面未保存"));
                    msgBox.ShowDialog();
                    return;
                }

                SVControlWindow win = widget.Parent as SVControlWindow;
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
                if (classItem.Value == null)
                    continue;

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
        /// <param Name="path">工程路径</param>
        /// <param Name="Name">工程名</param>
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

            SVLog.WinLog.Info("当前工程数据保存成功！");
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

        /// <summary>
        /// 新建页面执行事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void newPageMenuItem_Click(object sender, EventArgs e)
        {
            ///如果没有选中树节点，就不进行任何操作
            TreeNode node = _stationTreeView.SelectedNode;
            if (node == null)
                return;

            ///如果不是分类节点，就返回
            if (node.Level != 1)
                return;

            createPage(node);
        }

        /// <summary>
        /// 导入页面执行事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
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
            var window = currentControlWindow();
            if (window == null)
                return;

            SVPageWidget pageWidget = window.CoreControl as SVPageWidget;
            if (pageWidget == null)
            {
                SVLog.WinLog.Warning("焦点不在页面窗口中!");
                return;
            }

            window.undoMethod();
            int nCount = pageWidget.RedoUndo.getUndoCount();
            this.statusLabel.Text = String.Format("剩余撤销次数:{0}", nCount);
        }

        private void 恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //执行撤销
            var window = currentControlWindow();
            if (window == null)
                return;

            SVPageWidget pageWidget = window.CoreControl as SVPageWidget;
            if (pageWidget == null)
            {
                SVLog.WinLog.Warning("焦点不在页面窗口中!");
                return;
            }

            window.redoMethod();
            int nCount = pageWidget.RedoUndo.getRedoCount();
            this.statusLabel.Text = String.Format("剩余重做次数:{0}", nCount);
        }

        /// <summary>
        /// 执行当前页面的全选
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void selectAllMenuItem_Click(object sender, EventArgs e)
        {
            SVControlWindow win = currentControlWindow();
            if (win != null)
                win.selectAllMethod();
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var window = currentControlWindow();
            if (window == null)
                return;

            SVPageWidget pageWidget = window.CoreControl as SVPageWidget;
            if (pageWidget == null)
                return;

            Bitmap ctlbitmap = new Bitmap(pageWidget.Width, pageWidget.Height);
            pageWidget.DrawToBitmap(ctlbitmap, pageWidget.ClientRectangle);

            //打印
            SVPrinter printPixmap = new SVPrinter();
            printPixmap.printBmp(ctlbitmap);
        }

        /// <summary>
        /// 处理Ctrl的按下和弹起状态
        /// </summary>
        /// <param Name="m"></param>
        /// <returns></returns>
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
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void 编译ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SVCheckBeforeBuild check = new SVCheckBeforeBuild();
                check.checkAll();

                SVWPFProgressBar bar = new SVWPFProgressBar();

                Thread th = new Thread(() =>
                {
                    buildDownLoadFiles(bar);
                });
                th.IsBackground = true;
                th.Start();

                bar.ShowDialog();
            }
            catch (SVCheckValidException ex)
            {
                SVLog.WinLog.Info(ex.Message);
                _outPutWindow.Activate();
            }
        }

        /// <summary>
        /// 新建页面,目前有两种方式新建页面
        /// 1 - 以模板方式
        /// 2 - 以空白页面方式
        /// </summary>
        /// <param Name="node">当前页面的节点对象</param>
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
        /// <param Name="node">要添加的页面节点对象</param>
        void importPage(TreeNode node)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "页面文件|*.page";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog.FileNames)
                {
                    //String File = openFileDialog.FileName;

                    _svProject.importPageNode(node.Text, file);
                    String pageName = Path.GetFileNameWithoutExtension(file);

                    SVPageWidget widget = new SVPageWidget(pageName, file);
                    widget.ClassText = node.Text;
                    if (!widget.loadSelf(true))
                    {
                        String errorMsg = String.Format("页面文件{0} 导入失败!", file);
                        SVLog.WinLog.Info(errorMsg);
                        continue ;
                    }

                    SVPageNode pageNode = newPageFromWidget(widget);
                    node.Nodes.Add(pageNode);
                    _stationTreeView.ExpandAll();

                    String msg = String.Format("页面文件{0} 导入成功!", file);
                    SVLog.WinLog.Info(msg);
                }
            }
        }

        /// <summary>
        /// 将当前页面保存为模板
        /// </summary>
        /// <param Name="backPageWidget">页面窗口</param>
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
            SVConfig config = SVConfig.instance();

            SVWPFSettingWindow win = new SVWPFSettingWindow();
            win.DataContext = config;
            win.ShowDialog();
            //SVSettingWindow setWin = new SVSettingWindow();
            //setWin.ShowDialog();
        }

        private void 图元管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVWPFBitmapManagerWindow win = new SVWPFBitmapManagerWindow();
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
        private void buildDownLoadFiles(SVWPFProgressBar bar)
        {
            ///这里是固定下装文件的名称
            String downLoadName = @"svducfg.bin";
            ///这里加了协议头的文件
            String downLoadNameProtocol = @"psvducfg.bin";

            String file = Path.Combine(SVProData.DownLoadFile, downLoadName);
            String protocolFile = Path.Combine(SVProData.DownLoadFile, downLoadNameProtocol);

            //启动页面
            SVPageWidget fristWidget = SVPageWidget.MainPageWidget;
            SVPageWidget.clearBackGroundData();

            //序列化图片数据
            SVSerialize picBuffer = new SVSerialize();
            //页面数据
            PageArrayBin pageArrayBin = new PageArrayBin();

            //启动项
            Action log = () => { SVLog.WinLog.Info(String.Format("编译启动页面{0}成功", fristWidget.PageName)); };
            this.Invoke(log);

            Action barAc = () => { bar.setMaxBarValue(SVGlobalData.PageContainer.Count); };
            this.Invoke(barAc);
            
            Int32 currCount = 1;
            
            fristWidget.buildControlToBin(ref pageArrayBin, ref picBuffer);
            barAc = () => { bar.setText(String.Format("编译页面:{0}", fristWidget.PageName)); bar.setBarValue(currCount); };
            this.Invoke(barAc);
            
            foreach (var item in SVGlobalData.PageContainer)
            {
                if (fristWidget.Equals(item.Value))
                    continue;

                log = () => { SVLog.WinLog.Info(String.Format("编译页面{0}成功", item.Value.PageName)); };
                this.Invoke(log);
                //
                item.Value.buildControlToBin(ref pageArrayBin, ref picBuffer);

                barAc = () => 
                {
                    bar.setText(String.Format("编译页面:{0}", item.Value.PageName));
                    bar.setBarValue(currCount++); 
                };
                this.Invoke(barAc);
            }

            barAc = () =>
            {
                bar.Close();
            };
            this.Invoke(barAc);

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

            ///数据长度加上64字节的文件头，如果大于64M停止编译
            if ((resultAll.Length + 64) > 64 * 1024 * 1024)
            {
                SVLog.WinLog.Warning("下装文件生成失败，生成的文件大于64M");
                return;
            }

            buildFile.write();
            buildFile.writeProtocol(protocolFile);
            
            //提示生成成功信息
            log = () => 
            {
                String outMsg = String.Format("在指定目录生成文件:\r\n\t 文件路径: {0}.", file);
                SVLog.WinLog.Info(outMsg);
                //MessageBox.Show("编译完成");
            };
            this.Invoke(log);
        }

        /// <summary>
        /// 执行变量名管理事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void 变量名管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVVarWindow win = new SVVarWindow();
            win.ShowDialog();
        }

        /// <summary>
        /// 执行模板管理事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void 模板管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVTemplateWindow win = new SVTemplateWindow();
            win.ShowDialog();
        }

        /// <summary>
        /// 执行中文环境语言切换
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
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
                String proFileName = String.Format(@"-e {0} -s {1} -u {2} -p {3} -ip {4}",
                    SVProData.FullProPath, SVProData.stationID, SVProData.user, SVProData.passwd, SVProData.dbIp);
                Process.Start(Application.ExecutablePath, proFileName);
            }
        }

        /// <summary>
        /// 执行英文环境语言切换
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
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
                String proFileName = String.Format(@"-e {0} -s {1} -u {2} -p {3} -ip {4}", 
                    SVProData.FullProPath, SVProData.stationID, SVProData.user, SVProData.passwd, SVProData.dbIp);
                Process.Start(Application.ExecutablePath, proFileName);
            }
        }

        /// <summary>
        /// 打开关于对话框
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SVWPFAboutWindow win = new SVWPFAboutWindow();            
            win.ShowDialog();
            //SVAboutWindow win = new SVAboutWindow();
            //win.ShowDialog();
        }

        /// <summary>
        /// 打开帮助文档
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
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
