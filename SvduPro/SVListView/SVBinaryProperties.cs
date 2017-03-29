using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Text;
using SVCore;
using System.Collections.Generic;

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

        /// <summary>
        /// 以下三个字段，当显示为文本时，为文本内容
        /// 当显示为图片的时候，为图片文件路径
        /// </summary>
        String _customTrueText;      //自定义值为真
        String _customFlaseText;     //自定义值为假
        //String _customExceptionText; //异常字段
        SVBitmap _truePicture;       //为真的图片
        SVBitmap _flasePicture;      //为假的图片
        SVBitmap _exPicture;         //异常图片

        Rectangle _rect;    //尺寸
        //String _var;        //关联的变量
        //Byte _varType;      //变量类型
        Byte _type;       //显示的格式
        String _controlType;   //控件类型
        Boolean _isLock;

        SVVarDefine _var;  //变量

        public UpdateControl UpdateControl;

        public SVBinaryProperties()
        {
            _font = new Font("华文细黑", 12);
            _rect = new Rectangle(0, 0, 120, 40);
            _trueColor = Color.Green;
            _trueBgColor = Color.Moccasin;
            _falseColor = Color.Blue;
            _falseBgColor = Color.White;
            _exceptionColor = Color.Red;
            _exceptionBgColor = Color.White;
            _type = 0;
            _customTrueText = "True";
            _customFlaseText = "False";
            _controlType = "开关量";
            _isLock = false;
            _var = new SVVarDefine();

            _truePicture = new SVBitmap();
            _flasePicture = new SVBitmap();
            _exPicture = new SVBitmap();
        }

        [CategoryAttribute("数据")]
        [DescriptionAttribute("关联的开关量变量, I:表示接收区 O:表示发送区 S:表示系统区")]
        [DisplayName("关联变量")]
        [TypeConverter(typeof(SVDefineVarConverter))]
        [EditorAttribute(typeof(SVBinaryVarTypeEditor), typeof(UITypeEditor))]
        public SVVarDefine Variable
        {
            get { return _var; }
            set 
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                SVVarDefine before = _var;
                undoItem.ReDo = () =>
                {
                    _var = value;
                };
                undoItem.UnDo = () =>
                {
                    _var = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _var = value;
            }
        }

        /// <summary>
        /// 设置属性的显示和隐藏属性
        /// </summary>
        /// <param oldName="obj">当前属性对象</param>
        /// <param oldName="propertyName">具体属性名称</param>
        /// <param oldName="visible">true表示显示，false表示隐藏</param>
        void SetPropertyVisibility(object obj, string propertyName, bool visible)
        {
            Type type = typeof(BrowsableAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;

            FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
            fld.SetValue(attrs[type], visible);        
        }

        [Browsable(true)]
        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为真的时候，显示的背景图片")]
        [TypeConverter(typeof(SVBitmap))]
        [EditorAttribute(typeof(SVBitmapTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("真-背景")]
        public SVBitmap TruePicture
        {
            get { return _truePicture; }
            set 
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                SVBitmap before = _truePicture;
                undoItem.ReDo = () =>
                {
                    _truePicture = value;
                };
                undoItem.UnDo = () =>
                {
                    _truePicture = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _truePicture = value; 
            }
        }

        [Browsable(true)]
        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为假的时候，显示的背景图片")]
        [TypeConverter(typeof(SVBitmap))]
        [EditorAttribute(typeof(SVBitmapTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("假-背景")]
        public SVBitmap FlasePicture
        {
            get { return _flasePicture; }
            set 
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                SVBitmap before = _flasePicture;
                undoItem.ReDo = () =>
                {
                    _flasePicture = value;
                };
                undoItem.UnDo = () =>
                {
                    _flasePicture = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _flasePicture = value; 
            }
        }

        [Browsable(true)]
        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为异常情况下，显示的背景图片")]
        [TypeConverter(typeof(SVBitmap))]
        [EditorAttribute(typeof(SVBitmapTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("异常-背景")]
        public SVBitmap ExPicture
        {
            get { return _exPicture; }
            set
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                SVBitmap before = _exPicture;
                undoItem.ReDo = () =>
                {
                    _exPicture = value;
                };
                undoItem.UnDo = () =>
                {
                    _exPicture = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _exPicture = value; 
            }
        }

        //[Browsable(false)]
        //public String CustomExceptionText
        //{
        //    get { return _customExceptionText; }
        //    set { _customExceptionText = value; }
        //}

        [Browsable(true)]
        [CategoryAttribute("外观")]
        [DisplayName("真文本")]
        [DescriptionAttribute("当前开关量值为真表示的文本")]
        public String CustomTrueText
        {
            get { return _customTrueText; }
            set 
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                String before = _customTrueText;
                undoItem.ReDo = () =>
                {
                    _customTrueText = value;
                };
                undoItem.UnDo = () =>
                {
                    _customTrueText = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _customTrueText = value; 
            }
        }

        [Browsable(true)]
        [CategoryAttribute("外观")]
        [DisplayName("假文本")]
        [DescriptionAttribute("当前开关量值为假表示的文本")]
        public String CustomFlaseText
        {
            get { return _customFlaseText; }
            set 
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                String before = _customFlaseText;
                undoItem.ReDo = () =>
                {
                    _customFlaseText = value;
                };
                undoItem.UnDo = () =>
                {
                    _customFlaseText = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _customFlaseText = value; 
            }
        }

        //[Browsable(false)]
        //public Byte VarType
        //{
        //    get { return _varType; }
        //    set { _varType = value; }
        //}

        [CategoryAttribute("属性")]
        [EditorAttribute(typeof(SVLockUITypeEditor), typeof(UITypeEditor))]
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

        //[CategoryAttribute("数据")]
        //[DescriptionAttribute("关联的开关量变量")]
        //[DisplayName("变量")]
        //[EditorAttribute(typeof(SVBinaryVarTypeEditor), typeof(UITypeEditor))]
        //public String Var
        //{
        //    get { return _var; }
        //    set 
        //    {
        //        if (_var == value)
        //            return;

        //        SVRedoUndoItem undoItem = new SVRedoUndoItem();

        //        if (UpdateControl != null)
        //            UpdateControl(undoItem);
        //        String before = _var;
        //        undoItem.ReDo = () =>
        //        {
        //            _var = value;
        //        };
        //        undoItem.UnDo = () =>
        //        {
        //            _var = before;
        //        };

        //        _var = value; 
        //    }
        //}
        public void reRefresh()
        {
            if (_type == 0)
            {
                SetPropertyVisibility(this, "CustomTrueText", true);
                SetPropertyVisibility(this, "CustomFlaseText", true);
                SetPropertyVisibility(this, "ExceptionColor", true);
                SetPropertyVisibility(this, "ExceptionBgColor", true);
                SetPropertyVisibility(this, "TrueColor", true);
                SetPropertyVisibility(this, "TrueBgColor", true);
                SetPropertyVisibility(this, "FalseColor", true);
                SetPropertyVisibility(this, "FalseBgColor", true);
                SetPropertyVisibility(this, "TruePicture", false);
                SetPropertyVisibility(this, "FlasePicture", false);
                SetPropertyVisibility(this, "ExPicture", false);
                SetPropertyVisibility(this, "Font", true);
            }
            else
            {
                SetPropertyVisibility(this, "CustomTrueText", false);
                SetPropertyVisibility(this, "CustomFlaseText", false);
                SetPropertyVisibility(this, "ExceptionColor", false);
                SetPropertyVisibility(this, "ExceptionBgColor", false);
                SetPropertyVisibility(this, "TrueColor", false);
                SetPropertyVisibility(this, "TrueBgColor", false);
                SetPropertyVisibility(this, "FalseColor", false);
                SetPropertyVisibility(this, "FalseBgColor", false);
                SetPropertyVisibility(this, "TruePicture", true);
                SetPropertyVisibility(this, "FlasePicture", true);
                SetPropertyVisibility(this, "ExPicture", true);
                SetPropertyVisibility(this, "Font", false);
            }
        }
        
        [CategoryAttribute("数据")]
        [TypeConverter(typeof(SVBinaryTypeConverter))]
        [DescriptionAttribute("当前开关量的具体含义, 0：表示文本显示， 1：表示图片方式显示")]
        [EditorAttribute(typeof(SVBinaryTypeTypeEditor), typeof(UITypeEditor))]
        [DisplayName("背景显示类型")]
        public Byte Type
        {
            set
            {
                if (value == 0)
                {
                    SetPropertyVisibility(this, "CustomTrueText", true);
                    SetPropertyVisibility(this, "CustomFlaseText", true);
                    SetPropertyVisibility(this, "ExceptionColor", true);
                    SetPropertyVisibility(this, "ExceptionBgColor", true);
                    SetPropertyVisibility(this, "TrueColor", true);
                    SetPropertyVisibility(this, "TrueBgColor", true);
                    SetPropertyVisibility(this, "FalseColor", true);
                    SetPropertyVisibility(this, "FalseBgColor", true);
                    SetPropertyVisibility(this, "TruePicture", false);
                    SetPropertyVisibility(this, "FlasePicture", false);
                    SetPropertyVisibility(this, "ExPicture", false);
                    SetPropertyVisibility(this, "Font", true);
                }
                else
                {
                    SetPropertyVisibility(this, "CustomTrueText", false);
                    SetPropertyVisibility(this, "CustomFlaseText", false);
                    SetPropertyVisibility(this, "ExceptionColor", false);
                    SetPropertyVisibility(this, "ExceptionBgColor", false);
                    SetPropertyVisibility(this, "TrueColor", false);
                    SetPropertyVisibility(this, "TrueBgColor", false);
                    SetPropertyVisibility(this, "FalseColor", false);
                    SetPropertyVisibility(this, "FalseBgColor", false);
                    SetPropertyVisibility(this, "TruePicture", true);
                    SetPropertyVisibility(this, "FlasePicture", true);
                    SetPropertyVisibility(this, "ExPicture", true);
                    SetPropertyVisibility(this, "Font", false);
                }

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

        [Browsable(true)]
        [CategoryAttribute("外观"), DescriptionAttribute("出现异常后，字体显示的颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        [Browsable(true)]
        [CategoryAttribute("外观"), DescriptionAttribute("出现异常后，背景显示的颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        [Browsable(true)]
        [CategoryAttribute("数据")]
        [DescriptionAttribute("设置显示字体")]
        [TypeConverter(typeof(SVFontTypeConverter))]
        [EditorAttribute(typeof(SVFontTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        [Browsable(true)]
        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为真的时候的颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        [Browsable(false)]
        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为真的时候的背景颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        [Browsable(false)]
        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为假的时候的颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        [Browsable(false)]
        [CategoryAttribute("外观")]
        [DescriptionAttribute("值为假的时候的背景颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

        /// <summary>
        /// 执行拷贝字节数组中的字符串
        /// </summary>
        /// <param oldName="src"></param>
        /// <param oldName="dest"></param>
        void copyDestByteArray(byte[] src, byte[] dest)
        {
            int minLen = src.Length > dest.Length ? dest.Length : src.Length;
            Array.Copy(src, dest, minLen);
        }

        /// <summary>
        /// 生成下装文件
        /// </summary>
        /// <param oldName="pageArrayBin">下装配置文件</param>
        /// <param oldName="serialize">序列化对象</param>
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

            binaryBin.font = (Byte)_font.Size;
            binaryBin.type = _type;

            ///存放字符串相关的属性和文本信息
            if (_type == 0)
            {
                binaryBin.trueClr = (UInt32)TrueColor.ToArgb();
                binaryBin.trueBgClr = (UInt32)TrueBgColor.ToArgb();
                binaryBin.falseClr = (UInt32)FalseColor.ToArgb();
                binaryBin.falseBgClr = (UInt32)FalseBgColor.ToArgb();
                binaryBin.vinfoInvalid = (UInt32)ExceptionColor.ToArgb();
                binaryBin.vinfoInvalidBg = (UInt32)ExceptionBgColor.ToArgb();

                binaryBin.trueText = new Byte[SVLimit.BINARY_MAX_LEN];
                if (CustomTrueText != null)
                    copyDestByteArray(Encoding.Unicode.GetBytes(CustomTrueText), binaryBin.trueText);

                binaryBin.falseText = new Byte[SVLimit.BINARY_MAX_LEN];
                if (CustomFlaseText != null)
                    copyDestByteArray(Encoding.Unicode.GetBytes(CustomFlaseText), binaryBin.falseText);
            }
            else ///存放与背景图片有关的信息
            {
                ///为真的图片地址
                var trueAddress = this.TruePicture.bitmap8Data(Rect.Width, Rect.Height);
                if (trueAddress != null)
                {
                    binaryBin.trueClr = (UInt32)serialize.ToArray().Length;
                    serialize.pack(trueAddress);
                }

                ///为假的图片地址
                var falseAddress = this.FlasePicture.bitmap8Data(Rect.Width, Rect.Height);
                if (falseAddress != null)
                {
                    binaryBin.falseClr = (UInt32)serialize.ToArray().Length;
                    serialize.pack(falseAddress);
                }

                ///异常的图片地址
                var exAddress = this.ExPicture.bitmap8Data(Rect.Width, Rect.Height);
                if (exAddress != null)
                {
                    binaryBin.vinfoInvalid = (UInt32)serialize.ToArray().Length;
                    serialize.pack(exAddress);
                }
            }

            ///根据名称来获取地址
            var varInstance = SVVaribleType.instance();
            binaryBin.addrOffset = varInstance.strToAddress(_var.VarName, _var.VarType);
            binaryBin.varType = (Byte)varInstance.strToType(_var.VarName, _var.VarType);

            pageArrayBin.pageArray[pageCount].m_binary[binaryCount] = binaryBin;
        }
    }
}
