using System;
using System.Windows.Forms;
using SVControl;
using SVCore;

namespace SvduPro
{
    public partial class SVFindWindow : Form
    {
        //导航树
        SVTreeView _treeView;
        //查找窗口
        SVFindTextBox _findView;

        public SVFindWindow()
        {
            InitializeComponent();

            this.comboBox.SelectedIndexChanged += new EventHandler(comboBox_SelectedIndexChanged);
            this.comboBox.SelectedIndex = 0;

            SVInterfaceApplication app = SVApplication.Instance;
            _treeView = app.TreeProject as SVTreeView;
            _findView = app.FindWindow as SVFindTextBox;
        }

        /// <summary>
        /// 查找类型选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
                groupBox1.Enabled = false;
            else
                groupBox1.Enabled = true;
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            //通过ID号查找相关控件
            if (comboBox.SelectedIndex == 0)
            {
                int id = 0;
                Boolean r = int.TryParse(textBox.Text, out id);
                if (r)
                    findID(id);
                else
                {
                    MessageBox.Show("设置的查找条件不合法!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                String findString = textBox.Text;
                findStr(findString);
            }

            DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }

        /// <summary>
        /// 通过变量名称来查找对应控件
        /// </summary>
        /// <param name="findString">变量名称</param>
        private void findStr(string findString)
        {
            if (String.IsNullOrWhiteSpace(findString))
                return;

            _findView.Clear();
            _findView.AppendText("---------------查找结果如下:-----------------\n");
            _findView.AppendText("\n");

            foreach (SVPageNode pageNode in _treeView.Nodes)
            {
                foreach (TreeNode classItem in pageNode.Nodes)
                {
                    foreach (SVPageNode item in classItem.Nodes)
                    {
                        SVPageWidget widget = item.Addtionobj as SVPageWidget;
                        if (widget == null)
                            continue;

                        foreach (var panel in widget.Controls)
                        {
                            //如果为按钮
                            if (panel is SVButton)
                            {
                                SVButton button = (SVButton)panel;
                                if (button.Attrib.BtnType.VarText == findString)
                                {
                                    String text = String.Format("页面【{0}】中, 找到控件====>类型【{1}】.", widget.PageName, button.GetType().Name);
                                    _findView.AppendText(text);
                                    _findView.setMark(button);
                                    _findView.AppendText("\n");
                                }

                                if (button.Attrib.BtnType.EnVarText == findString)
                                {
                                    String text = String.Format("页面【{0}】中, 找到控件====>类型【{1}】.", widget.PageName, button.GetType().Name);
                                    _findView.AppendText(text);
                                    _findView.setMark(button);
                                    _findView.AppendText("\n");
                                }
                            }

                            //如果为模拟量
                            if (panel is SVAnalog)
                            {
                                SVAnalog analog = (SVAnalog)panel;
                                if (analog.Attrib.Var == findString)
                                {
                                    String text = String.Format("页面【{0}】中, 找到控件====>类型【{1}】.", widget.PageName, analog.GetType().Name);
                                    _findView.AppendText(text);
                                    _findView.setMark(analog);
                                    _findView.AppendText("\n");
                                }
                            }

                            //如果为开关量
                            if (panel is SVBinary)
                            {
                                SVBinary binary = (SVBinary)panel;
                                if (binary.Attrib.Var == findString)
                                {
                                    String text = String.Format("页面【{0}】中, 找到控件====>类型【{1}】.", widget.PageName, binary.GetType().Name);
                                    _findView.AppendText(text);
                                    _findView.setMark(binary);
                                    _findView.AppendText("\n");
                                }
                            }

                            //动态图
                            if (panel is SVGif)
                            {
                                SVGif gif = (SVGif)panel;
                                foreach (var str in gif.Attrib.VarName)
                                {
                                    if (str == findString)
                                    {
                                        String text = String.Format("页面【{0}】中, 找到控件====>类型【{1}】.", widget.PageName, gif.GetType().Name);
                                        _findView.AppendText(text);
                                        _findView.setMark(gif);
                                        _findView.AppendText("\n");
                                    }
                                }
                            }

                            //趋势图
                            if (panel is SVCurve)
                            {
                                SVCurve curve = (SVCurve)panel;
                                foreach (var str in curve.Attrib.VarArray)
                                {
                                    if (str == findString)
                                    {
                                        String text = String.Format("页面【{0}】中, 找到控件====>类型【{1}】.", widget.PageName, curve.GetType().Name);
                                        _findView.AppendText(text);
                                        _findView.setMark(curve);
                                        _findView.AppendText("\n");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!_findView.isMatches())
                _findView.AppendText("没有找到符合条件的相关内容!");
        }

        /// <summary>
        /// 通过控件ID号来查找对应的页面，并记录相关跳转信息
        /// </summary>
        /// <param name="id">控件ID</param>
        private void findID(int id)
        {
            _findView.Clear();
            _findView.AppendText("---------------查找结果如下:-----------------\n");
            _findView.AppendText("\n");

            foreach (SVPageNode pageNode in _treeView.Nodes)
            {
                foreach (TreeNode classItem in pageNode.Nodes)
                {
                    foreach (SVPageNode item in classItem.Nodes)
                    {
                        SVPageWidget widget = item.Addtionobj as SVPageWidget;
                        if (widget == null)
                            continue;

                        if (widget.Attrib.id == id)
                        {
                            String text = String.Format("找到页面====>【{0}】 id：{1}", widget.PageName, id);
                            _findView.AppendText(text);
                            _findView.setMark(widget);
                            _findView.AppendText("\n");
                        }

                        foreach (var panel in widget.Controls)
                        {
                            SVPanel p = panel as SVPanel;
                            if (p == null)
                                continue;

                            if (p.Id == id)
                            {
                                String text = String.Format("名称为【{0}】的页面中, 找到控件====>类型【{1}】.", widget.PageName, p.GetType().Name);
                                _findView.AppendText(text);
                                _findView.setMark(p);
                                _findView.AppendText("\n");
                            }
                        }
                    }
                }
            }

            if (!_findView.isMatches())
                _findView.AppendText("没有查找到相关内容!");
        }
    }
}
