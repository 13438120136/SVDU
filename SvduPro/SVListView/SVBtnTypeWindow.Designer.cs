using SVCore;

namespace SVControl
{
    partial class SVBtnDoWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBoxEnabled = new SVCore.SVCheckGroupBox();
            this.enVarBtn = new System.Windows.Forms.Button();
            this.enText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.closeRadio = new System.Windows.Forms.RadioButton();
            this.openRadio = new System.Windows.Forms.RadioButton();
            this.varBtn = new System.Windows.Forms.Button();
            this.varText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pageBtn = new System.Windows.Forms.Button();
            this.pageID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pageText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.doType = new System.Windows.Forms.ComboBox();
            this.groupBox3.SuspendLayout();
            this.groupBoxEnabled.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(170, 383);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 7;
            this.okBtn.Text = "确定";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(245, 383);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 8;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBoxEnabled);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.doType);
            this.groupBox3.Controls.Add(this.cancelBtn);
            this.groupBox3.Controls.Add(this.okBtn);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(353, 424);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "按钮类型";
            // 
            // groupBoxEnabled
            // 
            this.groupBoxEnabled.Controls.Add(this.enVarBtn);
            this.groupBoxEnabled.Controls.Add(this.enText);
            this.groupBoxEnabled.Controls.Add(this.label6);
            this.groupBoxEnabled.Location = new System.Drawing.Point(27, 68);
            this.groupBoxEnabled.Name = "groupBoxEnabled";
            this.groupBoxEnabled.Size = new System.Drawing.Size(293, 63);
            this.groupBoxEnabled.TabIndex = 10;
            this.groupBoxEnabled.TabStop = false;
            // 
            // enVarBtn
            // 
            this.enVarBtn.Location = new System.Drawing.Point(192, 25);
            this.enVarBtn.Name = "enVarBtn";
            this.enVarBtn.Size = new System.Drawing.Size(75, 23);
            this.enVarBtn.TabIndex = 2;
            this.enVarBtn.Text = "选择变量";
            this.enVarBtn.UseVisualStyleBackColor = true;
            this.enVarBtn.Click += new System.EventHandler(this.enVarBtn_Click);
            // 
            // enText
            // 
            this.enText.Location = new System.Drawing.Point(78, 27);
            this.enText.Name = "enText";
            this.enText.ReadOnly = true;
            this.enText.Size = new System.Drawing.Size(96, 21);
            this.enText.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "使能变量";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(10, 366);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 2);
            this.label1.TabIndex = 9;
            this.label1.Text = "label1";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.closeRadio);
            this.groupBox5.Controls.Add(this.openRadio);
            this.groupBox5.Controls.Add(this.varBtn);
            this.groupBox5.Controls.Add(this.varText);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Location = new System.Drawing.Point(27, 252);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(290, 97);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "操作变量";
            // 
            // closeRadio
            // 
            this.closeRadio.AutoSize = true;
            this.closeRadio.Location = new System.Drawing.Point(144, 63);
            this.closeRadio.Name = "closeRadio";
            this.closeRadio.Size = new System.Drawing.Size(83, 16);
            this.closeRadio.TabIndex = 5;
            this.closeRadio.TabStop = true;
            this.closeRadio.Text = "False-无效";
            this.closeRadio.UseVisualStyleBackColor = true;
            // 
            // openRadio
            // 
            this.openRadio.AutoSize = true;
            this.openRadio.Location = new System.Drawing.Point(53, 63);
            this.openRadio.Name = "openRadio";
            this.openRadio.Size = new System.Drawing.Size(77, 16);
            this.openRadio.TabIndex = 4;
            this.openRadio.TabStop = true;
            this.openRadio.Text = "True-有效";
            this.openRadio.UseVisualStyleBackColor = true;
            // 
            // varBtn
            // 
            this.varBtn.Location = new System.Drawing.Point(207, 24);
            this.varBtn.Name = "varBtn";
            this.varBtn.Size = new System.Drawing.Size(75, 23);
            this.varBtn.TabIndex = 2;
            this.varBtn.Text = "选择变量";
            this.varBtn.UseVisualStyleBackColor = true;
            this.varBtn.Click += new System.EventHandler(this.varBtn_Click);
            // 
            // varText
            // 
            this.varText.Location = new System.Drawing.Point(93, 25);
            this.varText.Name = "varText";
            this.varText.ReadOnly = true;
            this.varText.Size = new System.Drawing.Size(100, 21);
            this.varText.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "变量名";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pageBtn);
            this.groupBox4.Controls.Add(this.pageID);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.pageText);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(27, 154);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(290, 89);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "关联页面";
            // 
            // pageBtn
            // 
            this.pageBtn.Location = new System.Drawing.Point(192, 47);
            this.pageBtn.Name = "pageBtn";
            this.pageBtn.Size = new System.Drawing.Size(75, 23);
            this.pageBtn.TabIndex = 7;
            this.pageBtn.Text = "选择页面";
            this.pageBtn.UseVisualStyleBackColor = true;
            this.pageBtn.Click += new System.EventHandler(this.pageBtn_Click);
            // 
            // pageID
            // 
            this.pageID.Location = new System.Drawing.Point(84, 49);
            this.pageID.Name = "pageID";
            this.pageID.ReadOnly = true;
            this.pageID.Size = new System.Drawing.Size(100, 21);
            this.pageID.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 53);
            this.label4.MinimumSize = new System.Drawing.Size(40, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "页面ID";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pageText
            // 
            this.pageText.BackColor = System.Drawing.SystemColors.Control;
            this.pageText.Location = new System.Drawing.Point(84, 20);
            this.pageText.Name = "pageText";
            this.pageText.ReadOnly = true;
            this.pageText.Size = new System.Drawing.Size(100, 21);
            this.pageText.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 24);
            this.label3.MinimumSize = new System.Drawing.Size(40, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "页面名称";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "动作";
            // 
            // doType
            // 
            this.doType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.doType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.doType.FormattingEnabled = true;
            this.doType.Items.AddRange(new object[] {
            "切换页面",
            "操作设备"});
            this.doType.Location = new System.Drawing.Point(80, 38);
            this.doType.Name = "doType";
            this.doType.Size = new System.Drawing.Size(121, 20);
            this.doType.TabIndex = 0;
            this.doType.SelectedIndexChanged += new System.EventHandler(this.doType_SelectedIndexChanged);
            // 
            // SVBtnDoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 424);
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "SVBtnDoWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "按钮行为设置";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxEnabled.ResumeLayout(false);
            this.groupBoxEnabled.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox doType;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pageText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pageID;
        private System.Windows.Forms.Button pageBtn;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button varBtn;
        private System.Windows.Forms.TextBox varText;
        private System.Windows.Forms.RadioButton closeRadio;
        private System.Windows.Forms.RadioButton openRadio;
        private System.Windows.Forms.Label label1;
        private SVCheckGroupBox groupBoxEnabled;
        private System.Windows.Forms.Button enVarBtn;
        private System.Windows.Forms.TextBox enText;
        private System.Windows.Forms.Label label6;
    }
}