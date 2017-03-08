using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using SVControl;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVCurveProperties
    {
        UInt16 _id;          //ID
        Color _bgcolor;      //背景颜色
        Color _fgcolor;      //文本颜色
        Font _font;          //字体
        Rectangle _rect;     //尺寸
        Single _max;
        Single _min;
        UInt16 _interval;
        UInt16 _step;          //步进

        String _controlType;   //控件类型

        SVVarDefine _forwardControl;     //前进关联变量
        SVVarDefine _curControl;         //当前关联变量
        SVVarDefine _backwardControl;    //后退关联变量
        List<SVCurveProper> _variable = new List<SVCurveProper>();   //关联的变量
        static Int32 _indexControl = 0;

        public static String getIndexNumber()
        {
            Int32 result = _indexControl;
            _indexControl += 8;
            return result.ToString();
        }

        [Browsable(false)]
        public SVVarDefine ForwardControl
        {
            get { return _forwardControl; }
            set { _forwardControl = value; }
        }

        [Browsable(false)]
        public SVVarDefine CurControl
        {
            get { return _curControl; }
            set { _curControl = value; }
        }

        [Browsable(false)]
        public SVVarDefine BackwardControl
        {
            get { return _backwardControl; }
            set { _backwardControl = value; }
        }

        Boolean _isLock;

        public UpdateControl UpdateControl;

        public SVCurveProperties()
        {
            _font = new Font("华文细黑", 12);
            _rect = new Rectangle(0, 0, 270, 260);
            _bgcolor = Color.DimGray;
            _fgcolor = Color.White;
            _min = 0;
            _max = 200;
            _interval = 60;
            _step = 60;
            _controlType = "趋势图";
            _isLock = false;

            _forwardControl = new SVVarDefine();
            _curControl = new SVVarDefine();
            _backwardControl = new SVVarDefine();

            ///设置跳转变量
            _forwardControl.VarName = SVCurveProperties.getIndexNumber();
            _curControl.VarName = SVCurveProperties.getIndexNumber();
            _backwardControl.VarName = SVCurveProperties.getIndexNumber();
            _forwardControl.VarType = 3;
            _curControl.VarType = 3;
            _backwardControl.VarType = 3;
        }

        [CategoryAttribute("数据")]
        [DescriptionAttribute("趋势图中显示的变量")]
        [EditorAttribute(typeof(SVCurveVarTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SVCurveVarConverter))]
        [DisplayName("变量列表")]
        public List<SVCurveProper> Variable
        {
            get { return _variable; }
            set { _variable = value; }
        }

        [CategoryAttribute("数据")]
        [DisplayName("步进")]
        [DescriptionAttribute("设置可切换的步进值, 范围1-600，单位：秒")]
        public UInt16 Step
        {
            get { return _step; }
            set 
            {
                if (_step < 1 || _step > 600)
                    return;

                if (_step == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                UInt16 before = _step;
                undoItem.ReDo = () =>
                {
                    _step = value;
                };
                undoItem.UnDo = () =>
                {
                    _step = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _step = value;
            }
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
        [DescriptionAttribute("趋势图ID")]
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
        [DescriptionAttribute("趋势图背景颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("背景")]
        public Color BackgdColor
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
        [DescriptionAttribute("趋势图字体")]
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
        [DescriptionAttribute("趋势图坐标字体颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("颜色")]
        public Color FrontColor
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

        [CategoryAttribute("外观")]
        [DescriptionAttribute("设置趋势图的起始位置及尺寸")]
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
        [DescriptionAttribute("起始纵坐标")]
        [DisplayName("起始纵坐标")]
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

        [CategoryAttribute("数据")]
        [DescriptionAttribute("结束纵坐标")]
        [DisplayName("结束纵坐标")]
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

        [CategoryAttribute("数据")]
        [DescriptionAttribute("单位为:秒，范围60 - 3600之间，\r\n并且必须满足10的倍数。")]
        [DisplayName("最大显示时间")]
        public UInt16 Interval
        {
            set
            {
                if (value < 60 || value > 3600)
                    return;

                if (value % 10 != 0)
                {
                    SVMessageBox msgBox = new SVMessageBox();
                    msgBox.content(" ", "值必须满足10的倍数!");
                    msgBox.ShowDialog();
                    return;
                }

                if (_max == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);
                UInt16 before = _interval;
                undoItem.ReDo = () =>
                {
                    _interval = value;
                };
                undoItem.UnDo = () =>
                {
                    _interval = before;
                };

                _interval = value;
            }

            get
            {
                return _interval;
            }
        }

        public void make(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            UInt32 pageCount = pageArrayBin.pageCount;
            UInt32 curveCount = pageArrayBin.pageArray[pageCount].trendChartNum++;

            if (pageArrayBin.pageArray[pageCount].m_trendChart == null)
                pageArrayBin.pageArray[pageCount].m_trendChart = new TrendChartBin[SVLimit.PAGE_TCHART_MAXNUM];

            TrendChartBin curveBin = pageArrayBin.pageArray[pageCount].m_trendChart[curveCount];
            curveBin.lineClr = new UInt32[4];
            curveBin.lineWidth = new Byte[4];
            curveBin.addrOffset = new UInt32[4];
            curveBin.varType = new Byte[4];
            curveBin.keyOffset = new UInt32[3];

            curveBin.id = ID;
            curveBin.rect.sX = (UInt16)Rect.X;
            curveBin.rect.sY = (UInt16)Rect.Y;
            curveBin.rect.eX = (UInt16)(Rect.Width + curveBin.rect.sX);
            curveBin.rect.eY = (UInt16)(Rect.Height + curveBin.rect.sY);

            //字体颜色
            curveBin.scaleClr = (UInt32)FrontColor.ToArgb();
            //背景颜色
            curveBin.bgClr = (UInt32)BackgdColor.ToArgb();

            curveBin.yMin = Min;
            curveBin.yMax = Max;
            curveBin.maxTime = Interval;
            curveBin.font = (Byte)_font.Size;
            curveBin.stepTime = Step;

            var varInstance = SVVaribleType.instance();
            curveBin.keyOffset[0] = varInstance.strToAddress(ForwardControl.VarName, ForwardControl.VarType);
            curveBin.keyOffset[1] = varInstance.strToAddress(CurControl.VarName, CurControl.VarType);
            curveBin.keyOffset[2] = varInstance.strToAddress(BackwardControl.VarName, BackwardControl.VarType);

            ///编译变量、颜色及使能标志
            Int32 nCount = _variable.Count;
            for (Int32 i = 0; i < nCount; i++)
            {
                String name = _variable[i].Var.VarName;
                Byte type = _variable[i].Var.VarType;                

                curveBin.addrOffset[i] = varInstance.strToAddress(name, type);
                curveBin.varType[i] = (Byte)varInstance.strToType(name, type);
                curveBin.lineClr[i] = (UInt32)_variable[i].Color.ToArgb();
                curveBin.lineWidth[i] = _variable[i].Enabled ? (Byte)1 : (Byte)0;
            }

            ///"SHORT_INT", "SHORTINT_VAR", "INT", "INT_VAR", "REAL", "REAL_VAR" 
            pageArrayBin.pageArray[pageCount].m_trendChart[curveCount] = curveBin;
        }
    }
}


public class SVCurveProper
{
    SVVarDefine _var = new SVVarDefine();
    Color _color = Color.Black;
    Boolean _enabled = false;

    [CategoryAttribute("数据")]
    [DisplayName("线条使能")]
    [DescriptionAttribute("是否处于使能状态")]
    public Boolean Enabled
    {
        get { return _enabled; }
        set { _enabled = value; }
    }

    [CategoryAttribute("数据")]
    [DescriptionAttribute("当前变量显示的线条颜色")]
    [TypeConverter(typeof(SVColorConverter))]
    [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [DisplayName("线条颜色")]
    public Color Color
    {
        get { return _color; }
        set { _color = value; }
    }

    [Browsable(true)]
    [CategoryAttribute("数据")]
    [DisplayName("线条变量")]
    [DescriptionAttribute("趋势图关联的变量.")]
    [EditorAttribute(typeof(SVCurveVarUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    [TypeConverter(typeof(SVDefineVarConverter))]
    public SVVarDefine Var
    {
        get { return _var; }
        set { _var = value; }
    }
}
