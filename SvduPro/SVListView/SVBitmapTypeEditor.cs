using System;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;

namespace SVControl
{
    public class SVBitmapTypeEditor : UITypeEditor
    {
        /// <summary>
        /// 选择图片的属性类
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
            SVIcon icon = context.Instance as SVIcon;
            if (icon == null)
                return null;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl picDialog = new SVWpfControl();
                picDialog.Width = 300;
                picDialog.Height = 300;

                SVWPfIconPic iconPicture = new SVWPfIconPic();
                iconPicture.image.DataContext = icon.Attrib.PicIconData.ShowName;
                iconPicture.DataContext = icon;
                picDialog.addContent(iconPicture);
                edSvc.DropDownControl(picDialog);
                icon.refreshPropertyToPanel();
                icon.RedoUndo.operChanged();

                return value;
            }

            return value;
        }
    }
}
