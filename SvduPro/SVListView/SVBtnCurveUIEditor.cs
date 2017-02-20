using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVBtnCurveUIEditor : UITypeEditor
    {
        /// <summary>
        /// 选择开关量变量
        /// </summary>
        /// <param Name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl variableDialog = new SVWpfControl();
                variableDialog.Width = 180;
                variableDialog.Height = 200;

                SVWPFCurveSelect curveDialog = new SVWPFCurveSelect();
                variableDialog.addContent(curveDialog);
                edSvc.DropDownControl(variableDialog);
            }

            return value;
        }
    }
}
