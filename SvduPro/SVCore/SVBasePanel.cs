using System;
using System.Drawing;
using System.Windows.Forms;

namespace SVCore
{
    /// <summary>
    /// 当前节点的显示类型
    /// </summary>
    public enum SVNodesType
    {
        /// <summary>
        /// 显示所有节点
        /// </summary>
        All,

        /// <summary>
        /// 横轴方向
        /// </summary>
        Horizontal,

        /// <summary>
        /// //纵轴方向
        /// </summary>
        Vertical     
    }

    /// <summary>
    /// 自定义基础控件
    /// </summary>
    public class SVBasePanel : Label
    {
        #region 类成员变量
        
        /// <summary>
        /// 当前控件的选中状态
        /// 选中 Selected为true
        /// 不选中 Selected为false
        /// 默认为不选中.
        /// </summary>
        public Boolean Selected 
        {
            get 
            {
                return _selected;
            }
            set
            {
                ///如果被锁定就不会被选中
                if (IsSimulation)
                    return;

                if (value)
                {
                    SVSelectPanelObjs.addControlItem(this);
                    setFocus();
                }
                else
                {
                    SVSelectPanelObjs.removeControlItem(this);
                    clearFocus();

                    if (_selected)
                        resizeControl();
                }

                _selected = value;
            }
        }

        /// <summary>
        /// 失去焦点，自动设置宽和高
        /// </summary>
        private void resizeControl()
        {
            ///如果字符为空，就不做下面处理
            if (String.IsNullOrWhiteSpace(this.Text))
                return;

            ///
            int singleLine = (int)this.Font.Size + 4;
            int sunCount = this.Text.Length * singleLine;

            int lineCount = sunCount / this.Width;
            int tmpHeight = lineCount * singleLine + (lineCount - 1) * 2;

            if (tmpHeight > this.Height)
                this.Height = tmpHeight;
        }

        /// <summary>
        /// 控件是否被锁定
        /// true-不被锁定
        /// false-锁定
        /// </summary>
        public Boolean IsMoved
        {
            get { return _isMoved; }
            set { _isMoved = value; }
        }

        /// <summary>
        /// 是否为仿真模式
        /// 
        /// 默认为正常运行模式
        /// </summary>
        public Boolean IsSimulation
        {
            get { return _isSimulation; }
            set { _isSimulation = value; }
        }

        /// <summary>
        /// 默认不被选中
        /// </summary>
        Boolean _selected = false;
        /// <summary>
        /// 默认不锁定
        /// </summary>
        Boolean _isMoved = true;
        /// <summary>
        /// 默认为不是仿真模式
        /// </summary>
        Boolean _isSimulation = false;

        /// <summary>
        /// 左上角
        /// </summary>
        SVPanelNode _leftTop = new SVPanelNode(NodeType.左上角);
        /// <summary>
        /// 右上角
        /// </summary>
        SVPanelNode _rightTop = new SVPanelNode(NodeType.右上角);
        /// <summary>
        /// 左下角
        /// </summary>
        SVPanelNode _leftBottom = new SVPanelNode(NodeType.左下角);
        /// <summary>
        /// 右下角
        /// </summary>
        SVPanelNode _rightBottom = new SVPanelNode(NodeType.右下角);
        /// <summary>
        /// 上
        /// </summary>
        SVPanelNode _top = new SVPanelNode(NodeType.上);
        /// <summary>
        /// 下
        /// </summary>
        SVPanelNode _bottom = new SVPanelNode(NodeType.下);
        /// <summary>
        /// 左
        /// </summary>
        SVPanelNode _left = new SVPanelNode(NodeType.左);
        /// <summary>
        /// 右
        /// </summary>
        SVPanelNode _right = new SVPanelNode(NodeType.右);

        /// <summary>
        /// 当窗口发生变化后
        /// </summary>
        public Action SizeChangedEvent;

        //定义当前节点显示类型
        SVNodesType _nodeType = SVNodesType.All;
        Boolean _isMouseDown = false;

        #endregion

        /// <summary>
        /// 自定义控件
        /// </summary>
        public SVBasePanel()
        {
            _leftTop.MainControl = this;
            _rightTop.MainControl = this;
            _leftBottom.MainControl = this;
            _rightBottom.MainControl = this;
            _top.MainControl = this;
            _bottom.MainControl = this;
            _left.MainControl = this;
            _right.MainControl = this;

            this.LocationChanged += new EventHandler(labelSizeChanged);
            this.SizeChanged += new EventHandler(labelSizeChanged);
            this.MouseDown += new MouseEventHandler(SVBasePanel_MouseDown);
            this.MouseUp += new MouseEventHandler(SVBasePanel_MouseUp);

            initControlMove();
            nodesMouseUp();
        }

