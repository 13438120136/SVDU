using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVLabelProperties
    {
        Color _bgcolor;      //背景颜色
        Color _fgcolor;      //文本颜色
        String _text;        //文本内容
        Font _font;          //文本字体
        Rectangle _rect;     //文本控件尺寸
        UInt16 _id;          //文本ID
        String _align;       //文本对齐方式
        Boolean _transparent; //背景是否透明
        String _controlType;
        Boolean _isLock;

        //字体的配置
        Dictionary<Font, Byte> _fontConfig = new Dictionary<Font, Byte>(); 
        //对齐方式的配置
        Dictionary<String, Byte> _alignConfig = new Dictionary<String, Byte>();

        public UpdateControl UpdateControl;

        public SVLabelProperties()
        {
            _font = new Font("宋体", 8);
            _rect = new Rectangle(0, 0, 120, 80);
            _text = "Text";
            _bgcolor = Color.White;
            _fgcolor = Color.Black;
            _align = "水平和垂直居中";
            _transparent = false;
            _controlType = "文本框";
            _isLock = false;

            //字体的映射关系
            _fontConfig.Add(new Font("宋体", 8), 8);
            _fontConfig.Add(new Font("宋体", 12), 12);
            _fontConfig.Add(new Font("宋体", 16), 16);
            //对齐方式的映射关系
            _alignConfig.Add("左对齐", 0);
            _alignConfig.Add("右对齐", 1);
            _alignConfig.Add("居中对齐", 2);
            _alignConfig.Add("水平和垂直居中", 3);
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
        [DescriptionAttribute("文本ID")]        
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
        [DescriptionAttribute("背景颜色是否透明,True为透明 False不透明")]
        [DisplayName("背景透明")]
        public Boolean Transparent
        {
            get { return _transparent; }
            set 
            {
                if (_transparent == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Boolean before = _transparent;
                undoItem.ReDo = () =>
                {
                    _transparent = value;
                };
                undoItem.UnDo = () =>
                {
                    _transparent = before;
                };

                _transparent = value; 
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("文本背景显示颜色")]
        [DisplayName("背景")]
        public Color BackColorground
        {
            set
            {
                if (_bgcolor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _bgcolor;
                undoItem.ReDo = () =>
                {
                    _bgcolor = value;
                };
                undoItem.UnDo = () =>
                {
                    _bgcolor = before;
                };

                _bgcolor = value;
            }

            get
            {
                return _bgcolor;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("文本对齐方式显示")]
        [DisplayName("对齐方式")]
        [TypeConverter(typeof(SVSelectAlignProperty))]        
        public String Align
        {
            set
            {
                if (_align == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                String before = _align;
                undoItem.ReDo = () =>
                {
                    _align = value;
                };
                undoItem.UnDo = () =>
                {
                    _align = before;
                };

                _align = value;
            }

            get
            {
                return _align;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("文本文字颜色")]
        [DisplayName("颜色")]
        public Color FrontColorground
        {
            set
            {
                if (_fgcolor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _fgcolor;
                undoItem.ReDo = () =>
                {
                    _fgcolor = value;
                };
                undoItem.UnDo = () =>
                {
                    _fgcolor = before;
                };

                _fgcolor = value;
            }

            get
            {
                return _fgcolor;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("设置文本字体")]
        [TypeConverter(typeof(SVFontTypeConverter))]
        [DisplayName("字体")]
        public Font Font
        {
            set
            {
                if (_font == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Font before = _font;
                undoItem.ReDo = () =>
                {
                    _font = value;
                };
                undoItem.UnDo = () =>
                {
                    _font = before;
                };

                _font = value;
            }

            get
            {
                return _font;
            }
        }

        [CategoryAttribute("数据")]        
        [EditorAttribute(typeof(SVLabelTextUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DescriptionAttribute("设置文本控件显示的文本内容")]
        [DisplayName("文本")]
        public String Text
        {
            set
            {
                //最大长度不能超过32
                if (value.Length > SVLimit.TEXT_MAX_LEN)
                    return;

                if (_text == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                String before = _text;
                undoItem.ReDo = () =>
                {
                    _text = value;
                };
                undoItem.UnDo = () =>
                {
                    _text = before;
                };

                _text = value;
            }

            get
            {
                return _text;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("文本位置及尺寸设置")]
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


        void copyDestByteArray(byte[] src, byte[] dest)
        {
            int minLen = src.Length > dest.Length ? dest.Length : src.Length;
            Array.Copy(src, dest, minLen);
        }

        public void make(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            UInt32 pageCount = pageArrayBin.pageCount;
            UInt32 areaCount = pageArrayBin.pageArray[pageCount].areaNum++;

            if (pageArrayBin.pageArray[pageCount].m_area == null)
                pageArrayBin.pageArray[pageCount].m_area = new AreaBin[SVLimit.PAGE_AREA_MAXNUM];

            AreaBin areaBin = pageArrayBin.pageArray[pageCount].m_area[areaCount];

            areaBin.id = ID;
            areaBin.rect.sX = (UInt16)Rect.X;
            areaBin.rect.sY = (UInt16)Rect.Y;
            areaBin.rect.eX = (UInt16)(Rect.Width + areaBin.rect.sX);
            areaBin.rect.eY = (UInt16)(Rect.Height + areaBin.rect.sY);

            areaBin.fontClr = (UInt32)FrontColorground.ToArgb();
            areaBin.bgClr = (UInt32)BackColorground.ToArgb();

            areaBin.text = new Byte[SVLimit.TEXT_MAX_LEN];
            copyDestByteArray(Encoding.Unicode.GetBytes(Text), areaBin.text);

            areaBin.font = _fontConfig[_font];
            areaBin.align = _alignConfig[Align];
            areaBin.transparent = Transparent ? (Byte)1 : (Byte)0;

            pageArrayBin.pageArray[pageCount].m_area[areaCount] = areaBin;
        }
    }
}
