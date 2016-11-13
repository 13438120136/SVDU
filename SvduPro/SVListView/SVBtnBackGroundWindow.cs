using System.Windows.Forms;
using SVCore;
using System.IO;
using System;

namespace SVControl
{
    /// <summary>
    /// 修改按钮背景的自定义窗口类
    /// </summary>
    public class SVBtnBackGroundWindow : Form
    {
        private SVCheckGroupBox colorGroupBox;
        private Label label1;
        private Button okBtn;
        private Button cancelBtn;
        private SVCheckGroupBox picGroupBox;
        private SVColorButton colorBtnUp;
        private SVColorButton colorBtnDown;
        private Button picBtnUp;
        private Button picBtnDown;
        SVButton _button;
    
        /// <summary>
        /// 自定义构造函数
        /// </summary>
        /// <param Name="button">按钮对象</param>
        public SVBtnBackGroundWindow(SVButton button)
        {
            InitializeComponent();
            _button = button;

            ///设置颜色checkbox
            this.colorGroupBox.init();
            this.colorGroupBox.setEnabledText("颜色");

            ///设置图片checkbox
            this.picGroupBox.init();
            this.picGroupBox.setEnabledText("图片");

            if (_button.Attrib.IsShowPic)
            {
                this.colorGroupBox.setChecked(false);
                this.picGroupBox.setChecked(true);
            }
            else
            {
                this.colorGroupBox.setChecked(true);
                this.picGroupBox.setChecked(false);
            }

            if (_button.Attrib.BtnDownPic.ImageFileName != null)
            {
                String file = Path.Combine(SVProData.IconPath, _button.Attrib.BtnDownPic.ImageFileName);
                setButtonBackGd(picBtnDown, file);
            }

            if (_button.Attrib.BtnUpPic.ImageFileName != null)
            {
                String file = Path.Combine(SVProData.IconPath, _button.Attrib.BtnUpPic.ImageFileName);
                setButtonBackGd(picBtnUp, file);
            }

            colorBtnDown.BackColor = _button.Attrib.BackColorgroundDown;
            colorBtnUp.BackColor = _button.Attrib.BackColorground;

            ///颜色发生改变
            colorGroupBox.EnabledChanged += new System.EventHandler(colorGroupBox_EnabledChanged);
            ///图片发生改变
            picGroupBox.EnabledChanged += new System.EventHandler(picGroupBox_EnabledChanged);
        }

        void picGroupBox_EnabledChanged(object sender, System.EventArgs e)
        {
            colorGroupBox.setChecked(!picGroupBox.checkEnabled());
        }

        void colorGroupBox_EnabledChanged(object sender, System.EventArgs e)
        {
            picGroupBox.setChecked(!colorGroupBox.checkEnabled());
        }

        /// <summary>
        /// 执行初始化
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVBtnBackGroundWindow));
            this.colorGroupBox = new SVCore.SVCheckGroupBox();
            this.colorBtnUp = new SVCore.SVColorButton();
            this.colorBtnDown = new SVCore.SVColorButton();
            this.picGroupBox = new SVCore.SVCheckGroupBox();
            this.picBtnUp = new System.Windows.Forms.Button();
            this.picBtnDown = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.colorGroupBox.SuspendLayout();
            this.picGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // colorGroupBox
            // 
            resources.ApplyResources(this.colorGroupBox, "colorGroupBox");
            this.colorGroupBox.Controls.Add(this.colorBtnUp);
            this.colorGroupBox.Controls.Add(this.colorBtnDown);
            this.colorGroupBox.Name = "colorGroupBox";
            this.colorGroupBox.TabStop = false;
            // 
            // colorBtnUp
            // 
            resources.ApplyResources(this.colorBtnUp, "colorBtnUp");
            this.colorBtnUp.Name = "colorBtnUp";
            this.colorBtnUp.UseVisualStyleBackColor = true;
            // 
            // colorBtnDown
            // 
            resources.ApplyResources(this.colorBtnDown, "colorBtnDown");
            this.colorBtnDown.Name = "colorBtnDown";
            this.colorBtnDown.UseVisualStyleBackColor = true;
            // 
            // picGroupBox
            // 
            resources.ApplyResources(this.picGroupBox, "picGroupBox");
            this.picGroupBox.Controls.Add(this.picBtnUp);
            this.picGroupBox.Controls.Add(this.picBtnDown);
            this.picGroupBox.Name = "picGroupBox";
            this.picGroupBox.TabStop = false;
            // 
            // picBtnUp
            // 
            resources.ApplyResources(this.picBtnUp, "picBtnUp");
            this.picBtnUp.Name = "picBtnUp";
            this.picBtnUp.UseVisualStyleBackColor = true;
            this.picBtnUp.Click += new System.EventHandler(this.picBtnUp_Click);
            // 
            // picBtnDown
            // 
            resources.ApplyResources(this.picBtnDown, "picBtnDown");
            this.picBtnDown.Name = "picBtnDown";
            this.picBtnDown.UseVisualStyleBackColor = true;
            this.picBtnDown.Click += new System.EventHandler(this.picBtnDown_Click);
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
            // SVBtnBackGroundWindow
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picGroupBox);
            this.Controls.Add(this.colorGroupBox);
            this.MaximizeBox = false;
            this.Name = "SVBtnBackGroundWindow";
            this.colorGroupBox.ResumeLayout(false);
            this.picGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 单击确定窗口对应的事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void okBtn_Click(object sender, System.EventArgs e)
        {
            if (colorGroupBox.checkEnabled())
            {
                _button.Attrib.BackColorgroundDown = colorBtnDown.BackColor;
                _button.Attrib.BackColorground = colorBtnUp.BackColor;
                _button.Attrib.IsShowPic = false;
            }
            else
            {
                _button.Attrib.IsShowPic = true;
            }

            _button.refreshPropertyToPanel();
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 单击取消窗口对应的事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void cancelBtn_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        /// <summary>
        /// 图片按钮按下事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void picBtnDown_Click(object sender, System.EventArgs e)
        {
            SVBitmapManagerWindow window = new SVBitmapManagerWindow();
            if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                String file = Path.Combine(SVProData.IconPath, window.SvBitMap.ImageFileName);
                setButtonBackGd(picBtnDown, file);
                _button.Attrib.BtnDownPic = window.SvBitMap;               
            }
        }

        /// <summary>
        /// 图片按钮弹起事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void picBtnUp_Click(object sender, System.EventArgs e)
        {
            SVBitmapManagerWindow window = new SVBitmapManagerWindow();
            if (window.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                String file = Path.Combine(SVProData.IconPath, window.SvBitMap.ImageFileName);
                setButtonBackGd(picBtnUp, file);
                _button.Attrib.BtnUpPic = window.SvBitMap;
            }
        }

        /// <summary>
        /// 从当前图元文件获取背景图片数据设置按钮背景
        /// </summary>
        /// <param Name="button">要设置的按钮对象</param>
        /// <param Name="file">当前图片文件</param>
        private void setButtonBackGd(Button button, String file)
        {
            if (!File.Exists(file))
                return;

            SVPixmapFile pixmapFile = new SVPixmapFile();
            pixmapFile.readPixmapFile(file);
            button.BackgroundImageLayout = ImageLayout.Zoom;
            button.BackgroundImage = pixmapFile.getBitmapFromData();
        }
    }
}
