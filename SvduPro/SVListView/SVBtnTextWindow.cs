using System;
using System.Windows.Forms;

namespace SVControl
{
    public partial class SVBtnTextWindow : Form
    {
        public SVBtnTextWindow()
        {
            InitializeComponent();
        }

        public void setText(String text)
        {
            this.textBox.Text = text;
        }

        public String getText()
        {
            return textBox.Text;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
