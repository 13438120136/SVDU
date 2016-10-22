using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace SVControl
{
    public class SVLabelTextUIEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            SVLabel svLabel = context.Instance as SVLabel;
            if (svLabel == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                TextBox textBox = new TextBox();
                textBox.Multiline = true;
                textBox.Text = svLabel.Attrib.Text;
                textBox.Size = new Size(textBox.Width, 100);
                edSvc.DropDownControl(textBox);

                return textBox.Text;
            }

            return value;
        }
    }
}
