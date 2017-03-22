using System.Collections.Generic;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SVControl
{
    public class SVCurveVarTypeEditor : UITypeEditor
    {
        public override bool IsDropDownResizable { get { return true; } }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWPFCurveVar window = new SVWPFCurveVar();
                window.listView.ItemsSource = (List<SVCurveProper>)value;
                window.ShowDialog();
            }

            return value;
        }
    }
}
