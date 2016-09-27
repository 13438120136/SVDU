using SVCore;

namespace SVControl
{
    partial class SVBinaryTypeWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.customGroupBox = new SVCore.SVCheckGroupBox();
            this.textBoxFalse = new System.Windows.Forms.TextBox();
            this.textBoxTrue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.customGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "类型选择";
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "打开:关闭",
            "运行:停止",
            "1:0",
            "是:否",
            "真:假",
            "正确:错误",
            "开:关"});
            this.comboBoxType.Location = new System.Drawing.Point(107, 26);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(121, 20);
            this.comboBoxType.TabIndex = 1;
            // 
            // customGroupBox
            // 
            this.customGroupBox.Controls.Add(this.textBoxFalse);
            this.customGroupBox.Controls.Add(this.textBoxTrue);
            this.customGroupBox.Controls.Add(this.label3);
            this.customGroupBox.Controls.Add(this.label2);
            this.customGroupBox.Location = new System.Drawing.Point(25, 66);
            this.customGroupBox.Name = "customGroupBox";
            this.customGroupBox.Size = new System.Drawing.Size(283, 89);
            this.customGroupBox.TabIndex = 2;
            this.customGroupBox.TabStop = false;
            this.customGroupBox.Text = "groupBox1";
            // 
            // textBoxFalse
            // 
            this.textBoxFalse.Location = new System.Drawing.Point(116, 51);
            this.textBoxFalse.Name = "textBoxFalse";
            this.textBoxFalse.Size = new System.Drawing.Size(133, 21);
            this.textBoxFalse.TabIndex = 2;
            // 
            // textBoxTrue
            // 
            this.textBoxTrue.Location = new System.Drawing.Point(114, 21);
            this.textBoxTrue.Name = "textBoxTrue";
            this.textBoxTrue.Size = new System.Drawing.Size(135, 21);
            this.textBoxTrue.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "值为假文本信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "值为真文本信息";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(25, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(283, 2);
            this.label4.TabIndex = 3;
            this.label4.Text = "label4";
            // 
            // okBtn
            // 
            this.okBtn.Location = new System.Drawing.Point(152, 178);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "确定";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(234, 178);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 5;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // SVBinaryTypeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 226);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.customGroupBox);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SVBinaryTypeWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "开关量类型选择";
            this.customGroupBox.ResumeLayout(false);
            this.customGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxType;
        private SVCheckGroupBox customGroupBox;
        private System.Windows.Forms.TextBox textBoxFalse;
        private System.Windows.Forms.TextBox textBoxTrue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}