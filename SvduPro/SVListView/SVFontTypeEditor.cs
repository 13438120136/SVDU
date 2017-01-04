using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVFontTypeEditor : UITypeEditor
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
                SVWpfControl fontDialog = new SVWpfControl();
                fontDialog.Width = 160;
                fontDialog.Height = 120;

                SVWPFFontDialog dialog = new SVWPFFontDialog();
                dialog.listView.SelectedValue = value;
                fontDialog.addContent(dialog);

                edSvc.DropDownControl(fontDialog);
                if (dialog.listView.SelectedValue != null)
                    value = dialog.listView.SelectedValue;

                return value;
            }

            return value;
        }
    }
}
