using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    /// <summary>
    /// 按钮备注的编辑页面
    /// </summary>
    public class SVButtonMemoUIEditor : UITypeEditor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param oldName="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 进入编辑按钮备注的对话框
        /// </summary>
        /// <param oldName="context"></param>
        /// <param oldName="provider"></param>
        /// <param oldName="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
    System.IServiceProvider provider, object value)
        {
            ///确保操作的对象为按钮控件，其他对象不能使用该类进行包装
            SVButton svButton = context.Instance as SVButton;
            if (svButton == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl textDialog = new SVWpfControl();
                textDialog.Width = 200;
                textDialog.Height = 120;

                SVWPFBtnMemoEdit edit = new SVWPFBtnMemoEdit();
                edit.textBox.DataContext = svButton.Attrib;
                textDialog.addContent(edit);
                edSvc.DropDownControl(textDialog);

                return value;
            }

            return value;
        }
    }
}
