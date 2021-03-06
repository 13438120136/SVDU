﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Text;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVButtonProperties
    {
        Color _fgcolor;      //文本颜色
        String _text;        //文本内容
        String _fText;       //文本为假的内容
        String _fMemo;       //按钮备注信息

        Font _font;          //文本字体
        Rectangle _rect;     //按钮控件尺寸
        UInt16 _id;          //文本ID
        Boolean _isConfirm;  //是否有确认按钮

        SVBtnChoicePage _btnType; //按钮类型
        String _controlType;   //控件显示类型

        SVBitmap _btnDownPic;  //按钮按下图片
        SVBitmap _btnUpPic;    //按钮弹起图片
        Color _bgcolor;        //背景颜色
        Color _bgDownColor;    //按下去的背景颜色
        Boolean _isShowPic;    //是否显示图片

        Byte _buttonType;            //按钮的操作类型
        Boolean _btnEnable = false;  //按钮使能标志
        SVVarDefine _enVarText;      //选择使能变量
        SVVarDefine _btnVarText;     //按钮关联变量
        String _curveObj;            //关联的趋势图对象

        Boolean _isLock;       //是否锁定

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
        [CategoryAttribute("数据")]
        [DisplayName("趋势图")]
        [DescriptionAttribute("当前按钮操作的相关趋势图.")]
        [EditorAttribute(typeof(SVBtnCurveUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SVBtnCurveConverter))]
        public String CurveObj
        {
            get { return _curveObj; }
            set { _curveObj = value; }
        }

        [Browsable(false)]
        [CategoryAttribute("数据")]
        [DisplayName("假文本")]
        [DescriptionAttribute("变量为假的时候显示的文本内容.")]
        public String FText
        {
            get { return _fText; }
            set { _fText = value; }
        }

        [Browsable(true)]
        [CategoryAttribute("数据")]
        [DisplayName("关联变量")]
        [DescriptionAttribute("按钮关联变量选择.")]
        [EditorAttribute(typeof(SVBtnEnabledVarUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SVDefineVarConverter))]
        public SVVarDefine BtnVarText
        {
            get { return _btnVarText; }
            set { _btnVarText = value; }
        }

        [CategoryAttribute("使能")]
        [DisplayName("使能类型")]
        [DescriptionAttribute("按钮使能变量状态选择，True：表示使能，False：表示禁能.")]
        public Boolean BtnEnable
        {
            get { return _btnEnable; }
            set 
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Boolean before = _btnEnable;
                undoItem.ReDo = () =>
                {
                    _btnEnable = value;
                    SetPropertyVisibility(this, "EnVarText", _btnEnable);
                };
                undoItem.UnDo = () =>
                {
                    _btnEnable = before;
                    SetPropertyVisibility(this, "EnVarText", _btnEnable);
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _btnEnable = value;
                SetPropertyVisibility(this, "EnVarText", _btnEnable);
            }
        }

        [Browsable(true)]
        [CategoryAttribute("使能")]
        [DisplayName("使能变量")]
        [DescriptionAttribute("选择按钮使能关联变量.")]
        [EditorAttribute(typeof(SVBtnEnabledVarUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SVDefineVarConverter))]
        public SVVarDefine EnVarText
        {
            get { return _enVarText; }
            set 
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                SVVarDefine before = _enVarText;
                undoItem.ReDo = () =>
                {
                    _enVarText = value;
                };
                undoItem.UnDo = () =>
                {
                    _enVarText = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _enVarText = value;
            }
        }

        [CategoryAttribute("类型")]
        [DisplayName("操作类型")]
        [DescriptionAttribute("选取当前按钮的具体操作方式.")]
        [EditorAttribute(typeof(SVButtonTypeUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SVButtonTypeConverter))]
        public Byte ButtonType
        {
            get { return _buttonType; }
            set 
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Byte before = _buttonType;
                undoItem.ReDo = () =>
                {
                    _buttonType = value;
                    setBtnTypeShow(_buttonType);
                };
                undoItem.UnDo = () =>
                {
                    _buttonType = before;
                    setBtnTypeShow(_buttonType);
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _buttonType = value;
                setBtnTypeShow(_buttonType);
            }
        }

        private void setBtnTypeShow(Byte bByte)
        {
            switch (bByte)
            {
                case 0:
                    SetPropertyVisibility(this, "BtnVarText", false);
                    SetPropertyVisibility(this, "FText", false);
                    SetPropertyVisibility(this, "CurveObj", false);
                    SetPropertyVisibility(this, "ButtonPage", true);                    
                    break;
                case 1:
                case 2:
                case 4:
                case 5:
                    SetPropertyVisibility(this, "BtnVarText", true);
                    SetPropertyVisibility(this, "FText", false);
                    SetPropertyVisibility(this, "CurveObj", false);
                    SetPropertyVisibility(this, "ButtonPage", false);  
                    break;
                case 3:
                    SetPropertyVisibility(this, "BtnVarText", true);
                    SetPropertyVisibility(this, "FText", true);
                    SetPropertyVisibility(this, "CurveObj", false);
                    SetPropertyVisibility(this, "ButtonPage", false);  
                    break;
                case 6:
                case 7:
                case 8:
                    SetPropertyVisibility(this, "BtnVarText", false);
                    SetPropertyVisibility(this, "CurveObj", true);
                    SetPropertyVisibility(this, "ButtonPage", false);  
                    break;
            }
        }

        [CategoryAttribute("数据")]
        [DisplayName("备注信息")]
        [DescriptionAttribute("按钮的备注信息，用来描述当前按钮的功能及作用")]
        [EditorAttribute(typeof(SVButtonMemoUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String FMemo
        {
            get { return _fMemo; }
            set 
            { 
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                String before = _fMemo;
                undoItem.ReDo = () =>
                {
                    _fMemo = value;
                };
                undoItem.UnDo = () =>
                {
                    _fMemo = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _fMemo = value;
            }
        }

        /// <summary>
        /// 如果是true显示背景图片
        /// 否则显示背景颜色
        /// </summary>
        [CategoryAttribute("背景")]
        [DisplayName("背景类型")]
        [DescriptionAttribute("背景是否以图片显示，如果为True显示为图片，False显示为颜色.")]
        [Browsable(true)]
        public Boolean IsShowPic
        {
            get { return _isShowPic; }
            set 
            {
                if (_isShowPic == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Boolean before = _isShowPic;
                undoItem.ReDo = () =>
                {
                    _isShowPic = value;
                };
                undoItem.UnDo = () =>
                {
                    _isShowPic = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _isShowPic = value;
                setBackGroundType(_isShowPic);
            }
        }

        private void setBackGroundType(Boolean showPic)
        {
            SetPropertyVisibility(this, "BtnDownPic", showPic);
            SetPropertyVisibility(this, "BtnUpPic", showPic);
            SetPropertyVisibility(this, "BackColorground", !showPic);
            SetPropertyVisibility(this, "BackColorgroundDown", !showPic);            
        }

        public void reRefresh()
        {
            SetPropertyVisibility(this, "EnVarText", _btnEnable);
            setBtnTypeShow(_buttonType);
            setBackGroundType(_isShowPic);
        }

        //按钮类型配置
        //Dictionary<String, Byte> _btnConfig = new Dictionary<String, Byte>();

        public UpdateControl UpdateControl;

        public SVButtonProperties()
        {
            _font = new Font("华文细黑", 12);
            _rect = new Rectangle(0, 0, 120, 60);
            _text = "Button";
            _fText = "None";
            _fMemo = "";
            _bgcolor = Color.FromArgb(236, 236, 236);
            _fgcolor = Color.Black;
            _bgDownColor = _bgcolor;
            _btnType = new SVBtnChoicePage();
            _controlType = "按钮";
            _enVarText = new SVVarDefine();
            _btnVarText = new SVVarDefine();

            _btnDownPic = new SVBitmap();
            _btnUpPic = new SVBitmap();
            _isShowPic = false;
            _isLock = false;
        }

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

        [CategoryAttribute("背景")]        
        [DisplayName("按下图片")]
        [DescriptionAttribute("按钮按下后，背景显示的图片")]
        [TypeConverter(typeof(SVBitmap))]
        [EditorAttribute(typeof(SVBitmapTypeEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        public SVBitmap BtnDownPic
        {
            get { return _btnDownPic; }
            set 
            {
                if (_btnDownPic == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();

                SVBitmap before = _btnDownPic;
                undoItem.ReDo = () =>
                {
                    _btnDownPic = value;
                };
                undoItem.UnDo = () =>
                {
                    _btnDownPic = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _btnDownPic = value;
            }
        }

        [CategoryAttribute("背景")]        
        [DisplayName("弹起图片")]
        [DescriptionAttribute("按钮弹起后，背景显示的图片")]
        [TypeConverter(typeof(SVBitmap))]
        [EditorAttribute(typeof(SVBitmapTypeEditor), typeof(UITypeEditor))]
        [Browsable(true)]
        public SVBitmap BtnUpPic
        {
            get { return _btnUpPic; }
            set 
            {
                if (_btnUpPic == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();

                SVBitmap before = _btnUpPic;
                undoItem.ReDo = () =>
                {
                    _btnUpPic = value;
                };
                undoItem.UnDo = () =>
                {
                    _btnUpPic = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _btnUpPic = value;
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

        [Browsable(true)]
        [CategoryAttribute("数据")]
        [DisplayName("选择跳转页面")]
        [EditorAttribute(typeof(SVBtnTypeEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(SVBtnChoicePage))]
        public SVBtnChoicePage ButtonPage
        {
            set
            {
                if (_btnType.isEqual(value))
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                SVBtnChoicePage before = _btnType;
                undoItem.ReDo = () =>
                {
                    _btnType = value;
                };
                undoItem.UnDo = () =>
                {
                    _btnType = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _btnType = value;
            }

            get
            {
                return _btnType;
            }
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("按钮的ID")]
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
        [DescriptionAttribute("是否有确认对话框")]
        [DisplayName("确认")]
        public Boolean Comfirm
        {
            set
            {
                if (_isConfirm == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Boolean before = _isConfirm;
                undoItem.ReDo = () =>
                {
                    _isConfirm = value;
                };
                undoItem.UnDo = () =>
                {
                    _isConfirm = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _isConfirm = value;
            }

            get
            {
                return _isConfirm;
            }
        }

        [CategoryAttribute("背景")]
        [DescriptionAttribute("按钮处于弹起状态背景颜色显示")]
        [DisplayName("弹起背景")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Browsable(true)]
        public Color BackColorground
        {
            set
            {
                if (_bgcolor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Color before = _bgcolor;
                undoItem.ReDo = () => 
                {
                    _bgcolor = value;
                };
                undoItem.UnDo = () => 
                {
                    _bgcolor = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                _bgcolor = value;
            }

            get
            {
                return _bgcolor;
            }
        }

        [CategoryAttribute("背景")]
        [DescriptionAttribute("按钮处于按下去状态的背景颜色显示")]
        [DisplayName("按下背景")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Browsable(true)]
        public Color BackColorgroundDown
        {
            set
            {
                if (_bgDownColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Color before = _bgDownColor;
                undoItem.ReDo = () =>
                {
                    _bgDownColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _bgDownColor = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _bgDownColor = value;
            }

            get
            {
                return _bgDownColor;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("设置按钮上文本颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("颜色")]
        public Color FrontColorground
        {
            set
            {
                if (_fgcolor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();

                Color before = _fgcolor;
                undoItem.ReDo = () =>
                {
                    _fgcolor = value;
                };
                undoItem.UnDo = () =>
                {
                    _fgcolor = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _fgcolor = value;
            }

            get
            {
                return _fgcolor;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("按钮上文本的字体")]
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
                Font before = _font;
                undoItem.ReDo = () =>
                {
                    _font = value;
                };
                undoItem.UnDo = () =>
                {
                    _font = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _font = value;
            }

            get
            {
                return _font;
            }
        }

        [CategoryAttribute("外观")]
        [EditorAttribute(typeof(SVButtonTextUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DescriptionAttribute("按钮上的文本内容，最大长度不能超过64个字符")]
        [DisplayName("文本")]
        public String Text
        {
            set
            {
                //最大长度不能超过64
                if (value.Length > SVLimit.TEXT_MAX_LEN)
                    return;

                if (_text == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                String before = _text;
                undoItem.ReDo = () =>
                {
                    _text = value;
                };
                undoItem.UnDo = () =>
                {
                    _text = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _text = value;
            }

            get
            {
                return _text;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("按钮的尺寸")]
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

        void copyDestByteArray(byte[] src, byte[] dest)
        {
            int minLen = src.Length > dest.Length ? dest.Length : src.Length;
            Array.Copy(src, dest, minLen);
        }

        public void make(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            UInt32 pageNum = pageArrayBin.pageCount;
            UInt32 btnNum = pageArrayBin.pageArray[pageNum].btnNum++;

            if (pageArrayBin.pageArray[pageNum].m_btn == null)
                pageArrayBin.pageArray[pageNum].m_btn = new ButtonBin[SVLimit.PAGE_BTN_MAXNUM];

            ButtonBin btnBin = pageArrayBin.pageArray[pageNum].m_btn[btnNum];

            btnBin.id = ID;
            btnBin.rect.sX = (UInt16)Rect.X;
            btnBin.rect.sY = (UInt16)Rect.Y;
            btnBin.rect.eX = (UInt16)(Rect.Width + btnBin.rect.sX);
            btnBin.rect.eY = (UInt16)(Rect.Height + btnBin.rect.sY);

            btnBin.fontClr = (UInt32)FrontColorground.ToArgb();

            ///为真的文本
            btnBin.text = new Byte[SVLimit.BTN_MAX_LEN];
            copyDestByteArray(Encoding.Unicode.GetBytes(Text), btnBin.text);
            ///为假的文本
            btnBin.fText = new Byte[SVLimit.BTN_MAX_LEN];
            copyDestByteArray(Encoding.Unicode.GetBytes(FText), btnBin.fText);
            ///备注信息
            btnBin.fMemo = new Byte[SVLimit.BTN_MAX_LEN];
            copyDestByteArray(Encoding.Unicode.GetBytes(FMemo), btnBin.fMemo);

            btnBin.font = (Byte)_font.Size;
            //是否有确认窗口
            btnBin.confirm = Comfirm ? (byte)1 : (byte)0;
            //按钮的类型
            if (_buttonType == 0)
            {
                btnBin.type = 0;
                btnBin.param.pageId = _btnType.PageID;
            }
            else
            {
                var varInstance = SVVaribleType.instance();

                btnBin.param.addrOffset = varInstance.strToAddress(_btnVarText.VarName, _btnVarText.VarBlockType);
                btnBin.varTypeBtn = (Byte)varInstance.strToType(_btnVarText.VarName, _btnVarText.VarBlockType);
                btnBin.type = _buttonType;
                btnBin.enable = Convert.ToByte(_btnEnable);
                btnBin.enableAddrOffset = varInstance.strToAddress(_enVarText.VarName, _enVarText.VarBlockType);
                btnBin.varTypeEn = (Byte)varInstance.strToType(_enVarText.VarName, _enVarText.VarBlockType);
            }

            ///显示背景图片或者显示背景颜色
            if (!IsShowPic)
            {
                btnBin.bgUpFlag = 0;
                btnBin.bgDownFlag = 0;
                btnBin.bgUpColor = (UInt32)BackColorground.ToArgb();
                btnBin.bgDownColor = (UInt32)BackColorgroundDown.ToArgb();
            }
            else
            {
                btnBin.bgUpFlag = 1;
                btnBin.bgDownFlag = 1;

                //设置弹起图片

                var upAddress = BtnUpPic.bitmap8Data(Rect.Width, Rect.Height);
                if (upAddress != null)
                {
                    btnBin.bgUpColor = (UInt32)serialize.ToArray().Length;
                    serialize.pack(upAddress);
                }

                //设置按下图片
                var downAddress = BtnDownPic.bitmap8Data(Rect.Width, Rect.Height);                
                if (downAddress != null)
                {
                    btnBin.bgDownColor = (UInt32)serialize.ToArray().Length;
                    serialize.pack(downAddress);
                }
            }

            pageArrayBin.pageArray[pageNum].m_btn[btnNum] = btnBin;
        }
    }
}
