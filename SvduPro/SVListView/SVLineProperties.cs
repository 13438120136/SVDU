using System;
using System.Drawing;
using System.ComponentModel;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVLineProperties
    {
        Color _lineColor;
        Point _startPos;
        UInt16 _lineLen;
        Byte _lineWidth;
        Boolean _showType;
        UInt16 _id;
        Boolean _isLock;
        String _controlType;

        public UpdateControl UpdateControl;

        public SVLineProperties()
        {
            _lineColor = Color.Black;
            _lineWidth = 3;
            _lineLen = 180;
            _isLock = false;
            _controlType = "线条控件";

            ///默认为横向线条
            _showType = true;
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DisplayName("控件")]
        [DescriptionAttribute("显示当前控件类型名称")]
        public String CtrlType
        {
            get { return _controlType; }
            set { _controlType = value; }
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)]
        [DescriptionAttribute("线条ID")]
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

        [CategoryAttribute("外观")]
        [DescriptionAttribute("线条显示的方向：水平或者垂直显示")]
        [TypeConverter(typeof(SVLineTypeConverter))]
        [DisplayName("方向")]
        public Boolean ShowType
        {
            get { return _showType; }
            set 
            {
                if (_showType == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Boolean before = _showType;
                undoItem.ReDo = () =>
                {
                    _showType = value;
                };
                undoItem.UnDo = () =>
                {
                    _showType = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);

                _showType = value; 
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("线条的显示颜色")]
        [TypeConverter(typeof(SVColorConverter))]
        [EditorAttribute(typeof(SVColorTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("线条颜色")]
        public Color LineColor
        {
            set
            {
                if (_lineColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Color before = _lineColor;
                undoItem.ReDo = () =>
                {
                    _lineColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _lineColor = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                _lineColor = value;
            }

            get
            {
                return _lineColor;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("线条的起始位置")]
        [DisplayName("位置")]
        public Point start
        {
            set
            {
                if (_startPos == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Point before = _startPos;
                undoItem.ReDo = () =>
                {
                    _startPos = value;
                };
                undoItem.UnDo = () =>
                {
                    _startPos = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                _startPos = value;
            }
            get
            {
                return _startPos;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("线条的长度")]
        [DisplayName("长度")]
        public UInt16 LineLength
        {
            set
            {
                if (_lineLen == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                UInt16 before = _lineLen;
                undoItem.ReDo = () =>
                {
                    _lineLen = value;
                };
                undoItem.UnDo = () =>
                {
                    _lineLen = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                _lineLen = value;
            }
            get
            {
                return _lineLen;
            }
        }

        [CategoryAttribute("外观")]
        [DescriptionAttribute("线条宽度, 范围1-20")]
        [DisplayName("宽度")]
        public Byte LineWidth
        {
            set
            {
                if (value < 1 || value > 20)
                    return;

                if (_lineWidth == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Byte before = _lineWidth;
                undoItem.ReDo = () =>
                {
                    _lineWidth = value;
                };
                undoItem.UnDo = () =>
                {
                    _lineWidth = before;
                };

                if (UpdateControl != null)
                    UpdateControl(undoItem);
                _lineWidth = value;
            }
            get
            {
                return _lineWidth;
            }
        }

        public void make(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            UInt32 pageNum = pageArrayBin.pageCount;
            UInt32 lineNum = pageArrayBin.pageArray[pageNum].lineNum++;

            if (pageArrayBin.pageArray[pageNum].m_line == null)
                pageArrayBin.pageArray[pageNum].m_line = new LineBin[SVLimit.PAGE_LINE_MAXNUM];

            LineBin lineBtn = pageArrayBin.pageArray[pageNum].m_line[lineNum];
            lineBtn.id = ID;
            lineBtn.color = (UInt32)LineColor.ToArgb();
            lineBtn.width = LineWidth;

            ///起始坐标
            lineBtn.x1 = (UInt16)_startPos.X;
            lineBtn.y1 = (UInt16)_startPos.Y;

            ///结束坐标
            if (ShowType)
            {
                lineBtn.x2 = (UInt16)(lineBtn.x1 + LineLength - 1);
                lineBtn.y2 = lineBtn.y1;
            }
            else
            {
                lineBtn.x2 = lineBtn.x1;
                lineBtn.y2 = (UInt16)(lineBtn.y1 + LineLength - 1);
            }

            pageArrayBin.pageArray[pageNum].m_line[lineNum] = lineBtn;
        }
    }
}
