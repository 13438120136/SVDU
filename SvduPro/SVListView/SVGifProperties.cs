using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVGifProperties
    {
        UInt16 _id;          //动态图片ID
        Rectangle _rect;     //文本控件尺寸

        List<String> _var;
        List<Byte> _varType;

        SVBitmapArray _pic;
        SVBitmap _picError;

        String _controlType;
        Boolean _isLock;

        String _varList;   ///设置变量

        public UpdateControl UpdateControl;

        public SVGifProperties()
        {
            _rect = new Rectangle(0, 0, 100, 100);
            _controlType = "动态图";
            _picError = new SVBitmap();
            _pic = new SVBitmapArray();
            _var = new List<String>();
            _varType = new List<Byte>();
            _isLock = false;
        }

        [CategoryAttribute("数据")]
        [DisplayName("动态图数据")]
        [DescriptionAttribute("设置动态图的关联变量")]
        [EditorAttribute(typeof(SVGifDataTypeEditor), typeof(UITypeEditor))]
        public String VarList
        {
            get { return _varList; }
            set { _varList = value; }
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DisplayName("控件")]
        [DescriptionAttribute("控件属性显示")]
        public String CtrlType
        {
            get { return _controlType; }
            set { _controlType = value; }
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
        [DescriptionAttribute("对应的变量名称")]
        [DisplayName("变量")]
        [Browsable(false)]
        public List<String> VarName
        {
            set
            {
                _var = value;
            }

            get
            {
                return _var;
            }
        }

        [Browsable(false)]
        public List<Byte> VarType
        {
            get { return _varType; }
            set { _varType = value; }
        }

        [CategoryAttribute("数据")]
        [DescriptionAttribute("图片")]
        [DisplayName("图片")]
        [EditorAttribute(typeof(SVBitmapArrayTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SVBitmapArray))]
        [Browsable(false)]
        public SVBitmapArray Pic
        {
            set
            {
                if (_pic == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                SVBitmapArray before = _pic;
                undoItem.ReDo = () =>
                {
                    _pic = value;
                };
                undoItem.UnDo = () =>
                {
                    _pic = before;
                };

                _pic = value;
            }

            get
            {
                return _pic;
            }
        }

        [CategoryAttribute("数据")]
        [DescriptionAttribute("出错图片")]
        [DisplayName("出错图片")]
        [EditorAttribute(typeof(SVBitmapTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SVBitmap))]
        [Browsable(false)]
        public SVBitmap PicError
        {
            set
            {
                if (_picError == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                SVBitmap before = _picError;
                undoItem.ReDo = () =>
                {
                    _picError = value;
                };
                undoItem.UnDo = () =>
                {
                    _picError = before;
                };

                _picError = value;
            }

            get
            {
                return _picError;
            }
        }


        [CategoryAttribute("外观")]
        [DescriptionAttribute("设置动态图的位置以及尺寸")]
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
            UInt32 gifCount = pageArrayBin.pageArray[pageCount].gif_num++;

            if (pageArrayBin.pageArray[pageCount].m_gif == null)
                pageArrayBin.pageArray[pageCount].m_gif = new GifBin[SVLimit.PAGE_GIF_MAXNUM];

            GifBin gifBin = pageArrayBin.pageArray[pageCount].m_gif[gifCount];
            gifBin.addOffset = new UInt32[8];
            gifBin.varType = new Byte[8];

            gifBin.id = ID;
            gifBin.rect.sX = (UInt16)Rect.X;
            gifBin.rect.sY = (UInt16)Rect.Y;
            gifBin.rect.eX = (UInt16)(Rect.Width + gifBin.rect.sX);
            gifBin.rect.eY = (UInt16)(Rect.Height + gifBin.rect.sY);

            ///图片数组地址
            gifBin.imageOffset = new UInt32[8];

            ///保存所有图片数据
            List<SVBitmap> list = _pic.imageArray();
            for (int i = 0; i < list.Count; i++)
            {
                var address = list[i];
                gifBin.imageOffset[i] = (UInt32)serialize.ToArray().Length;
                serialize.pack(address.bitmap8Data(Rect.Width, Rect.Height));
            }

            ///写入当前错误图片数据            
            var errorAddress = PicError.bitmap8Data(Rect.Width, Rect.Height);
            if (errorAddress != null)
            {
                gifBin.iamgeOffsetErr = (UInt32)serialize.ToArray().Length;
                serialize.pack(errorAddress);
            }            

            ///根据名称来获取地址
            var varInstance = SVVaribleType.instance();

            for (int i = 0; i < _var.Count; i++)
            {
                varInstance.loadVariableData();
                varInstance.setDataType(_varType[i]);
                gifBin.addOffset[i] = varInstance.strToAddress(_var[i], _varType[i]);
                gifBin.varType[i] = (Byte)varInstance.strToType(_var[i]);
            }

            ///变量个数
            gifBin.type = Convert.ToByte(_var.Count);

            pageArrayBin.pageArray[pageCount].m_gif[gifCount] = gifBin;
        }
    }
}
