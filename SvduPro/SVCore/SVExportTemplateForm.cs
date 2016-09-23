using System;
using System.Windows.Forms;
using System.IO;

namespace SVCore
{
    public partial class SVExportTemplateForm : Form
    {
        public String TemplateFile { get; set; }

        public SVExportTemplateForm()
        {
            InitializeComponent();

            //限制输入模板名称长度
            this.textBox.MaxLength = 32;

            this.textBox.KeyPress += new KeyPressEventHandler((sender, e) =>
            {
                //if (e.KeyChar == '\b')
                //    return;

                //String str = this.textBox.Text + e.KeyChar;
                //if (!Regex.IsMatch(str, "^[_a-zA-Z][_a-zA-Z0-9]*$"))
                //    e.Handled = true;
                //else
                //    e.Handled = false;
            });

            this.okBtn.Click += new EventHandler((sender, e) =>
            {
                if (String.IsNullOrEmpty(textBox.Text))
                {
                    MessageBox.Show("模板名称不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                String file = Path.Combine(SVProData.TemplatePath, textBox.Text);
                if (File.Exists(file))
                {
                    MessageBox.Show("模板已经存在!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                TemplateFile = file;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            });

            this.cancelBtn.Click += new EventHandler((sender, e) =>
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            });
        }
    }
}
