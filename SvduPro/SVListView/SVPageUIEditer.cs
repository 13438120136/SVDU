using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

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
                UserControl user = new UserControl();
                ElementHost host = new ElementHost();
                host.Dock = DockStyle.Fill;
                user.Controls.Add(host);
                user.Width = 400;
                user.Height = 300;
                host.Child = new WPFUser();
                edSvc.DropDownControl(user);
            }

            return value;
        }
    }
}
