using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
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

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl textDialog = new SVWpfControl();
                textDialog.Width = 200;
                textDialog.Height = 120;

                SVWPFBtnTextEdit edit = new SVWPFBtnTextEdit();
                edit.textBox.DataContext = svButton.Attrib;
                textDialog.addContent(edit);                
                edSvc.DropDownControl(textDialog);
                svButton.refreshPropertyToPanel();

                return value;
            }

            return value;
        }
    }
}
