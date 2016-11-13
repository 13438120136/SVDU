using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SVCore;

namespace SVControl
{
    /// <summary>
    /// 动态图变量数据设置窗口
    /// </summary>
    public class SVGifWindow : Form
    {
        private GroupBox groupBox1;
        private Button delVarBtn;
        private Button addVarBtn;
        private ListView varListView;
        private Button errBtn;
        private Button bgBtn;
        private GroupBox groupBox2;
        private Label label1;
        private Button okBtn;
        private Button cancelBtn;
        SVGif _gif;

        /// <summary>
        /// 构造函数定义
        /// </summary>
        /// <param Name="gif">传入一个动态图对象</param>
        public SVGifWindow(SVGif gif)
        {
            _gif = gif;
            ///初始化界面
            InitializeComponent();
            init();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void init()
        {
            if (_gif.Attrib.PicError.ImageFileName != null)
            {
                String file = Path.Combine(SVProData.IconPath, _gif.Attrib.PicError.ImageFileName);
                if (File.Exists(file))
                {
                    SVPixmapFile pixmapFile = new SVPixmapFile();
                    pixmapFile.readPixmapFile(file);
                    errBtn.BackgroundImageLayout = ImageLayout.Zoom;
                    errBtn.BackgroundImage = pixmapFile.getBitmapFromData();
                }
            }

            this.bgBtn.Enabled = (_gif.Attrib.VarName.Count != 0);
            foreach (var item in _gif.Attrib.VarName)
            {
                varListView.Items.Add(item);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVGifWindow));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.delVarBtn = new System.Windows.Forms.Button();
            this.addVarBtn = new System.Windows.Forms.Button();
            this.varListView = new System.Windows.Forms.ListView();
            this.errBtn = new System.Windows.Forms.Button();
            this.bgBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.delVarBtn);
            this.groupBox1.Controls.Add(this.addVarBtn);
            this.groupBox1.Controls.Add(this.varListView);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // delVarBtn
            // 
            resources.ApplyResources(this.delVarBtn, "delVarBtn");
            this.delVarBtn.Name = "delVarBtn";
            this.delVarBtn.UseVisualStyleBackColor = true;
            this.delVarBtn.Click += new System.EventHandler(this.delVarBtn_Click);
            // 
            // addVarBtn
            // 
            resources.ApplyResources(this.addVarBtn, "addVarBtn");
            this.addVarBtn.Name = "addVarBtn";
            this.addVarBtn.UseVisualStyleBackColor = true;
            this.addVarBtn.Click += new System.EventHandler(this.addVarBtn_Click);
            // 
            // varListView
            // 
            resources.ApplyResources(this.varListView, "varListView");
            this.varListView.Name = "varListView";
            this.varListView.UseCompatibleStateImageBehavior = false;
            this.varListView.View = System.Windows.Forms.View.List;
            // 
            // errBtn
            // 
            resources.ApplyResources(this.errBtn, "errBtn");
            this.errBtn.Name = "errBtn";
            this.errBtn.UseVisualStyleBackColor = true;
            this.errBtn.Click += new System.EventHandler(this.errBtn_Click);
            // 
            // bgBtn
            // 
            resources.ApplyResources(this.bgBtn, "bgBtn");
            this.bgBtn.Name = "bgBtn";
            this.bgBtn.UseVisualStyleBackColor = true;
            this.bgBtn.Click += new System.EventHandler(this.bgBtn_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.errBtn);
            this.groupBox2.Controls.Add(this.bgBtn);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Name = "label1";
            // 
            // okBtn
            // 
            resources.ApplyResources(this.okBtn, "okBtn");
            this.okBtn.Name = "okBtn";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            resources.ApplyResources(this.cancelBtn, "cancelBtn");
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // SVGifWindow
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SVGifWindow";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 添加变量事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void addVarBtn_Click(object sender, System.EventArgs e)
        {
            if (varListView.Items.Count >= 3)
            {
                MessageBox.Show("最大只能添加3个变量", "提示");
                return;
            }

            SVVarWindow varWindow = new SVVarWindow();
            varWindow.setFilter(new List<String> { "BOOL", "BOOL_VAR" });
            if (varWindow.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                _gif.Attrib.VarType.Add(varWindow.getVarType());
                varListView.Items.Add(varWindow.varText());
            }

            bgBtn.Enabled = (varListView.Items.Count != 0);
        }

        /// <summary>
        /// 删除变量事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void delVarBtn_Click(object sender, System.EventArgs e)
        {
            foreach (ListViewItem item in varListView.SelectedItems)
            {
                int index = varListView.Items.IndexOf(item);
                _gif.Attrib.VarType.RemoveAt(index);
                varListView.Items.RemoveAt(index);
            }

            bgBtn.Enabled = (varListView.Items.Count != 0);
        }

        /// <summary>
        /// 设置出错背景图片
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void errBtn_Click(object sender, System.EventArgs e)
        {
            SVBitmapManagerWindow window = new SVBitmapManagerWindow();
            if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                String file = Path.Combine(SVProData.IconPath, window.SvBitMap.ImageFileName);
                _gif.Attrib.PicError = window.SvBitMap;

                if (!File.Exists(file))
                    return;

                SVPixmapFile pixmapFile = new SVPixmapFile();
                pixmapFile.readPixmapFile(file);
                errBtn.BackgroundImageLayout = ImageLayout.Zoom;
                errBtn.BackgroundImage = pixmapFile.getBitmapFromData();
            }
        }

        /// <summary>
        /// 设置动态图背景图片数组
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void bgBtn_Click(object sender, System.EventArgs e)
        {
            Int32 count = this.varListView.Items.Count;
            SVBitmapArrayWindow window = new SVBitmapArrayWindow(_gif.Attrib.Pic, (Int32)Math.Pow(2, count));
            window.ShowDialog();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void okBtn_Click(object sender, System.EventArgs e)
        {
            _gif.Attrib.VarName.Clear();

            foreach (ListViewItem item in this.varListView.Items)
            {
                _gif.Attrib.VarName.Add(item.Text);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void cancelBtn_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
