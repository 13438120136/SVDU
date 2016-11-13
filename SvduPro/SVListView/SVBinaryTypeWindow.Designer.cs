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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVBinaryTypeWindow));
            this.customGroupBox = new SVCore.SVCheckGroupBox();
            this.textBoxFalse = new System.Windows.Forms.TextBox();
            this.textBoxTrue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.picGroupBox = new SVCore.SVCheckGroupBox();
            this.exPic = new System.Windows.Forms.Button();
            this.falsePic = new System.Windows.Forms.Button();
            this.truePic = new System.Windows.Forms.Button();
            this.customGroupBox.SuspendLayout();
            this.picGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // customGroupBox
            // 
            this.customGroupBox.Controls.Add(this.textBoxFalse);
            this.customGroupBox.Controls.Add(this.textBoxTrue);
            this.customGroupBox.Controls.Add(this.label3);
            this.customGroupBox.Controls.Add(this.label2);
            resources.ApplyResources(this.customGroupBox, "customGroupBox");
            this.customGroupBox.Name = "customGroupBox";
            this.customGroupBox.TabStop = false;
            // 
            // textBoxFalse
            // 
            resources.ApplyResources(this.textBoxFalse, "textBoxFalse");
            this.textBoxFalse.Name = "textBoxFalse";
            // 
            // textBoxTrue
            // 
            resources.ApplyResources(this.textBoxTrue, "textBoxTrue");
            this.textBoxTrue.Name = "textBoxTrue";
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
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
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
            // picGroupBox
            // 
            this.picGroupBox.Controls.Add(this.exPic);
            this.picGroupBox.Controls.Add(this.falsePic);
            this.picGroupBox.Controls.Add(this.truePic);
            resources.ApplyResources(this.picGroupBox, "picGroupBox");
            this.picGroupBox.Name = "picGroupBox";
            this.picGroupBox.TabStop = false;
            // 
            // exPic
            // 
            resources.ApplyResources(this.exPic, "exPic");
            this.exPic.Name = "exPic";
            this.exPic.UseVisualStyleBackColor = true;
            // 
            // falsePic
            // 
            resources.ApplyResources(this.falsePic, "falsePic");
            this.falsePic.Name = "falsePic";
            this.falsePic.UseVisualStyleBackColor = true;
            // 
            // truePic
            // 
            resources.ApplyResources(this.truePic, "truePic");
            this.truePic.Name = "truePic";
            this.truePic.UseVisualStyleBackColor = true;
            // 
            // SVBinaryTypeWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picGroupBox);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.customGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SVBinaryTypeWindow";
            this.customGroupBox.ResumeLayout(false);
            this.customGroupBox.PerformLayout();
            this.picGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SVCheckGroupBox customGroupBox;
        private System.Windows.Forms.TextBox textBoxFalse;
        private System.Windows.Forms.TextBox textBoxTrue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private SVCheckGroupBox picGroupBox;
        private System.Windows.Forms.Button exPic;
        private System.Windows.Forms.Button falsePic;
        private System.Windows.Forms.Button truePic;
    }
}