        /// <summary>
        /// 设置当前选择节点显示类型
        /// 默认为全部显示
        /// </summary>
        /// <param name="type"></param>
        public void setNodeType(SVNodesType type)
        {
            _nodeType = type;
        }

        /// <summary>
        /// 注册所有节点鼠标弹起事件
        /// </summary>
        private void nodesMouseUp()
        {
            _leftTop.MouseUp += new MouseEventHandler((sender, e) =>
            {
                SizeChangedEvent();
                Selected = true;
            });

            _rightTop.MouseUp += new MouseEventHandler((sender, e) =>
            {
                SizeChangedEvent();
                Selected = true;
            });

            _leftBottom.MouseUp += new MouseEventHandler((sender, e) =>
            {
                SizeChangedEvent();
                Selected = true;
            });

            _rightBottom.MouseUp += new MouseEventHandler((sender, e) =>
            {
                SizeChangedEvent();
                Selected = true;
            });

            _top.MouseUp += new MouseEventHandler((sender, e) =>
            {
                SizeChangedEvent();
                Selected = true;
            });

            _bottom.MouseUp += new MouseEventHandler((sender, e) =>
            {
                SizeChangedEvent();
                Selected = true;
            });

            _left.MouseUp += new MouseEventHandler((sender, e) =>
            {
                SizeChangedEvent();
                Selected = true;
            });

            _right.MouseUp += new MouseEventHandler((sender, e) =>
            {
                SizeChangedEvent();
                Selected = true;
            });
        }

        /// <summary>
        /// 当前控件鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SVBasePanel_MouseUp(object sender, MouseEventArgs e)
        {
            SizeChangedEvent();
            //Selected = true;
        }

        /// <summary>
        /// 背景控件鼠标弹起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _backControl_MouseUp(object sender, MouseEventArgs e)
        {
            SizeChangedEvent();
            //Selected = true;            
        }

        /// <summary>
        /// 初始化控件移动事件
        /// </summary>
        private void initControlMove()
        {
            Point pos = new Point();

            this.MouseDown += new MouseEventHandler((sender, e) =>
            {
                if (!IsMoved)
                    return;

                if (e.Button != MouseButtons.Left)
                    return;

                pos = new Point(e.X, e.Y);
            });

            this.MouseMove += new MouseEventHandler((sender, e) =>
            {
                Cursor.Current = Cursors.Hand;

                if (!IsMoved)
                    return;

                if (e.Button != MouseButtons.Left)
                    return;

                Int32 cX = e.X - pos.X;
                Int32 cY = e.Y - pos.Y;

                ///防止控件在拖动过程中发生滑动
                double disIValue = Math.Sqrt(cX * cX + cY * cY);
                if (!_isMouseDown)
                {
                    if (disIValue > 18)
                        _isMouseDown = true;
                    return;
                }

                Int32 disX = cX + this.Location.X;
                Int32 disY = cY + this.Location.Y;

                if (disX < 0)
                    disX = 0;

                if (disY < 0)
                    disY = 0;

                if (disX + this.Width > Parent.Width)
                    disX = Parent.Width - this.Width;

                if (disY + this.Height > Parent.Height)
                    disY = Parent.Height - this.Height;

                this.Location = new Point(disX, disY);

                ///移动选中的所有控件
                SVSelectPanelObjs.moveSelectControls(this, cX, cY);

                this.clearFocus();
            });

            this.MouseUp += new MouseEventHandler((sender, e)=>
            {
                if (!IsMoved)
                    return;

                Cursor.Current = Cursors.SizeAll;

                if (e.Button != MouseButtons.Left)
                    return;

                this.Selected = true;
                _isMouseDown = false;
            });
        }

        /// <summary>
        /// 控件本身尺寸发生改变的事件
        /// 改变控件尺寸的控制点将跟着偏移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void labelSizeChanged(object sender, EventArgs e)
        {
            _leftTop.Location = new Point(this.Location.X - 5, this.Location.Y - 5);
            _rightTop.Location = new Point(this.Location.X + this.Width - 5, this.Location.Y - 5);
            _leftBottom.Location = new Point(this.Location.X - 5, this.Location.Y + this.Height - 5);
            _rightBottom.Location = new Point(this.Location.X + this.Width - 5, this.Location.Y + this.Height - 5);
            _top.Location = new Point(this.Location.X + this.Width / 2 - 5, this.Location.Y - 5);
            _bottom.Location = new Point(this.Location.X + this.Width / 2 - 5, this.Location.Y + this.Height - 5);
            _left.Location = new Point(this.Location.X - 5, this.Location.Y + (this.Height / 2) - 5);
            _right.Location = new Point(this.Location.X + this.Width - 5, this.Location.Y + (this.Height / 2) - 5);

            ///父窗口刷新(即页面窗口刷新)
            if (this.Parent != null)
                Parent.Refresh();
        }

