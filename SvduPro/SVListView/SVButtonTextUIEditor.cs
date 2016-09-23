using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;
using SVCore;

namespace SVControl
{
    public class SVButtonTextUIEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            SVButton svButton = context.Instance as SVButton;
            if (svButton == null)
                return value;

            try
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    TextBox textBox = new TextBox();
                    textBox.Multiline = true;
                    textBox.Text = svButton.Attrib.Text;
                    textBox.Size = new Size(textBox.Width, 100);
                    edSvc.DropDownControl(textBox);

                    return textBox.Text;
                }
            }
            catch (Exception ex)
            {
                SVLog.TextLog.Exception(ex);
                return value;
            }

            return value;
        }
    }
}
