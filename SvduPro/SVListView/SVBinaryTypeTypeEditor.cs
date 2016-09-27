using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVBinaryTypeTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 选择开关量类型
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
                SVBinary binary = context.Instance as SVBinary;
                if (binary == null)
                    return null;

                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (edSvc != null)
                {
                    SVBinaryTypeWindow window = new SVBinaryTypeWindow(binary);
                    if (edSvc.ShowDialog(window) == System.Windows.Forms.DialogResult.Yes)
                        return binary.Attrib.Type;
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
