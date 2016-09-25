namespace SVSimulation
{
    partial class SVSHeartBeatControl
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
            this.checkBox = new System.Windows.Forms.CheckBox();
            this.numControl = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numControl)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Checked = true;
            this.checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox.Location = new System.Drawing.Point(37, 39);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(72, 16);
            this.checkBox.TabIndex = 0;
            this.checkBox.Text = "自动播放";
            this.checkBox.UseVisualStyleBackColor = true;
            // 
            // numControl
            // 
            this.numControl.Location = new System.Drawing.Point(119, 79);
            this.numControl.Name = "numControl";
            this.numControl.Size = new System.Drawing.Size(61, 21);
            this.numControl.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "当前桢图片";
            // 
            // SVSHeartBeatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numControl);
            this.Controls.Add(this.checkBox);
            this.Name = "SVSHeartBeatControl";
            this.Size = new System.Drawing.Size(200, 241);
            ((System.ComponentModel.ISupportInitialize)(this.numControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox;
        private System.Windows.Forms.NumericUpDown numControl;
        private System.Windows.Forms.Label label1;
    }
}
