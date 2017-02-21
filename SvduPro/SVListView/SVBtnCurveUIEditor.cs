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
            SVButton svPanel = context.Instance as SVButton;
            if (svPanel == null)
                return value;

            ///获取当前页面中的所以趋势图
            List<SVCurveProperties> attribList = new List<SVCurveProperties>();
            foreach (var sv in svPanel.Parent.Controls)
            {
                SVCurve curve = sv as SVCurve;
                if (curve != null)
                    attribList.Add(curve.Attrib);
            }

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl variableDialog = new SVWpfControl();
                variableDialog.Width = 100;
                variableDialog.Height = 100;

                SVWPFCurveSelect curveDialog = new SVWPFCurveSelect();
                curveDialog.listView.ItemsSource = attribList;
                variableDialog.addContent(curveDialog);
                edSvc.DropDownControl(variableDialog);

                SVCurveProperties obj = curveDialog.listView.SelectedItem as SVCurveProperties;
                if (obj != null)
                {
                    switch (svPanel.Attrib.ButtonType)
                    {
                        case 6:
                            svPanel.Attrib.BtnVarText = obj.ForwardControl;
                            break;
                        case 7:
                            svPanel.Attrib.BtnVarText = obj.CurControl;
                            break;
                        case 8:
                            svPanel.Attrib.BtnVarText = obj.BackwardControl;
                            break;
                    }

                    return obj.ID.ToString();
                }
            }

            return value;
        }
    }
}
