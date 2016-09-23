using System;
using System.Windows.Forms;
using System.Drawing;

namespace SVCore
{
    public class SVPageNode : TreeNode
    {
        static SVPageNode _isMainNode;

        /// <summary>
        /// 设置当前节点是否为启动节点
        /// </summary>
        public static SVPageNode IsMainNode
        {
            get { return _isMainNode; }
            set 
            {
                if (_isMainNode != null)
                {
                    ///去掉当前工程的显示属性
                    //_isMainNode.NodeFont = new Font(value.TreeView.Font, value.TreeView.Font.Style);
                    _isMainNode.ForeColor = Color.Black;
                }

                ///设置新的显示属性
                //value.NodeFont = new Font(value.TreeView.Font, value.TreeView.Font.Style | FontStyle.Bold);
                value.ForeColor = Color.Red;
                _isMainNode = value; 
            }
        }

        //页面节点的附加属性
        public Object Addtionobj { get; set; }

        public SVPageNode(String text)
            :base(text)
        {
        }

        public SVPageNode()
            : base()
        {
        }

        public void setMainNode()
        {
            IsMainNode = this;
        }
    }
}
