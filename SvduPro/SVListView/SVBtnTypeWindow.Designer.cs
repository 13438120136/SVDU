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
        /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVBtnDoWindow));
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBoxEnabled = new SVCore.SVCheckGroupBox();
            this.enVarBtn = new System.Windows.Forms.Button();
            this.enText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
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
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // groupBoxEnabled
            // 
            this.groupBoxEnabled.Controls.Add(this.enVarBtn);
            this.groupBoxEnabled.Controls.Add(this.enText);
            this.groupBoxEnabled.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBoxEnabled, "groupBoxEnabled");
            this.groupBoxEnabled.Name = "groupBoxEnabled";
            this.groupBoxEnabled.TabStop = false;
            // 
            // enVarBtn
            // 
            resources.ApplyResources(this.enVarBtn, "enVarBtn");
            this.enVarBtn.Name = "enVarBtn";
            this.enVarBtn.UseVisualStyleBackColor = true;
            this.enVarBtn.Click += new System.EventHandler(this.enVarBtn_Click);
            // 
            // enText
            // 
            resources.ApplyResources(this.enText, "enText");
            this.enText.Name = "enText";
            this.enText.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.varBtn);
            this.groupBox5.Controls.Add(this.varText);
            this.groupBox5.Controls.Add(this.label5);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // varBtn
            // 
            resources.ApplyResources(this.varBtn, "varBtn");
            this.varBtn.Name = "varBtn";
            this.varBtn.UseVisualStyleBackColor = true;
            this.varBtn.Click += new System.EventHandler(this.varBtn_Click);
            // 
            // varText
            // 
            resources.ApplyResources(this.varText, "varText");
            this.varText.Name = "varText";
            this.varText.ReadOnly = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pageBtn);
            this.groupBox4.Controls.Add(this.pageID);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.pageText);
            this.groupBox4.Controls.Add(this.label3);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // pageBtn
            // 
            resources.ApplyResources(this.pageBtn, "pageBtn");
            this.pageBtn.Name = "pageBtn";
            this.pageBtn.UseVisualStyleBackColor = true;
            this.pageBtn.Click += new System.EventHandler(this.pageBtn_Click);
            // 
            // pageID
            // 
            resources.ApplyResources(this.pageID, "pageID");
            this.pageID.Name = "pageID";
            this.pageID.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // pageText
            // 
            this.pageText.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.pageText, "pageText");
            this.pageText.Name = "pageText";
            this.pageText.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // doType
            // 
            this.doType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.doType, "doType");
            this.doType.FormattingEnabled = true;
            this.doType.Items.AddRange(new object[] {
            resources.GetString("doType.Items"),
            resources.GetString("doType.Items1"),
            resources.GetString("doType.Items2"),
            resources.GetString("doType.Items3"),
            resources.GetString("doType.Items4")});
            this.doType.Name = "doType";
            this.doType.SelectedIndexChanged += new System.EventHandler(this.doType_SelectedIndexChanged);
            // 
            // SVBtnDoWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "SVBtnDoWindow";
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
        private System.Windows.Forms.Label label1;
        private SVCheckGroupBox groupBoxEnabled;
        private System.Windows.Forms.Button enVarBtn;
        private System.Windows.Forms.TextBox enText;
        private System.Windows.Forms.Label label6;
    }
}