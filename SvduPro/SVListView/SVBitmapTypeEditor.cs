using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using SVCore;
using System.Drawing;

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

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            SVBitmap bitmap = e.Value as SVBitmap;
            if (bitmap == null)
                return ;

            Rectangle rect = new Rectangle(1, 1, 19, 17);
            e.Graphics.DrawImage(bitmap.bitmap(), rect);
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            System.IServiceProvider provider, object value)
        {
            SVBitmap bitmap = value as SVBitmap;
            if (bitmap == null)
                return value;

            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                SVWpfControl picDialog = new SVWpfControl();
                picDialog.Width = 300;
                picDialog.Height = 300;

                SVWPfIconPic iconPicture = new SVWPfIconPic();
                iconPicture.image.DataContext = bitmap.ShowName;
                //iconPicture.DataContext = icon;
                picDialog.addContent(iconPicture);
                edSvc.DropDownControl(picDialog);
                value = iconPicture.resultBitmap();
                //icon.refreshPropertyToPanel();
                //icon.RedoUndo.operChanged();

                return value;
            }

            return value;
        }
    }
}
