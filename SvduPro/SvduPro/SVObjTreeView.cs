using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SVCore;
using SVControl;

namespace SvduPro
{
    delegate void Function1(TreeNode treenodes, Control c);
    ///public delegate void ClickHander(Object o);

    /// <summary>
    /// 对象窗口类，用来显示当前选中的页面控件中子控件信息
    /// </summary>
    public class SVObjTreeView : TreeView
    {
        class ObjTreeNode : TreeNode
        {
            public Control objControl { get; set; }
        }

        ///public ClickHander ClickHander;
        Dictionary<String, Function1> _nameDict;

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        public SVObjTreeView()
        {
            this.Indent = 20;
            this.ItemHeight = 20;
            this.Font = new Font(this.Font.FontFamily, 11.0f);

            ///建立名称与控件的对应关系
            _nameDict = new Dictionary<String, Function1>();
            _nameDict.Add("SVButton", (cc, c) => 
            {
                SVButton button = (SVButton)c;
                String text = String.Format("按钮-(ID：{0})", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                cc.Nodes.Add(node);
            });

            _nameDict.Add("SVLabel", (cc, c) =>
            {
                SVLabel button = (SVLabel)c;
                String text = String.Format("文本-(ID：{0})", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                cc.Nodes.Add(node);
            });

            _nameDict.Add("SVAnalog", (cc, c) =>
            {
                SVAnalog button = (SVAnalog)c;
                String text = String.Format("模拟量-(ID：{0})", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                cc.Nodes.Add(node);
            });

            _nameDict.Add("SVBinary", (cc, c) =>
            {
                SVBinary button = (SVBinary)c;
                String text = String.Format("开关量-(ID：{0})", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                cc.Nodes.Add(node);
            });

            _nameDict.Add("SVCurve", (cc, c) =>
            {
                SVCurve button = (SVCurve)c;
                String text = String.Format("趋势图-(ID：{0})", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                cc.Nodes.Add(node);
            });

            _nameDict.Add("SVIcon", (cc, c) =>
            {
                SVIcon button = (SVIcon)c;
                String text = String.Format("静态图-(ID：{0})", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                cc.Nodes.Add(node);
            });

            _nameDict.Add("SVLine", (cc, c) =>
            {
                SVLine button = (SVLine)c;
                String text = String.Format("直线-(ID：{0})", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                cc.Nodes.Add(node);
            });

            _nameDict.Add("SVGif", (cc, c) =>
            {
                SVGif button = (SVGif)c;
                String text = String.Format("动态图-(ID：{0})", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                cc.Nodes.Add(node);
            });

            _nameDict.Add("SVHeartbeat", (cc, c) =>
            {
                SVHeartbeat button = (SVHeartbeat)c;
                String text = String.Format("心跳控件-(ID：{0})", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                cc.Nodes.Add(node);
            });

            this.AfterSelect += new TreeViewEventHandler(SVObjTreeView_AfterSelect);
            this.NodeMouseClick += new TreeNodeMouseClickEventHandler(SVObjTreeView_NodeMouseClick);
        }

        /// <summary>
        /// 单击树节点事件
        /// 清空当前选中节点对象的选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SVObjTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ObjTreeNode node = this.SelectedNode as ObjTreeNode;
            if (node == null)
                return;

            SVPanel panel = node.objControl as SVPanel;
            if (panel == null)
                return;

            panel.Selected = false;
            panel.Refresh();
        }

        /// <summary>
        /// 选中树节点后发生的事件
        /// 将当前选中节点中的控件对象设置为选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SVObjTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ObjTreeNode node = e.Node as ObjTreeNode;
            if (node == null)
                return;

            SVPanel panel = node.objControl as SVPanel;
            if (panel == null)
                return;

            panel.Selected = true;
            panel.Refresh();
        }

        /// <summary>
        /// 返回当前树中对应页面对象
        /// </summary>
        /// <returns>页面控件</returns>
        SVPageWidget getCurrentNode()
        {
            if (this.Nodes.Count == 0)
                return null;

            ObjTreeNode tmp = this.Nodes[0] as ObjTreeNode;
            if (tmp == null)
                return null;

            return (SVPageWidget)(tmp.objControl);
        }

        /// <summary>
        /// 在树中显示当前页面的子控件信息
        /// </summary>
        /// <param name="pageWidget">页面控件窗口对象</param>
        public void setPageWidget(SVPageWidget pageWidget)
        {
            //如果没有发生变化，就直接返回
            if (getCurrentNode() == pageWidget)
                return;

            clearAllNodes();
            String text = String.Format("页面-(ID：{0})", pageWidget.Attrib.id);
            ObjTreeNode rootNode = new ObjTreeNode();
            rootNode.Text = text;
            rootNode.objControl = pageWidget;            
            this.Nodes.Add(rootNode);

            //对显示名称进行排序
            var sortList = from Control item in pageWidget.Controls
                           orderby item.Text ascending //或descending
                           select item;

            foreach (Control item in sortList)
            {
                String name = item.GetType().Name;
                if (_nameDict.ContainsKey(name))
                    _nameDict[name](rootNode, item);
            }

            rootNode.ExpandAll();
        }

        /// <summary>
        /// 清空树
        /// </summary>
        public void clearAllNodes()
        {
            this.Nodes.Clear();
        }
    }
}
