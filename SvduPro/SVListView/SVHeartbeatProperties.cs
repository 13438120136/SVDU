using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVHeartbeatProperties
    {
        Rectangle _rect;     //心跳控件尺寸
        UInt16 _id;          //文本ID
        String _controlType; //控件显示名称
        SVBitmapArray _bitMapArray;
        Boolean _isLock;

        public UpdateControl UpdateControl;

        public SVHeartbeatProperties()
        {
            _rect = new Rectangle(0, 0, 120, 120);
            _controlType = "心跳控件";
            _bitMapArray = new SVBitmapArray();
            _isLock = false;
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DisplayName("控件")]
        [DescriptionAttribute("心跳控件类型显示")]
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

        [CategoryAttribute("数据")]
        [DescriptionAttribute("设置心跳控件显示的图片,显示已设置图片的个数,最大个数为8")]
        [DisplayName("图片")]
        [EditorAttribute(typeof(SVBitmapArrayTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SVBitmapArray))]
        public SVBitmapArray BitMapArray
        {
            get { return _bitMapArray; }
            set 
            {
                _bitMapArray = value;
            }
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("心跳控件的ID")]
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

        [CategoryAttribute("外观")]
        [DescriptionAttribute("心跳控件尺寸")]
        [DisplayName("位置")]
        public Rectangle Rect
        {
            set
            {
                if (_rect == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Rectangle before = _rect;
                undoItem.ReDo = () =>
                {
                    _rect = value;
                };
                undoItem.UnDo = () =>
                {
                    _rect = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

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
            UInt32 heartCount = pageArrayBin.pageArray[pageCount].tickNum++;

            if (pageArrayBin.pageArray[pageCount].m_tick == null)
                pageArrayBin.pageArray[pageCount].m_tick = new TickBin[SVLimit.PAGE_TICKGIF_MAXNUM];


            TickBin tickBin = pageArrayBin.pageArray[pageCount].m_tick[heartCount];
            tickBin.imageOffsetList = new UInt32[8];

            tickBin.id = ID;
            tickBin.rect.sX = (UInt16)Rect.X;
            tickBin.rect.sY = (UInt16)Rect.Y;
            tickBin.rect.eX = (UInt16)(Rect.Width + tickBin.rect.sX - 1);
            tickBin.rect.eY = (UInt16)(Rect.Height + tickBin.rect.sY - 1);

            ///保存所有图片数据
            List<SVBitmap> list = _bitMapArray.imageArray();
            tickBin.count = (Byte)list.Count;

            for (int i = 0; i < list.Count; i++)
            {
                var offset = list[i];
                tickBin.imageOffsetList[i] = (UInt32)serialize.ToArray().Length;
                serialize.pack(offset.bitmap8Data(Rect.Width, Rect.Height));
            }

            pageArrayBin.pageArray[pageCount].m_tick[heartCount] = tickBin;
        }
    }
}
