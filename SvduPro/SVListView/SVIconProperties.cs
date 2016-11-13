using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVIconProperties
    {
        UInt16 _id;          //文本ID
        Rectangle _rect;     //按钮控件尺寸        
        SVBitmap _iconData;      //icon的数据
        String _controlType;
        Boolean _isLock;

        public UpdateControl UpdateControl;

        public SVIconProperties()
        {
            _rect = new Rectangle(0, 0, 100, 100);
            _iconData = new SVBitmap();
            _controlType = "静态图";
            _isLock = false;
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DisplayName("控件")]
        [DescriptionAttribute("控件类型显示")]
        public String CtrlType
        {
            get { return _controlType; }
            set { _controlType = value; }
        }

        [CategoryAttribute("属性")]
        [DescriptionAttribute("是否锁定当前控件?\nTrue锁定,False不锁定")]
        [DisplayName("锁定")]
        public Boolean Lock
        {
            set
            {
                if (_isLock == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Boolean before = _isLock;
                undoItem.ReDo = () =>
                {
                    _isLock = value;
                };
                undoItem.UnDo = () =>
                {
                    _isLock = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _isLock = value;
            }

            get
            {
                return _isLock;
            }
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("图标ID")]
        public UInt16 ID
        {
            set
            {
                _id = value;
            }

            get
            {
                return _id;
            }
        }

        [CategoryAttribute("数据")]
        [DescriptionAttribute("设置静态图中需要显示的图片")]
        [TypeConverter(typeof(SVBitmap))]
        [EditorAttribute(typeof(SVBitmapTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]  
        [DisplayName("图片")]
        public SVBitmap PicIconData
        {
            set
            {
                if (_iconData == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                SVBitmap before = _iconData;
                undoItem.ReDo = () =>
                {
                    _iconData = value;
                };
                undoItem.UnDo = () =>
                {
                    _iconData = before;
                };

                _iconData = value;
            }

            get
            {
                return _iconData;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("图标尺寸")]
        [DisplayName("位置")]
        public Rectangle Rect
        {
            set
            {
                if (_rect == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Rectangle before = _rect;
                undoItem.ReDo = () =>
                {
                    _rect = value;
                };
                undoItem.UnDo = () =>
                {
                    _rect = before;
                };

                _rect = value;
            }

            get
            {
                return _rect;
            }
        }

        public void make(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            UInt32 pageCount = pageArrayBin.pageCount;
            UInt32 curveCount = pageArrayBin.pageArray[pageCount].iconNum++;

            if (pageArrayBin.pageArray[pageCount].m_icon == null)
                pageArrayBin.pageArray[pageCount].m_icon = new IconBin[SVLimit.PAGE_ICON_MAXNUM];

            IconBin iconBin = pageArrayBin.pageArray[pageCount].m_icon[curveCount];

            iconBin.id = ID;
            iconBin.rect.sX = (UInt16)Rect.X;
            iconBin.rect.sY = (UInt16)Rect.Y;
            iconBin.rect.eX = (UInt16)(Rect.Width + iconBin.rect.sX - 1);
            iconBin.rect.eY = (UInt16)(Rect.Height + iconBin.rect.sY - 1);

            var address = PicIconData.bitmap8Data(Rect.Width, Rect.Height);
            if (address != null)
            {
                //图标位置
                iconBin.imageOffset = (UInt32)serialize.ToArray().Length;
                serialize.pack(address);             
            }

            pageArrayBin.pageArray[pageCount].m_icon[curveCount] = iconBin;
        }
    }
}