        /// <summary>
        /// 鼠标按下，控件拥有焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SVBasePanel_MouseDown(object sender, MouseEventArgs e)
        {
            ///如果选中多个控件或者按下Ctrl按键，就直接选中
            if (SVSelectPanelObjs._VK_Ctrl == true)
            {
                Selected = true;
            }
            else
            {
                if (!Selected)
                {
                    ///让兄弟控件不被选中
                    if (this.Parent != null)
                    {
                        foreach (var item in Parent.Controls)
                        {
                            SVBasePanel panel = item as SVBasePanel;
                            if ((panel != null) && (panel != this))
                                panel.Selected = false;
                        }
                    }

                    ///选中当前控件
                    Selected = true;
                }
            }
        }

        /// <summary>
        /// 设置当前控件的选中状态
        /// </summary>
        void setFocus()
        {
            BringToFront();

            if (!IsMoved)
                return;

            if (this.Parent == null)
                return;

            switch (_nodeType)
            {
                case SVNodesType.All:
                    {
                        ///左上
                        _leftTop.Location = new Point(this.Location.X - 5, this.Location.Y - 5);
                        _leftTop.Size = new Size(10, 10);
                        Parent.Controls.Add(_leftTop);
                        _leftTop.BringToFront();

                        ///右上
                        _rightTop.Location = new Point(this.Location.X + this.Width - 5, this.Location.Y - 5);
                        _rightTop.Size = new Size(10, 10);
                        Parent.Controls.Add(_rightTop);
                        _rightTop.BringToFront();

                        ///左下
                        _leftBottom.Location = new Point(this.Location.X - 5, this.Location.Y + this.Height - 5);
                        _leftBottom.Size = new Size(10, 10);
                        Parent.Controls.Add(_leftBottom);
                        _leftBottom.BringToFront();

                        ///右下
                        _rightBottom.Location = new Point(this.Location.X + this.Width - 5, this.Location.Y + this.Height - 5);
                        _rightBottom.Size = new Size(10, 10);
                        Parent.Controls.Add(_rightBottom);
                        _rightBottom.BringToFront();

                        ///上
                        _top.Location = new Point(this.Location.X + this.Width / 2 - 5, this.Location.Y - 5);
                        _top.Size = new Size(10, 10);
                        Parent.Controls.Add(_top);
                        _top.BringToFront();

                        ///下
                        _bottom.Location = new Point(this.Location.X + this.Width / 2 - 5, this.Location.Y + this.Height - 5);
                        _bottom.Size = new Size(10, 10);
                        Parent.Controls.Add(_bottom);
                        _bottom.BringToFront();

                        ///左
                        _left.Location = new Point(this.Location.X - 5, this.Location.Y + (this.Height / 2) - 5);
                        _left.Size = new Size(10, 10);
                        Parent.Controls.Add(_left);
                        _left.BringToFront();

                        ///右
                        _right.Location = new Point(this.Location.X + this.Width - 5, this.Location.Y + (this.Height / 2) - 5);
                        _right.Size = new Size(10, 10);
                        Parent.Controls.Add(_right);
                        _right.BringToFront();
                    }
                    break;
                case SVNodesType.Horizontal:
                    {
                        ///左
                        _left.Location = new Point(this.Location.X - 5, this.Location.Y + (this.Height / 2) - 5);
                        _left.Size = new Size(10, 10);
                        Parent.Controls.Add(_left);
                        _left.BringToFront();

                        ///右
                        _right.Location = new Point(this.Location.X + this.Width - 5, this.Location.Y + (this.Height / 2) - 5);
                        _right.Size = new Size(10, 10);
                        Parent.Controls.Add(_right);
                        _right.BringToFront();
                    }
                    break;
                case SVNodesType.Vertical:
                    {
                        ///上
                        _top.Location = new Point(this.Location.X + this.Width / 2 - 5, this.Location.Y - 5);
                        _top.Size = new Size(10, 10);
                        Parent.Controls.Add(_top);
                        _top.BringToFront();

                        ///下
                        _bottom.Location = new Point(this.Location.X + this.Width / 2 - 5, this.Location.Y + this.Height - 5);
                        _bottom.Size = new Size(10, 10);
                        Parent.Controls.Add(_bottom);
                        _bottom.BringToFront();
                    }
                    break;
            }
        }

        /// <summary>
        /// 清除当前控件的选中状态
        /// </summary>
        public void clearFocus()
        {
            if (this.Parent == null)
                return;

            this.Parent.Controls.Remove(_leftTop);
            this.Parent.Controls.Remove(_rightTop);
            this.Parent.Controls.Remove(_leftBottom);
            this.Parent.Controls.Remove(_rightBottom);
            this.Parent.Controls.Remove(_top);
            this.Parent.Controls.Remove(_bottom);
            this.Parent.Controls.Remove(_left);
            this.Parent.Controls.Remove(_right);
        }
    }
}
