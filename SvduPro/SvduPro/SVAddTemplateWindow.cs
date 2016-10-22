using System;
using System.Windows.Forms;
using SVControl;
using SVCore;
using System.IO;
using System.Drawing;

namespace SvduPro
{
    public partial class SVAddTemplateWindow : Form
    {
        /// <summary>
        /// 当前关联的树形窗口
        /// </summary>
        SVTreeView _svTreeView;

        public SVAddTemplateWindow(SVTreeView svTreeView)
        {
            InitializeComponent();

            ///进行布局
            _svTreeView = svTreeView;            
            this.panel1.Controls.Add(svTreeView);
            svTreeView.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            SVPageNode node = _svTreeView.SelectedNode as SVPageNode;
            if (node == null || node.Level != 2)
            {
                SVMessageBox msgBox = new SVMessageBox();
                msgBox.content(Resource.提示, Resource.请选择页面);
                msgBox.ShowDialog();
                return;
            }

            SVPageWidget widget = node.Addtionobj as SVPageWidget;
            ///模板文件
            String file = Path.Combine(SVProData.TemplatePath, node.Text);
            if (File.Exists(file))
            {
                SVMessageBox msgBox = new SVMessageBox();
                msgBox.content(Resource.提示, node.Text + " " + Resource.模板已经存在);
                msgBox.ShowDialog();
                return;
            }

            //保存文件
            SVXml pageXML = new SVXml();
            pageXML.createRootEle("Page");
            widget.saveXML(pageXML);
            pageXML.writeXml(file);

            //保存缩略图
            Bitmap ctlbitmap = new Bitmap(widget.Width, widget.Height);
            widget.DrawToBitmap(ctlbitmap, widget.ClientRectangle);
            ctlbitmap.Save(file + ".jpg");

            //记录日志
            SVLog.WinLog.Info(String.Format("模板{0}保存成功", node.Text));

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
