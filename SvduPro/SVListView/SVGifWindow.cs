using System;
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
        /// <param name="gif">传入一个动态图对象</param>
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
                if (!File.Exists(file))
                    return;

                SVPixmapFile pixmapFile = new SVPixmapFile();
                pixmapFile.readPixmapFile(file);
                errBtn.BackgroundImageLayout = ImageLayout.Zoom;
                errBtn.BackgroundImage = pixmapFile.getBitmapFromData();
            }

            this.bgBtn.Enabled = (_gif.Attrib.VarName.Count != 0);
            foreach (var item in _gif.Attrib.VarName)
            {
                varListView.Items.Add(item);
            }
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.varListView = new System.Windows.Forms.ListView();
            this.addVarBtn = new System.Windows.Forms.Button();
            this.delVarBtn = new System.Windows.Forms.Button();
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
            this.groupBox1.Controls.Add(this.delVarBtn);
            this.groupBox1.Controls.Add(this.addVarBtn);
            this.groupBox1.Controls.Add(this.varListView);
            this.groupBox1.Location = new System.Drawing.Point(27, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 194);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "变量";
            // 
            // varListView
            // 
            this.varListView.Location = new System.Drawing.Point(16, 20);
            this.varListView.Name = "varListView";
            this.varListView.Size = new System.Drawing.Size(154, 129);
            this.varListView.TabIndex = 0;
            this.varListView.UseCompatibleStateImageBehavior = false;
            this.varListView.View = System.Windows.Forms.View.List;
            // 
            // addVarBtn
            // 
            this.addVarBtn.Location = new System.Drawing.Point(16, 161);
            this.addVarBtn.Name = "addVarBtn";
            this.addVarBtn.Size = new System.Drawing.Size(75, 23);
            this.addVarBtn.TabIndex = 1;
            this.addVarBtn.Text = "添加";
            this.addVarBtn.UseVisualStyleBackColor = true;
            this.addVarBtn.Click += new System.EventHandler(this.addVarBtn_Click);
            // 
            // delVarBtn
            // 
            this.delVarBtn.Location = new System.Drawing.Point(98, 161);
            this.delVarBtn.Name = "delVarBtn";
            this.delVarBtn.Size = new System.Drawing.Size(75, 23);
            this.delVarBtn.TabIndex = 2;
            this.delVarBtn.Text = "删除";
            this.delVarBtn.UseVisualStyleBackColor = true;
            this.delVarBtn.Click += new System.EventHandler(this.delVarBtn_Click);
            // 
            // errBtn
            // 
            this.errBtn.Location = new System.Drawing.Point(29, 33);
            this.errBtn.Name = "errBtn";
            this.errBtn.Size = new System.Drawing.Size(100, 100);
            this.errBtn.TabIndex = 1;
            this.errBtn.Text = "出错背景图片";
            this.errBtn.UseVisualStyleBackColor = true;
            this.errBtn.Click += new System.EventHandler(this.errBtn_Click);
            // 
            // bgBtn
            // 
            this.bgBtn.Location = new System.Drawing.Point(29, 147);
            this.bgBtn.Name = "bgBtn";
            this.bgBtn.Size = new System.Drawing.Size(100, 23);
            this.bgBtn.TabIndex = 2;
            this.bgBtn.Text = "背景图片";
            this.bgBtn.UseVisualStyleBackColor = true;
            this.bgBtn.Click += new System.EventHandler(this.bgBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.errBtn);
            this.groupBox2.Controls.Add(this.bgBtn);
            this.groupBox2.Location = new System.Drawing.Point(233, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 194);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "背景图片";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(27, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(363, 2);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(229, 258);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 5;
            this.okBtn.Text = "确定";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(310, 258);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 6;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // SVGifWindow
            // 
            this.ClientSize = new System.Drawing.Size(417, 292);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SVGifWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "动态图变量设置窗口";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 添加变量事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addVarBtn_Click(object sender, System.EventArgs e)
        {
            if (varListView.Items.Count >= 3)
            {
                MessageBox.Show("最大只能添加3个变量", "提示");
                return;
            }

            SVVarWindow varWindow = new SVVarWindow();
            if (varWindow.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                varListView.Items.Add(varWindow.varText());
            }

            bgBtn.Enabled = (varListView.Items.Count != 0);
        }

        /// <summary>
        /// 删除变量事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delVarBtn_Click(object sender, System.EventArgs e)
        {
            foreach (ListViewItem item in varListView.SelectedItems)
            {
                varListView.Items.Remove(item);
            }

            bgBtn.Enabled = (varListView.Items.Count != 0);
        }

        /// <summary>
        /// 设置出错背景图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgBtn_Click(object sender, System.EventArgs e)
        {
            Int32 count = this.varListView.Items.Count;
            SVBitmapArrayWindow window = new SVBitmapArrayWindow(_gif.Attrib.Pic, (Int32)Math.Pow(2, count));
            window.ShowDialog();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
