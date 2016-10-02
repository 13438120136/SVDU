using System.Windows.Forms;
using System;

namespace SVCore
{
    /// <summary>
    /// 由于系统的MessageBox中英文切换过于繁琐。
    /// 
    /// 这里自定义类
    /// </summary>
    public partial class SVMessageBox : Form
    {
        public SVMessageBox()
        {
            InitializeComponent();

            this.yesBtn.Text = Resource.确定;
            this.noBtn.Text = Resource.取消;
            this.ControlBox = false;
        }
        
        /// <summary>
        /// 设置显示的标题以及内容
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="text">文本内容</param>
        public void content(String title, String text)
        {
            this.Text = title;
            this.label.Text = text;
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void yesBtn_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }
        
        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noBtn_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }
    }
}
