using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVBinaryVarTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 选择开关量变量
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
    System.IServiceProvider provider, object value)
        {
            try
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    SVVarWindow window = new SVVarWindow();
                    if (edSvc.ShowDialog(window) == System.Windows.Forms.DialogResult.Yes)
                        return window.varText();
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                SVLog.TextLog.Exception(ex);
            }

            return value;
        }
    }
}
