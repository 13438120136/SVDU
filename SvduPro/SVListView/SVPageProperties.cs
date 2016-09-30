/*
 * 页面属性管理，包含了当前页面的主要数据。
 * */

using System;
using System.ComponentModel;
using System.Drawing;
using SVCore;

namespace SVControl
{
    /// <summary>
    /// 页面属性类
    /// </summary>
    public class SVPageProperties
    {
        #region 页面属性变量
        int _width = 800;
        int _height = 600;
        Color _backGroundColor = Color.LightGray;
        Boolean _isAlignment = false;
        UInt16 _id;
        public UpdateControl UpdateControl;
        #endregion

        public SVPageProperties()
        {
        }

        [CategoryAttribute("数据")]
        [DisplayName("对齐")]
        [DescriptionAttribute("页面中的控件小数点是否对齐\nTrue为对齐  False为不对齐\n默认:False")]
        public Boolean IsAlignment
        {
            get { return _isAlignment; }
            set 
            {
                if (_isAlignment == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Boolean before = _isAlignment;
                undoItem.ReDo = () =>
                {
                    _isAlignment = value;
                };
                undoItem.UnDo = () =>
                {
                    _isAlignment = before;
                };
                UpdateControl(undoItem);

                _isAlignment = value; 
            }
        }

        [CategoryAttribute("属性")]
        [ReadOnlyAttribute(true)] 
        [DescriptionAttribute("显示当前页面的ID号")]
        public UInt16 id
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

        [CategoryAttribute("外观"),
        DescriptionAttribute("设置当前页面的背景显示颜色")]
        [DisplayName("背景")]
        public Color BackColor
        {
            set
            {
                if (_backGroundColor.ToArgb() == value.ToArgb())
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Color before = _backGroundColor;
                undoItem.ReDo = () =>
                {
                    _backGroundColor = value;
                };
                undoItem.UnDo = () =>
                {
                    _backGroundColor = before;
                };
                UpdateControl(undoItem);

                _backGroundColor = value;
            }

            get
            {
                return _backGroundColor;
            }
        }

        [CategoryAttribute("外观"), ReadOnlyAttribute(true),
        DescriptionAttribute("当前页面的宽度")]
        [DisplayName("宽度")]
        public int Width 
        { 
            set
            {
                _width = value;
            }
            
            get
            {
                return _width;
            }
        }

        [CategoryAttribute("外观"), ReadOnlyAttribute(true),
        DescriptionAttribute("当前页面的高度")]
        [DisplayName("高度")]
        public int Height
        {
            set
            {
                _height = value;
            }
            get
            {
                return _height;
            }
        }
    }
}
