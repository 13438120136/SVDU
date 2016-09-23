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
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(264, 31);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(95, 87);
            this.textBox1.TabIndex = 2;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(264, 124);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(95, 87);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            // 
            // importPicBtn
            // 
            this.importPicBtn.Location = new System.Drawing.Point(115, 244);
            this.importPicBtn.Name = "importPicBtn";
            this.importPicBtn.Size = new System.Drawing.Size(75, 23);
            this.importPicBtn.TabIndex = 4;
            this.importPicBtn.Text = "导入图片";
            this.importPicBtn.UseVisualStyleBackColor = true;
            // 
            // listView
            // 
            this.listView.Location = new System.Drawing.Point(135, 24);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(108, 187);
            this.listView.TabIndex = 5;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            // 
            // classTreeView
            // 
            this.classTreeView.Location = new System.Drawing.Point(25, 24);
            this.classTreeView.Name = "classTreeView";
            this.classTreeView.Size = new System.Drawing.Size(104, 187);
            this.classTreeView.TabIndex = 6;
            // 
            // newClassBtn
            // 
            this.newClassBtn.Location = new System.Drawing.Point(34, 244);
            this.newClassBtn.Name = "newClassBtn";
            this.newClassBtn.Size = new System.Drawing.Size(75, 23);
            this.newClassBtn.TabIndex = 7;
            this.newClassBtn.Text = "新建分类";
            this.newClassBtn.UseVisualStyleBackColor = true;
            this.newClassBtn.Click += new System.EventHandler(this.newClassBtn_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(196, 244);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(84, 21);
            this.textBoxName.TabIndex = 8;
            // 
            // SVBitmapWindow
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 303);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.newClassBtn);
            this.Controls.Add(this.classTreeView);
            this.Controls.Add(this.listView);
            this.Controls.Add(this.importPicBtn);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.textBox1);
            //this.MaximizeBox = false;
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
