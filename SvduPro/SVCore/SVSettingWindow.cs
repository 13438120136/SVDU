using System;
using System.Windows.Forms;

namespace SVCore
{
    public partial class SVSettingWindow : Form
    {
        public SVSettingWindow()
        {
            InitializeComponent();

            SVConfig config = SVConfig.instance();
            config.loadConfig();

            this.textBox2.Text = config.SaveInterval.ToString();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            SVConfig config = SVConfig.instance();
            //config.RectCount = Int32.Parse(this.textBox1.Text);
            config.SaveInterval = Int32.Parse(this.textBox2.Text);
            config.saveConfig();

            DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }
}
