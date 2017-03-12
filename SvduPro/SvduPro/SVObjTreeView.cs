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

        TreeNode _btnNode = new TreeNode("按钮");
        TreeNode _textNode = new TreeNode("文本");
        TreeNode _curveNode = new TreeNode("趋势图");
        TreeNode _heartNode = new TreeNode("心跳控件");
        TreeNode _analogNode = new TreeNode("模拟量");
        TreeNode _binaryNode = new TreeNode("开关量");
        TreeNode _iconNode = new TreeNode("静态图");
        TreeNode _gifNode = new TreeNode("动态图");
        TreeNode _lineNode = new TreeNode("直线");

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
                if (!cc.Nodes.Contains(_btnNode))
                    cc.Nodes.Add(_btnNode);

                SVButton button = (SVButton)c;
                //String text = String.Format("按钮-(ID：{0})", button.Attrib.ID);
                String text = String.Format("ID：{0}", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                _btnNode.Nodes.Add(node);
            });

            _nameDict.Add("SVLabel", (cc, c) =>
            {
                if (!cc.Nodes.Contains(_textNode))
                    cc.Nodes.Add(_textNode);

                SVLabel button = (SVLabel)c;
                //String text = String.Format("文本-(ID：{0})", button.Attrib.ID);
                String text = String.Format("ID：{0}", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                _textNode.Nodes.Add(node);
            });

            _nameDict.Add("SVAnalog", (cc, c) =>
            {
                if (!cc.Nodes.Contains(_analogNode))
                    cc.Nodes.Add(_analogNode);

                SVAnalog button = (SVAnalog)c;
                //String text = String.Format("模拟量-(ID：{0})", button.Attrib.ID);
                String text = String.Format("ID：{0}", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                _analogNode.Nodes.Add(node);
            });

            _nameDict.Add("SVBinary", (cc, c) =>
            {
                if (!cc.Nodes.Contains(_binaryNode))
                    cc.Nodes.Add(_binaryNode);

                SVBinary button = (SVBinary)c;
                //String text = String.Format("开关量-(ID：{0})", button.Attrib.ID);
                String text = String.Format("ID：{0}", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                _binaryNode.Nodes.Add(node);
            });

            _nameDict.Add("SVCurve", (cc, c) =>
            {
                if (!cc.Nodes.Contains(_curveNode))
                    cc.Nodes.Add(_curveNode);

                SVCurve button = (SVCurve)c;
                //String text = String.Format("趋势图-(ID：{0})", button.Attrib.ID);
                String text = String.Format("ID：{0}", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                _curveNode.Nodes.Add(node);
            });

            _nameDict.Add("SVIcon", (cc, c) =>
            {
                if (!cc.Nodes.Contains(_iconNode))
                    cc.Nodes.Add(_iconNode);

                SVIcon button = (SVIcon)c;
                //String text = String.Format("静态图-(ID：{0})", button.Attrib.ID);
                String text = String.Format("ID：{0}", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                _iconNode.Nodes.Add(node);
            });

            _nameDict.Add("SVLine", (cc, c) =>
            {
                if (!cc.Nodes.Contains(_lineNode))
                    cc.Nodes.Add(_lineNode);

                SVLine button = (SVLine)c;
                //String text = String.Format("直线-(ID：{0})", button.Attrib.ID);
                String text = String.Format("ID：{0}", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                _lineNode.Nodes.Add(node);
            });

            _nameDict.Add("SVGif", (cc, c) =>
            {
                if (!cc.Nodes.Contains(_gifNode))
                    cc.Nodes.Add(_gifNode);

                SVGif button = (SVGif)c;
                //String text = String.Format("动态图-(ID：{0})", button.Attrib.ID);
                String text = String.Format("ID：{0}", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                _gifNode.Nodes.Add(node);
            });

            _nameDict.Add("SVHeartbeat", (cc, c) =>
            {
                if (!cc.Nodes.Contains(_heartNode))
                    cc.Nodes.Add(_heartNode);

                SVHeartbeat button = (SVHeartbeat)c;
                //String text = String.Format("心跳控件-(ID：{0})", button.Attrib.ID);
                String text = String.Format("ID：{0}", button.Attrib.ID);

                ObjTreeNode node = new ObjTreeNode();
                node.Text = text;
                node.objControl = c;
                _heartNode.Nodes.Add(node);
            });

            this.AfterSelect += new TreeViewEventHandler(SVObjTreeView_AfterSelect);
            this.NodeMouseClick += new TreeNodeMouseClickEventHandler(SVObjTreeView_NodeMouseClick);
        }

        /// <summary>
        /// 单击树节点事件
        /// 清空当前选中节点对象的选中状态
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
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
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
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
        /// <param Name="pageWidget">页面控件窗口对象</param>
        public void setPageWidget(SVPageWidget pageWidget)
        {
            //如果没有发生变化，就直接返回
            //if (getCurrentNode() == pageWidget)
            //    return;

            clearAllNodes();
            String text = String.Format("页面名称:{0}, (ID：{1})", pageWidget.PageName, pageWidget.Attrib.id);
            ObjTreeNode rootNode = new ObjTreeNode();
            rootNode.Text = text;
            rootNode.objControl = pageWidget;            
            this.Nodes.Add(rootNode);

            //根据ID进行排序
            var sortList = from SVPanel item in pageWidget.Controls
                           orderby item.Id ascending //或descending 和ascending
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
            _btnNode.Nodes.Clear();
            _textNode.Nodes.Clear();
            _lineNode.Nodes.Clear();
            _curveNode.Nodes.Clear();
            _heartNode.Nodes.Clear();
            _iconNode.Nodes.Clear();
            _gifNode.Nodes.Clear();
            _analogNode.Nodes.Clear();
            _binaryNode.Nodes.Clear();
        }
    }
}
