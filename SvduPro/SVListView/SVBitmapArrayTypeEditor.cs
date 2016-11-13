using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace SVControl
{
    public class SVBitmapArrayTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 重载GetEditStyle
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 重载EditValue
        /// </summary>
        /// <param Name="context"></param>
        /// <param Name="provider"></param>
        /// <param Name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            SVHeartbeat heart = context.Instance as SVHeartbeat;
            if (heart == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVBitmapArrayWindow window = new SVBitmapArrayWindow(heart.Attrib.BitMapArray);
                window.ShowDialog();
                heart.refreshPropertyToPanel();
                heart.RedoUndo.operChanged();

                return window.bitmapArray();
            }

            return value;
        }
    }
}
