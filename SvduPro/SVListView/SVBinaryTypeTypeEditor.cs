using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SVControl
{
    public class SVBinaryTypeTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 选择开关量类型
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
            SVBinary binary = context.Instance as SVBinary;
            if (binary == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVBinaryTypeWindow window = new SVBinaryTypeWindow(binary);
                if (edSvc.ShowDialog(window) == System.Windows.Forms.DialogResult.Yes)
                    return binary.Attrib.Type;
            }

            return value;
        }
    }
}
