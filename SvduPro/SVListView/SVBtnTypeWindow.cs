using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using SVCore;

namespace SVControl
{
    /// <summary>
    /// 设置按钮控件动作的弹出窗口
    /// </summary>
    public partial class SVBtnDoWindow : Form
    {
        SVButton _svButton;

        /// <summary>
        /// 按钮动作窗口的构造函数
        /// </summary>
        /// <param Name="button">按钮控件</param>
        public SVBtnDoWindow(SVButton button)
        {
            _svButton = button;

            InitializeComponent();
            intializeWindow(); 
        }

        /// <summary>
        /// 根据当前按钮的值的状态来初始化窗口中内容
        /// </summary>
        void intializeWindow()
        {
            this.doType.SelectedIndex = _svButton.Attrib.BtnType.Type;
            if (this.doType.SelectedIndex == 0)
            {
                this.pageID.Text = _svButton.Attrib.BtnType.PageID.ToString();
                this.pageText.Text = _svButton.Attrib.BtnType.PageText;
                textGroupBox.Enabled = false;
            }
            else if (this.doType.SelectedIndex == 3)
            {
                textGroupBox.Enabled = true;
                this.varText.Text = _svButton.Attrib.BtnType.VarText;
                falseTextBox.Text = _svButton.Attrib.FText;
            }
            else
            {
                this.varText.Text = _svButton.Attrib.BtnType.VarText;
                textGroupBox.Enabled = false;
            }

            this.groupBoxEnabled.init();
            this.groupBoxEnabled.setEnabledText("使能");
            this.groupBoxEnabled.setChecked(_svButton.Attrib.BtnType.Enable);
            this.enText.Text = _svButton.Attrib.BtnType.EnVarText;
        }

        /// <summary>
        /// 单击确认按钮后执行的操作
        /// </summary>
        /// <param Name="sender">按钮对象</param>
        /// <param Name="e">按钮事件</param>
        private void okBtn_Click(object sender, EventArgs e)
        {
            if (!valid())
                return;

            _svButton.Attrib.FText = falseTextBox.Text;
            _svButton.RedoUndo.operChanged();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 单击取消按钮后窗口执行的操作
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        /// <summary>
        /// 当前类型选择Combobox改变后触发的事件处理
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void doType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.doType.SelectedIndex;
            switch (index)
            {
                case 0:
                    {
                        groupBox4.Enabled = true;
                        groupBox5.Enabled = false;
                        textGroupBox.Enabled = false;
                    }
                    break;
                case 3:
                    {
                        groupBox4.Enabled = false;
                        groupBox5.Enabled = true;
                        textGroupBox.Enabled = true;
                    }
                    break;
                default:
                    {
                        groupBox4.Enabled = false;
                        groupBox5.Enabled = true;
                        textGroupBox.Enabled = false; 
                    }
                    break;
            }
        }

        /// <summary>
        /// 判断当前的设置是否合法
        /// </summary>
        /// <returns>true-合法  false-不合法</returns>
        Boolean valid()
        {
            Boolean bResult = false;

            if (this.groupBoxEnabled.checkEnabled())
            {
                if (String.IsNullOrEmpty(this.enText.Text))
                {
                    bResult = false;
                    MessageBox.Show("未选择使能变量!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return bResult;
                }
            }

            int index = this.doType.SelectedIndex;
            if (index == 0)
                checkPageGoto(ref bResult);
            else
                checkVar(ref bResult);

            _svButton.Attrib.BtnType.Enable = this.groupBoxEnabled.checkEnabled();
            _svButton.Attrib.BtnType.EnVarText = this.enText.Text;

            return bResult;
        }

        /// <summary>
        /// 检查当前跳转页面是否已经设置
        /// </summary>
        /// <param Name="bResult">true-合法 false-不合法</param>
        void checkPageGoto(ref Boolean bResult)
        {
            if (String.IsNullOrEmpty(this.pageID.Text) 
                || String.IsNullOrEmpty(this.pageText.Text))
            {
                bResult = false;
                MessageBox.Show("没有选择对应的页面!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ///检查合法，将当前按钮中的值修改
            int index = this.doType.SelectedIndex;
            _svButton.Attrib.BtnType.Type = (Byte)index;
            _svButton.Attrib.BtnType.PageID = UInt16.Parse(this.pageID.Text);
            _svButton.Attrib.BtnType.PageText = pageText.Text;
            bResult = true;
        }

        /// <summary>
        /// 检查当前变量的选取是否合法
        /// </summary>
        /// <param Name="bResult">true-合法 false-不合法</param>
        void checkVar(ref Boolean bResult)
        {
            if (String.IsNullOrEmpty(this.varText.Text))
            {
                bResult = false;
                MessageBox.Show("没有选择操作变量!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = this.doType.SelectedIndex;
            _svButton.Attrib.BtnType.Type = (Byte)index;
            _svButton.Attrib.BtnType.VarText = this.varText.Text;
            bResult = true;
        }

        /// <summary>
        /// 弹出选择页面的窗口，并执行选择操作
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void pageBtn_Click(object sender, EventArgs e)
        {
            SVPageSelectWindow win = new SVPageSelectWindow();
            if (win.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                pageText.Text = win.getPageText();
                pageID.Text = win.getPageID().ToString();
            }
        }

        /// <summary>
        /// 选择使能关联变量
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void enVarBtn_Click(object sender, EventArgs e)
        {
            SVVarWindow win = new SVVarWindow();
            //win.setFilter(new List<String> { "BOOL", "BOOL_VAR" });
            if (win.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                _svButton.Attrib.BtnType.EnVarTextType = win.getVarType();
                enText.Text = win.varText();
            }
        }

        /// <summary>
        /// 选择按钮操作变量
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void varBtn_Click(object sender, EventArgs e)
        {
            int index = this.doType.SelectedIndex;

            SVVarWindow win = new SVVarWindow();

            if (index == 1 || index == 2)
                win.setFilter(new List<String> { "BOOL", "BOOL_VAR" });
            else
                win.setFilter(new List<String> {"BOOL_VAR" });

            if (win.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                _svButton.Attrib.BtnType.VarTextType = win.getVarType();
                varText.Text = win.varText();
            }
        }
    }
}
