namespace SVControl
{
    partial class SVBitmapWindow 
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SVBitmapWindow));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.importPicBtn = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.classTreeView = new System.Windows.Forms.TreeView();
            this.newClassBtn = new System.Windows.Forms.Button();
            this.textBoxName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            // 
            // pictureBox
            // 
            resources.ApplyResources(this.pictureBox, "pictureBox");
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.TabStop = false;
            // 
            // importPicBtn
            // 
            resources.ApplyResources(this.importPicBtn, "importPicBtn");
            this.importPicBtn.Name = "importPicBtn";
            this.importPicBtn.UseVisualStyleBackColor = true;
            // 
            // listView
            // 
            resources.ApplyResources(this.listView, "listView");
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            // 
            // classTreeView
            // 
            resources.ApplyResources(this.classTreeView, "classTreeView");
            this.classTreeView.Name = "classTreeView";
            // 
            // newClassBtn
            // 
            resources.ApplyResources(this.newClassBtn, "newClassBtn");
            this.newClassBtn.Name = "newClassBtn";
            this.newClassBtn.UseVisualStyleBackColor = true;
            this.newClassBtn.Click += new System.EventHandler(this.newClassBtn_Click);
            // 
            // textBoxName
            // 
            resources.ApplyResources(this.textBoxName, "textBoxName");
            this.textBoxName.Name = "textBoxName";
            // 
            // SVBitmapWindow
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.newClassBtn);
            this.Controls.Add(this.classTreeView);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.importPicBtn);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.textBox1);
            this.Name = "SVBitmapWindow";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button importPicBtn;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.TreeView classTreeView;
        private System.Windows.Forms.Button newClassBtn;
        private System.Windows.Forms.TextBox textBoxName;

    }
}
