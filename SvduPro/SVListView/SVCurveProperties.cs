﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        String _controlType;   //控件类型

        String _var;
        String[] _varArray = new String[4];   //变量列表
        Color[] _varColorArray = new Color[4];
        Byte[] _lineWidth = new Byte[4];

        Boolean _isLock;

        //字体的配置
        Dictionary<Font, Byte> _fontConfig = new Dictionary<Font, Byte>();

        public UpdateControl UpdateControl;

        public void setVarArray(String[] value)
        {
            _varArray = value;
        }

        public String[] getVarArray()
        {
            return _varArray;
        }

        public void setColorArray(Color[] value)
        {
            _varColorArray = value;
        }

        public Color[] getColorArray()
        {
            return _varColorArray;
        }

        public void setLineWidthArray(Byte[] value)
        {
            _lineWidth = value;
        }

        public Byte[] getLineWidthArray()
        {
            return _lineWidth;
        }

        public SVCurveProperties()
        {
            _font = new Font("宋体", 16);
            _rect = new Rectangle(0, 0, 150, 150);
            _bgcolor = Color.DimGray;
            _fgcolor = Color.White;
            _min = 0;
            _max = 200;
            _interval = 60;
            _controlType = "趋势图";
            _isLock = false;

            //字体的映射关系
            _fontConfig.Add(new Font("宋体", 16), 16);
            _fontConfig.Add(new Font("宋体", 24), 24);
            _fontConfig.Add(new Font("宋体", 32), 32);
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
        [DisplayName("背景")]
        public Color BackgdColor
        {
            set
            {
                if (_bgcolor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
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
        [DisplayName("字体")]
        public Font Font
        {
            set
            {
                if (_font == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
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
        [DescriptionAttribute("趋势图坐标字体颜色")]
        [DisplayName("颜色")]
        public Color FrontColor
        {
            set
            {
                if (_fgcolor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
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
        [DescriptionAttribute("变量")]
        [EditorAttribute(typeof(SVCurveVarTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("变量列表")]
        public String Var
        {
            get { return _var; }
            set { _var = value; }
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
        [DescriptionAttribute("单位为:秒，范围60 - 3600之间。")]
        [DisplayName("最大显示时间")]
        public UInt16 Interval
        {
            set
            {
                if (value < 60 || value > 3600)
                    return;

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
            curveBin.font = _fontConfig[_font];

            //变量地址
            for (int i = 0; i < _varArray.Length; i++)
            {
                String str =_varArray[i];
            }
            //线条颜色
            for (int i = 0; i < _varArray.Length; i++)
            {
                UInt32 value = (UInt32)_varColorArray[i].ToArgb();
                curveBin.lineClr[i] = value;
            }
            //线条宽度
            Array.Copy(curveBin.lineWidth, _lineWidth, _varArray.Length);

            pageArrayBin.pageArray[pageCount].m_trendChart[curveCount] = curveBin;
        }
    }
}
