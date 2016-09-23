using System;
using System.Windows.Forms;
using SVCore;
using SVControl;

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

        void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
                groupBox1.Enabled = false;
            else
                groupBox1.Enabled = true;
        }

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
            }

            DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void findID(int id)
        {
            _findView.Clear();
            _findView.AppendText("---------------查找结果如下:-----------------\n");
            _findView.AppendText("\n");

            foreach (SVPageNode pageNode in _treeView.Nodes)
            {
                foreach (SVPageNode item in pageNode.Nodes)
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
                            _findView.setMark(widget);
                            _findView.AppendText("\n");
                        }
                    }
                }
            }

            if (!_findView.isMatches())
                _findView.AppendText("没有查找到相关内容!");
        }
    }
}
