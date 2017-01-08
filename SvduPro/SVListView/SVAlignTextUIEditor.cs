using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    class SVAlignTextUIEditor : UITypeEditor
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
                SVWpfControl alignTextDialog = new SVWpfControl();
                alignTextDialog.Width = 120;
                alignTextDialog.Height = 120;

                SVWPFAlignDialog dialog = new SVWPFAlignDialog();
                alignTextDialog.addContent(dialog);
                dialog.CloseEvent += () => 
                {
                    edSvc.CloseDropDown();
                    value = dialog.AlignValue;
                };

                edSvc.DropDownControl(alignTextDialog);

                return value;
            }

            return value;
        }
    }
}
