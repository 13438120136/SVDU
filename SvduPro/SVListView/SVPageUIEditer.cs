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
            //从当前对象中获取按钮控件对象
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
    System.IServiceProvider provider, object value)
        {
            //从当前对象中获取按钮控件对象
            SVPageWidget page = context.Instance as SVPageWidget;
            if (page == null)
                return null;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl textDialog = new SVWpfControl();
                textDialog.Width = 200;
                textDialog.Height = 300;

                WPFUser edit = new WPFUser();
                edit.DataContext = page.Attrib;

                textDialog.addContent(edit);
                edSvc.DropDownControl(textDialog);

                page.refreshPropertyToPanel();
            }

            return value;
        }
    }
}
