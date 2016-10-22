using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SVControl
{
    public partial class SVBinaryTypeWindow : Form
    {
        SVBinary _binary;

        public SVBinaryTypeWindow(SVBinary binary)
        {
            InitializeComponent();
            _binary = binary;

            this.customGroupBox.init();
            this.customGroupBox.setEnabledText("自定义类型");

            if (binary.Attrib.Type != 7)
            {
                this.comboBoxType.SelectedIndex = binary.Attrib.Type;
                customGroupBox.setChecked(false);
            }
            else
            {
                this.textBoxTrue.Text = binary.Attrib.CustomTrueText;
                this.textBoxFalse.Text = binary.Attrib.CustomFlaseText;
                customGroupBox.setChecked(true);
            }

            this.customGroupBox.EnabledChanged += new EventHandler(customGroupBox_EnabledChanged);
        }

        /// <summary>
        /// 状态切换
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void customGroupBox_EnabledChanged(object sender, EventArgs e)
        {
            comboBoxType.Enabled = !customGroupBox.Enabled;
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            if (customGroupBox.Enabled)
            {
                _binary.Attrib.Type = 7;
                _binary.Attrib.CustomTrueText = this.textBoxTrue.Text;
                _binary.Attrib.CustomFlaseText = this.textBoxFalse.Text;
            }
            else
            {
                _binary.Attrib.Type = (Byte)this.comboBoxType.SelectedIndex;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
