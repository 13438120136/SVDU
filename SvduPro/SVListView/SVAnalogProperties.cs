using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVAnalogProperties
    {
        UInt16 _id;               //ID
        Single _max;              //最大值
        Single _min;              //最小值
        Font _font;               //显示字体

        Color _normalColor;       //正常显示
        Color _overMaxClr;
        Color _overMinClr;

        Color _normalBgColor;
        Color _overMaxBgClr;
        Color _overMinBgClr;

        Color _exceptionColor;   //出现异常的字体颜色
        Color _exceptionBgColor; //出现异常的背景颜色

        Byte _decNum;           //小数点后面位数
        Rectangle _rect;        //尺寸
        String _var;            //变量
        Byte _varType;          //变量类型

        Boolean _isExponent;    //是否指数显示
        String _controlType;    //控件类型

        Boolean _isLock;        //是否被锁定

        //字体的配置
        Dictionary<Font, Byte> _fontConfig = new Dictionary<Font, Byte>();

        public UpdateControl UpdateControl;

        public SVAnalogProperties()
        {
            _font = new Font("Courier New", 8);
            _rect = new Rectangle(0, 0, 120, 40);
            _normalColor = Color.DodgerBlue;
            _overMaxClr = Color.Green;
            _overMinClr = Color.Green;
            _normalBgColor = Color.Moccasin;
            _overMaxBgClr = Color.Red;
            _overMinBgClr = Color.Red;
            _exceptionColor = _normalColor;
            _exceptionBgColor = _normalBgColor;
            _decNum = 1;
            _min = 1;
            _max = 100;

            _controlType = "模拟量";

            //字体的映射关系
            _fontConfig.Add(new Font("Courier New", 8), 8);
            _fontConfig.Add(new Font("Courier New", 12), 12);
            _fontConfig.Add(new Font("Courier New", 16), 16);

            _isLock = false;
        }

        [Browsable(false)]
        public Byte VarType
        {
            get { return _varType; }
            set { _varType = value; }
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
        [DisplayName("控件")]
        [DescriptionAttribute("控件类型显示")]
        public String CtrlType
        {
            get { return _controlType; }
            set { _controlType = value; }
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("显示ID")]
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
        [DescriptionAttribute("设置当前模拟量关联的变量")]
        [DisplayName("变量")]
        [EditorAttribute(typeof(SVAnalogVarTypeEditor), typeof(UITypeEditor))]
        public String Var
        {
            set
            {
                if (_var == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);

                String before = _var;
                undoItem.ReDo = () =>
                {
                    _var = value;
                };
                undoItem.UnDo = () =>
                {
                    _var = before;
                };

                _var = value;
            }

            get
            {
                return _var;
            }
        }

        [CategoryAttribute("数据"), DescriptionAttribute("设置模拟量最大值")]
        [DisplayName("最大值")]
        public Single Max
        {
            set
            {
                if (_max == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Single before = _max;
                undoItem.ReDo = () =>
                {
                    _max = value;
                };
                undoItem.UnDo = () =>
                {
                    _max = before;
                };

                _max = value;
            }

            get
            {
                return _max;
            }
        }

        [CategoryAttribute("数据"), DescriptionAttribute("设置模拟量最小值")]
        [DisplayName("最小值")]
        public Single Min
        {
            set
            {
                if (_min == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Single before = _min;
                undoItem.ReDo = () =>
                {
                    _min = value;
                };
                undoItem.UnDo = () =>
                {
                    _min = before;
                };
                _min = value;
            }

            get
            {
                return _min;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("设置文本显示字体")]
        [DisplayName("字体")]
        [TypeConverter(typeof(SVFontTypeConverter))]
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

        [CategoryAttribute("外观"), DescriptionAttribute("出现异常后，字体显示的颜色")]
        [DisplayName("异常")]
        public Color ExceptionColor
        {
            set
            {
                if (_exceptionColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _exceptionColor;
                undoItem.ReDo = () =>
                {
                    _exceptionColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _exceptionColor = before;
                };
                _exceptionColor = value;
            }

            get
            {
                return _exceptionColor;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("出现异常后，背景显示的颜色")]
        [DisplayName("异常背景")]
        public Color ExceptionBgColor
        {
            set
            {
                if (_exceptionBgColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _exceptionBgColor;
                undoItem.ReDo = () =>
                {
                    _exceptionBgColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _exceptionBgColor = before;
                };
                _exceptionBgColor = value;
            }

            get
            {
                return _exceptionBgColor;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("设置文本正常显示颜色")]
        [DisplayName("颜色")]
        public Color NormalColor
        {
            set
            {
                if (_normalColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _normalColor;
                undoItem.ReDo = () =>
                {
                    _normalColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _normalColor = before;
                };
                _normalColor = value;
            }

            get
            {
                return _normalColor;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("文本大于最大值,设置显示颜色")]
        [DisplayName("颜色")]
        public Color OverMaxColor
        {
            set
            {
                if (_overMaxClr.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _overMaxClr;
                undoItem.ReDo = () =>
                {
                    _overMaxClr = value;
                };
                undoItem.UnDo = () =>
                {
                    _overMaxClr = before;
                };
                _overMaxClr = value;
            }

            get
            {
                return _overMaxClr;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("文本小于最小值, 设置显示颜色")]
        [DisplayName("颜色")]
        public Color OverMinColor
        {
            set
            {
                if (_overMinClr.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _overMinClr;
                undoItem.ReDo = () =>
                {
                    _overMinClr = value;
                };
                undoItem.UnDo = () =>
                {
                    _overMinClr = before;
                };
                _overMinClr = value;
            }

            get
            {
                return _overMinClr;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("设置当前值处于正常范围内的背景颜色")]
        [DisplayName("背景")]
        public Color NormalBgColor
        {
            set
            {
                if (_normalBgColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _normalBgColor;
                undoItem.ReDo = () =>
                {
                    _normalBgColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _normalBgColor = before;
                };
                _normalBgColor = value;
            }

            get
            {
                return _normalBgColor;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("设置当前值小于最小值正常范围内的背景颜色")]
        [DisplayName("背景")]
        public Color OverMinBgColor
        {
            set
            {
                if (_overMinBgClr.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _overMinBgClr;
                undoItem.ReDo = () =>
                {
                    _overMinBgClr = value;
                };
                undoItem.UnDo = () =>
                {
                    _overMinBgClr = before;
                };
                _overMinBgClr = value;
            }

            get
            {
                return _overMinBgClr;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("设置当前值大于最大值正常范围内的背景颜色")]
        [DisplayName("背景")]
        public Color OverMaxBgColor
        {
            set
            {
                if (_overMaxBgClr.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _overMaxBgClr;
                undoItem.ReDo = () =>
                {
                    _overMaxBgClr = value;
                };
                undoItem.UnDo = () =>
                {
                    _overMaxBgClr = before;
                };
                _overMaxBgClr = value;
            }

            get
            {
                return _overMaxBgClr;
            }
        }

        [CategoryAttribute("外观"), DescriptionAttribute("模拟量控件的尺寸")]
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

        [CategoryAttribute("数据")]
        [DescriptionAttribute("当前模拟量显示，小数点位数,范围1-6")]
        [DisplayName("小数点")]
        public Byte DecNum
        {
            set
            {
                if (value >= 1 && value <= 6)
                {
                    if (_decNum == value)
                        return;

                    SVRedoUndoItem undoItem = new SVRedoUndoItem();
                    if (UpdateControl != null)
                        UpdateControl(undoItem);
                    Byte before = _decNum;
                    undoItem.ReDo = () =>
                    {
                        _decNum = value;
                    };
                    undoItem.UnDo = () =>
                    {
                        _decNum = before;
                    };
                    _decNum = value;
                }
            }

            get
            {
                return _decNum;
            }
        }

        [CategoryAttribute("数据"), DescriptionAttribute("当前模拟量显示，是否已指数显示\nTrue表示以指数显示，False正常显示,默认为:False")]
        [DisplayName("指数")]
        public Boolean IsExponent
        {
            set
            {
                if (_isExponent == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Boolean before = _isExponent;
                undoItem.ReDo = () =>
                {
                    _isExponent = value;
                };
                undoItem.UnDo = () =>
                {
                    _isExponent = before;
                };
                _isExponent = value;
            }

            get
            {
                return _isExponent;
            }
        }

        public void make(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            UInt32 pageCount = pageArrayBin.pageCount;
            UInt32 analogCount = pageArrayBin.pageArray[pageCount].analog_num++;

            if (pageArrayBin.pageArray[pageCount].m_analog == null)
                pageArrayBin.pageArray[pageCount].m_analog = new AnalogBin[SVLimit.PAGE_ANA_MAXNUM];

            AnalogBin analogBin = pageArrayBin.pageArray[pageCount].m_analog[analogCount];

            analogBin.id = ID;
            analogBin.rect.sX = (UInt16)Rect.X;
            analogBin.rect.sY = (UInt16)Rect.Y;
            analogBin.rect.eX = (UInt16)(Rect.Width + analogBin.rect.sX);
            analogBin.rect.eY = (UInt16)(Rect.Height + analogBin.rect.sY);

            analogBin.normalClr = (UInt32)NormalColor.ToArgb();
            analogBin.normalBgClr = (UInt32)NormalBgColor.ToArgb();
            analogBin.overMaxClr = (UInt32)OverMaxColor.ToArgb();
            analogBin.overMinClr = (UInt32)OverMinColor.ToArgb();
            analogBin.overMaxBgClr = (UInt32)OverMaxBgColor.ToArgb();
            analogBin.overMinBgClr = (UInt32)OverMinBgColor.ToArgb();
            analogBin.vinfoInvalid = (UInt32)ExceptionColor.ToArgb();
            analogBin.vinfoInvalidBg = (UInt32)ExceptionBgColor.ToArgb();

            analogBin.vMax = Max;
            analogBin.vMin = Min;

            analogBin.font = _fontConfig[_font];
            analogBin.nDecimalNum = DecNum;
            analogBin.enExponent = _isExponent ? (Byte)1 : (Byte)0;

            ///根据名称来获取地址
            var varInstance = SVVaribleType.instance();
            varInstance.loadVariableData();
            varInstance.setDataType(_varType);

            analogBin.addrOffset = varInstance.strToAddress(_var, _varType);
            analogBin.varType = (Byte)varInstance.strToType(_var);

            pageArrayBin.pageArray[pageCount].m_analog[analogCount] = analogBin;
        }
    }
}
