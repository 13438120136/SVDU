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
                    SVCurveVarWindow varWindow = new SVCurveVarWindow();
                    varWindow.VarArray = curve.Attrib.getVarArray();
                    varWindow.ColorArray = curve.Attrib.getColorArray();
                    varWindow.LineWidthArray = curve.Attrib.getLineWidthArray();

                    edSvc.DropDownControl(varWindow);
                    curve.Attrib.setVarArray(varWindow.VarArray);
                    curve.Attrib.setColorArray(varWindow.ColorArray);
                    curve.Attrib.setLineWidthArray(varWindow.LineWidthArray);
                    return String.Empty;
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
