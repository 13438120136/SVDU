using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVAnalogVarTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 选择模拟量变量
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            var analog = context.Instance as SVAnalog;
            if (analog == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVVarWindow window = new SVVarWindow();
                window.setFilter(new List<String> { "SHORT_INT", "SHORTINT_VAR", "INT", "INT_VAR", "REAL", "REAL_VAR" });
                if (edSvc.ShowDialog(window) == System.Windows.Forms.DialogResult.Yes)
                {
                    analog.Attrib.VarType = window.getVarType();
                    analog.RedoUndo.operChanged();
                    return window.varText();
                }
            }

            return value;
        }
    }
}
