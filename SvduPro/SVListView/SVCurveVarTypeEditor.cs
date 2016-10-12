using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SVControl
{
    public class SVCurveVarTypeEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            try
            {
                SVCurve curve = context.Instance as SVCurve;
                if (curve == null)
                    return String.Empty; 

                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    SVCurveVarWindow varWindow = new SVCurveVarWindow(curve);
                    edSvc.DropDownControl(varWindow);
                    curve.RedoUndo.operChanged();

                    return "变量列表";
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("PropertyGridDateItem Error : " + ex.Message);
                return value;
            }

            return value;
        }
    }
}
