using System;
using System.Drawing;
using System.Windows.Forms;

namespace SVCore
{
    /// <summary>
    /// 自定义的带有checkbox功能的GroupBox控件
    /// </summary>
    public class SVCheckGroupBox : GroupBox
    {
        CheckBox _checkBox = new CheckBox();

        /// <summary>
        /// 初始化操作
        /// </summary>
        public SVCheckGroupBox()
        {            
            init();
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        public void init()
        {
            if (this.Parent == null)
                return;

            Graphics gh = this.CreateGraphics();
            SizeF sizeF = gh.MeasureString(_checkBox.Text, _checkBox.Font);
            _checkBox.Size = new Size((int)sizeF.Width + 20, (int)sizeF.Height + 5);

            this.Parent.Controls.Add(_checkBox);

            _checkBox.Location = new Point(_checkBox.Left + this.Left + 10, _checkBox.Top + this.Top);
            _checkBox.BringToFront();

            this.Enabled = _checkBox.Checked;

            _checkBox.CheckedChanged += new EventHandler(_checkBox_EnabledChanged);
        }

        /// <summary>
        /// checkbox控件状态改变事件处理
        /// </summary>
        /// <param oldName="sender"></param>
        /// <param oldName="e"></param>
        void _checkBox_EnabledChanged(object sender, EventArgs e)
        {
            this.Enabled = _checkBox.Checked;
        }

        /// <summary>
        /// 获取当前控件的checkbox状态
        /// </summary>
        /// <returns>返回checkbox选中状态</returns>
        public Boolean checkEnabled()
        {
            return _checkBox.Checked;
        }

        /// <summary>
        /// 设置check控件的显示文本内容
        /// </summary>
        /// <param oldName="text">显示的文本</param>
        public void setEnabledText(String text)
        {
            _checkBox.Text = text;

            Graphics gh = this.CreateGraphics();
            SizeF sizeF = gh.MeasureString(_checkBox.Text, _checkBox.Font);
            _checkBox.Size = new Size((int)sizeF.Width + 20, (int)sizeF.Height + 5);
        }

        /// <summary>
        /// 设置当前的选中状态
        /// </summary>
        /// <param oldName="enabled">true-为选中， false-不选中</param>
        public void setChecked(Boolean enabled)
        {
            _checkBox.Checked = enabled;
        }
    }
}
