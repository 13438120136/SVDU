using System;
using System.Windows.Forms;

namespace SVCore
{
    public class SVColorButton : Button
    {
        public SVColorButton()
        {
            this.Click += new EventHandler(SVColorButton_Click);
        }

        void SVColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.BackColor = dialog.Color;
            }
        }
    }
}
