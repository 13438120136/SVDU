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
                    SVMessageBox msgBox = new SVMessageBox();
                    msgBox.content(Resource.提示, Resource.模板名称不能为空);
                    msgBox.Show();

                    return;
                }

                String file = Path.Combine(SVProData.TemplatePath, textBox.Text);
                if (File.Exists(file))
                {
                    SVMessageBox msgBox = new SVMessageBox();
                    msgBox.content(Resource.提示, Resource.模板已经存在);
                    msgBox.Show();

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
