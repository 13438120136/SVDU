using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVBinaryProperties
    {
        UInt16 _id;         //ID

        Font _font;         //显示字体
        Color _trueColor;
        Color _trueBgColor;
        Color _falseColor;
        Color _falseBgColor;

        Color _exceptionColor;   //出现异常的字体颜色
        Color _exceptionBgColor; //出现异常的背景颜色
        String _customTrueText;  //自定义值为真
        String _customFlaseText; //自定义值为假

        Rectangle _rect;    //尺寸
        String _var;        //关联的变量
        Byte _varType;      //变量类型
        Byte _type;       //显示的格式
        String _controlType;   //控件类型
        Boolean _isLock;

        //字体的配置
        Dictionary<Font, Byte> _fontConfig = new Dictionary<Font, Byte>(); 
        //显示的格式
        Dictionary<String, Byte> _showConfig = new Dictionary<String, Byte>();

        public UpdateControl UpdateControl;

        public SVBinaryProperties()
        {
            _font = new Font("宋体", 16);
            _rect = new Rectangle(0, 0, 120, 40);
            _trueColor = Color.Blue;
            _trueBgColor = Color.Moccasin;
            _falseColor = Color.Red;
            _falseBgColor = Color.Blue;
            _exceptionColor = _trueColor;
            _exceptionBgColor = _trueBgColor;
            //_type = "打开 or 关闭";
            _controlType = "开关量";
            _isLock = false;

            //字体的映射关系
            _fontConfig.Add(new Font("宋体", 8), 8);
            _fontConfig.Add(new Font("宋体", 12), 12);
            _fontConfig.Add(new Font("宋体", 16), 16);
            //显示格式的映射关系
            _showConfig.Add("打开 or 关闭", 0);
            _showConfig.Add("运行 or 停止", 1);
            _showConfig.Add("1 or 0", 2);
            _showConfig.Add("是 or 否", 3);
            _showConfig.Add("真 or 假", 4);
            _showConfig.Add("正确 or 错误", 5);
            _showConfig.Add("开 or 关", 6);
        }

        [Browsable(false)]
        public String CustomTrueText
        {
            get { return _customTrueText; }
            set { _customTrueText = value; }
        }

        [Browsable(false)]
        public String CustomFlaseText
        {
            get { return _customFlaseText; }
            set { _customFlaseText = value; }
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
        [DescriptionAttribute("控件属性显示")]
        public String CtrlType
        {
            get { return _controlType; }
            set { _controlType = value; }
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("开关量控件显示ID")]
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
        [DescriptionAttribute("关联的开关量变量")]
        [DisplayName("变量")]
        [EditorAttribute(typeof(SVBinaryVarTypeEditor), typeof(UITypeEditor))]
        public String Var
        {
            get { return _var; }
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
        }

        [CategoryAttribute("数据")]
        [DescriptionAttribute("当前开关量的具体含义")]
        //[TypeConverter(typeof(SVBinaryTypeConverter))]
        [EditorAttribute(typeof(SVBinaryTypeTypeEditor), typeof(UITypeEditor))]
        [DisplayName("类型")]
        //[Browsable(false)]
        public Byte Type
        {
            set
            {
                ///限制范围
                if (!(value >= 0 && value <= 7)) 
                    return;

                if (_type == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Byte before = _type;
                undoItem.ReDo = () =>
                {
                    _type = value;
                };
                undoItem.UnDo = () =>
                {
                    _type = before;
                };
                
                _type = value;
            }

            get
            {
                return _type;
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

        [CategoryAttribute("数据")]
        [DescriptionAttribute("设置显示字体")]
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

        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为真的时候的颜色")]
        [DisplayName("为真")]
        public Color TrueColor
        {
            set
            {
                if (_trueColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _trueColor;
                undoItem.ReDo = () =>
                {
                    _trueColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _trueColor = before;
                };

                _trueColor = value;
            }

            get
            {
                return _trueColor;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为真的时候的背景颜色")]
        [DisplayName("为真背景")]
        public Color TrueBgColor
        {
            set
            {
                if (_trueBgColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _trueBgColor;
                undoItem.ReDo = () =>
                {
                    _trueBgColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _trueBgColor = before;
                };
                _trueBgColor = value;
            }

            get
            {
                return _trueBgColor;
            }
        }


        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为假的时候的颜色")]
        [DisplayName("为假")]
        public Color FalseColor
        {
            set
            {
                if (_falseColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _falseColor;
                undoItem.ReDo = () =>
                {
                    _falseColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _falseColor = before;
                };
                _falseColor = value;
            }

            get
            {
                return _falseColor;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为假的时候的背景颜色")]
        [DisplayName("为假背景")]
        public Color FalseBgColor
        {
            set
            {
                if (_falseBgColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                Color before = _falseBgColor;
                undoItem.ReDo = () =>
                {
                    _falseBgColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _falseBgColor = before;
                };
                _falseBgColor = value;
            }

            get
            {
                return _falseBgColor;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("开关量控件的尺寸")]
        [DisplayName("尺寸")]
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
            UInt32 binaryCount = pageArrayBin.pageArray[pageCount].binaryNum++;

            if (pageArrayBin.pageArray[pageCount].m_binary == null)
                pageArrayBin.pageArray[pageCount].m_binary = new BinaryBin[SVLimit.PAGE_BOOL_MAXNUM];

            BinaryBin binaryBin = pageArrayBin.pageArray[pageCount].m_binary[binaryCount];

            binaryBin.id = ID;
            binaryBin.rect.sX = (UInt16)Rect.X;
            binaryBin.rect.sY = (UInt16)Rect.Y;
            binaryBin.rect.eX = (UInt16)(Rect.Width + binaryBin.rect.sX);
            binaryBin.rect.eY = (UInt16)(Rect.Height + binaryBin.rect.sY);

            binaryBin.trueClr = (UInt32)TrueColor.ToArgb();
            binaryBin.trueBgClr = (UInt32)TrueBgColor.ToArgb();
            binaryBin.falseClr = (UInt32)FalseColor.ToArgb();
            binaryBin.falseBgClr = (UInt32)FalseBgColor.ToArgb();
            binaryBin.vinfoInvalid = (UInt32)ExceptionColor.ToArgb();
            binaryBin.vinfoInvalidBg = (UInt32)ExceptionBgColor.ToArgb();

            binaryBin.trueText = new Byte[SVLimit.TEXT_MAX_LEN];
            if (CustomTrueText != null)
                copyDestByteArray(Encoding.Unicode.GetBytes(CustomTrueText), binaryBin.trueText);

            binaryBin.falseText = new Byte[SVLimit.TEXT_MAX_LEN];
            if (CustomFlaseText != null)
                copyDestByteArray(Encoding.Unicode.GetBytes(CustomFlaseText), binaryBin.falseText);

            binaryBin.font = _fontConfig[_font];
            binaryBin.type = _type;

            ///根据名称来获取地址
            var varInstance = SVVaribleType.instance();
            varInstance.loadVariableData();
            varInstance.setDataType(_varType);

            binaryBin.addrOffset = varInstance.strToAddress(_var, _varType);
            binaryBin.varType = (Byte)varInstance.strToType(_var);

            pageArrayBin.pageArray[pageCount].m_binary[binaryCount] = binaryBin;
        }
    }
}
