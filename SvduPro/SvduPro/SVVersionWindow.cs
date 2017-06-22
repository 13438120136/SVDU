using System;
using System.Windows.Forms;
using SVCore;
using System.Text;

namespace SvduPro
{
    public partial class SVVersionWindow : Form
    {
        public SVVersionWindow()
        {
            InitializeComponent();
            textBox.Text = SVProData.version;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            String ver = textBox.Text;
            if (String.IsNullOrWhiteSpace(ver))
            {
                MessageBox.Show("当前版本不能为空", "提示", MessageBoxButtons.OK);
                return;
            }

            if (ver.Length > 8)
            {
                MessageBox.Show("字符串长度不能大于8", "提示", MessageBoxButtons.OK);
                return;
            }

            SVProData.version = ver;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
