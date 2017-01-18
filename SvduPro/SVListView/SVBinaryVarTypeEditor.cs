using System;
using System.Collections.Generic;
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
                variableDialog.Width = 260;
                variableDialog.Height = 300;

                SVWPFVariableDialog dialog = new SVWPFVariableDialog();
                variableDialog.addContent(dialog);

                edSvc.DropDownControl(variableDialog);
            }

            //SVBinary bin = context.Instance as SVBinary;
            //if (bin == null)
            //    return value;

            //IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            //if (edSvc != null)
            //{
            //    SVVarWindow window = new SVVarWindow();
            //    window.setFilter(new List<String> { "BOOL", "BOOL_VAR" });
            //    if (edSvc.ShowDialog(window) == System.Windows.Forms.DialogResult.Yes)
            //    {
            //        bin.Attrib.VarType = window.getVarType();
            //        bin.RedoUndo.operChanged();
            //        return window.varText();
            //    }
            //}

            return value;
        }
    }
}
