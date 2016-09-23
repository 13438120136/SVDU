/**
 * 重写treeview类，主要是用来进行功能性的封装，提高控件的独立性和功能的独立性。
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SVCore;

namespace SVControl
{
    public class SVTreeView : TreeView
    {
        //工程节点
        SVPageNode _proNode = new SVPageNode();

        Dictionary<SVPageNode, String> _mapDict = new Dictionary<SVPageNode, String>();

        //工程节点右键菜单
        public ContextMenuStrip ProContextMenu 
        {
            get
            {
                return _proNode.ContextMenuStrip;
            }
            set
            {
                _proNode.ContextMenuStrip = value;
            }
        }

        public SVTreeView()
        {
            ImageList _imageList = new ImageList();
            _imageList.Images.Add("Project", Resource.project);
            _imageList.Images.Add("Page", Resource.page);
            //_imageList.Images.Add("MainPage", Resource.start);
            this.ImageList = _imageList;

            ///
            this.ItemHeight = 20;
            this.Font = new Font(this.Font.FontFamily, 11.0f);

            initalizeNodes();

            _mapDict.Add(_proNode, "普通页面");
            initInstance();

            ///当节点标签内容修改后发生事件
            this.AfterLabelEdit += new NodeLabelEditEventHandler(SVTreeView_AfterLabelEdit);
        }

        /// <summary>
        /// 树节点标签也修改后的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SVTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            this.LabelEdit = false;
        }

        /// <summary>
        /// 根据输入的名称来新建页面分类节点
        /// </summary>
        /// <param name="text">新建的页面名称</param>
        /// <returns>返回新的页面节点</returns>
        public TreeNode newClassNode(String text)
        {
            ///添加子节点
            TreeNode node = _proNode.Nodes.Add(text);
            //展开
            this.ExpandAll();

            ///使其可以编辑
            this.LabelEdit = true;
            ///开始编辑节点
            node.BeginEdit();
            return node;
        }

        /// <summary>
        /// 对指定节点进行重命名
        /// </summary>
        /// <param name="node">输入的树节点</param>
        public void renameTreeNode(TreeNode node)
        {
            ///使其可以编辑
            this.LabelEdit = true;
            ///开始编辑节点
            node.BeginEdit();
        }

        /// <summary>
        /// 注册所有控件类型
        /// 
        /// 后面会通过该类型创建对象
        /// </summary>
        void initInstance()
        {
            SVNameToObject.addInstance(typeof(SVButton));
            SVNameToObject.addInstance(typeof(SVLabel));
            SVNameToObject.addInstance(typeof(SVAnalog));
            SVNameToObject.addInstance(typeof(SVBinary));
            SVNameToObject.addInstance(typeof(SVCurve));
            SVNameToObject.addInstance(typeof(SVGif));
            SVNameToObject.addInstance(typeof(SVIcon));
            SVNameToObject.addInstance(typeof(SVLine));
            SVNameToObject.addInstance(typeof(SVHeartbeat));
        }

        public SVPageNode getNodeFromName(String name)
        {
            foreach (var item in _mapDict)
            {
                if (item.Value == name)
                    return item.Key;
            }

            return null;
        }

        public String getNameFromNode(SVPageNode node)
        {
            foreach (var item in _mapDict)
            {
                if (item.Key == node)
                    return item.Value;
            }

            return null;
        }

        //初始化工程数最上层的节点
        void initalizeNodes()
        {
            _proNode.ImageKey = "Project";
            _proNode.SelectedImageKey = "Project";
        }

        public void initTree()
        {
            this.Nodes.Clear();
            _proNode.Nodes.Clear();
            this.Nodes.Add(_proNode);
        }

        //得到工程节点
        public SVPageNode proNode()
        {
            return _proNode;
        }

        /// <summary>
        /// 保存当前工程中的所有页面数据
        /// </summary>
        public void saveAllPageData()
        {
            ///根节点,工程名节点
            foreach (TreeNode stationNode in this.Nodes)
            {
                ///所有分类节点
                foreach (TreeNode item in stationNode.Nodes)
                {
                    ///所有页面节点
                    foreach (SVPageNode page in item.Nodes)
                    {
                        SVPageWidget widget = page.Addtionobj as SVPageWidget;
                        if (widget == null)
                            continue;

                        ///保存页面
                        widget.saveSelf();
                    }
                }
            }
        }
    }
}
