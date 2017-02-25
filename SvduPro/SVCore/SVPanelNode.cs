using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SVCore
{
    /// <summary>
    /// 定义一个枚举,用来区分每种类型节点
    /// </summary>
    public enum NodeType
    {
        无,
        左上角,
        右上角,
        左下角,
        右下角,
        上,
        下,
        左,
        右
    }

    /// <summary>
    /// 控件的缩放节点,通过该节点对控件尺寸进行修改
    /// 同时用来指示控件是否被选中的状态
    /// </summary>
    public class SVPanelNode : Panel
    {
        #region 类成员变量定义
        NodeType _nodeType = new NodeType();
        Point _startPos = new Point();
        Control _mainControl = new Control();

        /// <summary>
        /// 设置和获取当前控制点关联的控件
        /// </summary>
        public Control MainControl
        {
            get { return _mainControl; }
            set { _mainControl = value; }
        }

        #endregion        

        #region 构造函数
        /// <summary>
        /// 构造函数，设置默认的背景显示颜色
        /// </summary>
        public SVPanelNode()
        {
            ///节点的颜色值
            this.BackColor = Color.Blue;
            //设置默认宽高
            this.Size = new System.Drawing.Size(10, 10);
            //默认类型
            _nodeType = NodeType.无;
        }

        /// <summary>
        /// 根据类型来初始化
        /// </summary>
        /// <param oldName="type">节点类型</param>
        public SVPanelNode(NodeType type)
        {
            ///节点的颜色值
            this.BackColor = Color.Blue;
            //类型
            _nodeType = type;
        }

        #endregion

        /// <summary>
        /// 重写鼠标按下事件
        /// </summary>
        /// <param oldName="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            showCursors();

            if (e.Button != MouseButtons.Left)
                return;

            _startPos = new Point(e.X, e.Y);            
        }

        /// <summary>
        /// 重写鼠标移动事件
        /// </summary>
        /// <param oldName="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            showCursors();            

            if (e.Button != MouseButtons.Left)
                return;

            Int32 disX = e.X - _startPos.X;
            Int32 disY = e.Y - _startPos.Y;
            this.Location = new Point(disX + this.Location.X, disY + this.Location.Y);
            
            modifyParentSize(disX, disY);
        }

        /// <summary>
        /// 根据类型来修改父控件的大小
        /// </summary>
        /// <param oldName="disX">x方向的偏移</param>
        /// <param oldName="disY">y方向的偏移</param>
        private void modifyParentSize(Int32 disX, Int32 disY)
        {
            if (MainControl == null)
                return;

            switch (_nodeType)
            {
                case NodeType.左上角:
                    {
                        MainControl.Width -= disX;
                        MainControl.Height -= disY;
                        MainControl.Location = new Point(disX + MainControl.Location.X, disY + MainControl.Location.Y);
                        break;
                    }
                case NodeType.右上角:
                    {
                        MainControl.Width += disX;
                        MainControl.Height -= disY;
                        MainControl.Location = new Point(MainControl.Location.X, MainControl.Location.Y + disY);
                        break;
                    }
                case NodeType.左下角:
                    {
                        MainControl.Width -= disX;
                        MainControl.Location = new Point(MainControl.Location.X + disX, MainControl.Location.Y);
                        MainControl.Height += disY;
                        break;
                    }
                case NodeType.右下角:
                    {
                        MainControl.Width += disX;
                        MainControl.Height += disY;
                        break;
                    }
                case NodeType.上:
                    {
                        MainControl.Height -= disY;
                        MainControl.Location = new Point(MainControl.Location.X, MainControl.Location.Y + disY);                        
                        break;
                    }
                case NodeType.下:
                    {
                        MainControl.Height += disY;
                        break;
                    }
                case NodeType.左:
                    {
                        MainControl.Width = MainControl.Width - disX;
                        MainControl.Location = new Point(MainControl.Location.X + disX, MainControl.Location.Y);                        
                        break;
                    }
                case NodeType.右:
                    {
                        MainControl.Width += disX;
                        break;
                    }
            }
        }

        /// <summary>
        /// 根据类型的不同，显示的光标也不同
        /// </summary>
        private void showCursors()
        {
            switch (_nodeType)
            {
                case NodeType.左上角:
                case NodeType.右下角:
                    Cursor.Current = Cursors.SizeNWSE;
                    break;
                case NodeType.左下角:
                case NodeType.右上角:
                    Cursor.Current = Cursors.SizeNESW;
                    break;
                case NodeType.上:
                case NodeType.下:
                    Cursor.Current = Cursors.SizeNS;
                    break;
                case NodeType.左:
                case NodeType.右:
                    Cursor.Current = Cursors.SizeWE;
                    break;
            }
        }
    }
}
