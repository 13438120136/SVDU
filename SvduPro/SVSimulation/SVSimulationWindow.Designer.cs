namespace SVSimulation
{
    partial class SVSimulationWindow
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.dataPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Location = new System.Drawing.Point(12, 12);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(163, 212);
            this.mainPanel.TabIndex = 0;
            // 
            // dataPanel
            // 
            this.dataPanel.Location = new System.Drawing.Point(182, 12);
            this.dataPanel.Name = "dataPanel";
            this.dataPanel.Size = new System.Drawing.Size(113, 212);
            this.dataPanel.TabIndex = 1;
            // 
            // SVSimulationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 241);
            this.Controls.Add(this.dataPanel);
            this.Controls.Add(this.mainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SVSimulationWindow";
            this.Text = "仿真窗口";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel dataPanel;
    }
}