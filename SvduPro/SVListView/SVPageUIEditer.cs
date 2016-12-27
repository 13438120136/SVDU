using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using SVCore;

namespace SVControl
{
    public class SVPageUIEditer : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
    System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl textDialog = new SVWpfControl();
                textDialog.Width = 200;
                textDialog.Height = 300;

                WPFUser edit = new WPFUser();
                textDialog.addContent(edit);
                edSvc.DropDownControl(textDialog);
            }

            return value;
        }
    }
}
