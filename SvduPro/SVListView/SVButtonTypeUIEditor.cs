using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;
using System;

namespace SVControl
{
    public class SVButtonTypeUIEditor : UITypeEditor
    {
                /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        /// <summary>
        /// 进入编辑按钮备注的对话框
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl btnTypeDialog = new SVWpfControl();
                btnTypeDialog.Width = 100;
                btnTypeDialog.Height = 180;

                SVWPFButtonType control = new SVWPFButtonType();
                btnTypeDialog.addContent(control);
                control.listView.DataContext = value;
                edSvc.DropDownControl(btnTypeDialog);

                if (control.listView.SelectedIndex != -1)
                    value = (Byte)control.listView.SelectedIndex;

                return value;
            }

            return value;
        }
    }
}
