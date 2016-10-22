namespace SVSimulation
{
    partial class SVSBitnaryControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param Name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
            this.checkBoxTrue = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox
            // 
            this.checkBox.AutoSize = true;
            this.checkBox.Checked = true;
            this.checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox.Location = new System.Drawing.Point(42, 29);
            this.checkBox.Name = "checkBox";
            this.checkBox.Size = new System.Drawing.Size(72, 16);
            this.checkBox.TabIndex = 1;
            this.checkBox.Text = "自动播放";
            this.checkBox.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrue
            // 
            this.checkBoxTrue.AutoSize = true;
            this.checkBoxTrue.Location = new System.Drawing.Point(42, 64);
            this.checkBoxTrue.Name = "checkBoxTrue";
            this.checkBoxTrue.Size = new System.Drawing.Size(36, 16);
            this.checkBoxTrue.TabIndex = 2;
            this.checkBoxTrue.Text = "真";
            this.checkBoxTrue.UseVisualStyleBackColor = true;
            // 
            // SVSBitnaryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxTrue);
            this.Controls.Add(this.checkBox);
            this.Name = "SVSBitnaryControl";
            this.Size = new System.Drawing.Size(200, 177);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox;
        private System.Windows.Forms.CheckBox checkBoxTrue;
    }
}
