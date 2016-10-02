namespace SVControl
{
    partial class SVVarWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVVarWindow));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.cacenlBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelVar = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dataGridView, "dataGridView");
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 23;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // okBtn
            // 
            resources.ApplyResources(this.okBtn, "okBtn");
            this.okBtn.Name = "okBtn";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cacenlBtn
            // 
            resources.ApplyResources(this.cacenlBtn, "cacenlBtn");
            this.cacenlBtn.Name = "cacenlBtn";
            this.cacenlBtn.UseVisualStyleBackColor = true;
            this.cacenlBtn.Click += new System.EventHandler(this.cacenlBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelVar);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.refreshBtn);
            this.groupBox2.Controls.Add(this.textBox);
            this.groupBox2.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // labelVar
            // 
            resources.ApplyResources(this.labelVar, "labelVar");
            this.labelVar.Name = "labelVar";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // refreshBtn
            // 
            resources.ApplyResources(this.refreshBtn, "refreshBtn");
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.UseVisualStyleBackColor = true;
            // 
            // textBox
            // 
            resources.ApplyResources(this.textBox, "textBox");
            this.textBox.Name = "textBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // SVVarWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cacenlBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SVVarWindow";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cacenlBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelVar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label label1;
    }
}