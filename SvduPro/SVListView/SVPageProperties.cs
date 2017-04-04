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
        Byte _isAlignment = 0;
        UInt16 _id;
        SVBitmap _bitMap;
        Byte _backGroundType = 0;  //为0的时候表示颜色背景，1的时候表示图片背景
        Boolean _isMainPage = false;

        [CategoryAttribute("属性")]
        [DescriptionAttribute("当前页面是否为启动页面，如果是启动页面-True,否则-False")]
        [DisplayName("启动页面")]
        [ReadOnlyAttribute(true)] 
        public Boolean IsMainPage
        {
            get { return _isMainPage; }
            set { _isMainPage = value; }
        }

        public UpdateControl UpdateControl;
        #endregion

        public SVPageProperties()
        {
            _bitMap = new SVBitmap();
        }

        [CategoryAttribute("数据")]
        [DescriptionAttribute("设置背景颜色或者图片")]
        [TypeConverter(typeof(SVPageBackGroundTypeConverter))]
        [EditorAttribute(typeof(SVPageUIEditer), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("背景设置")]
        public Byte BackGroundType
        {
            get 
            {
                return _backGroundType;
            }
            set
            {
                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                if (UpdateControl != null)
                    UpdateControl(undoItem);

                if (_backGroundType == value)
                    return;

                Byte before = _backGroundType;
                undoItem.ReDo = () =>
                {
                    _backGroundType = value;
                };
                undoItem.UnDo = () =>
                {
                    _backGroundType = before;
                };

                _backGroundType = value; 
            }
        }

        [Browsable(false)]
        [CategoryAttribute("数据")]
        [DescriptionAttribute("设置页面的背景图片")]
        [TypeConverter(typeof(SVBitmap))]
        [DisplayName("背景图片")]
        public SVBitmap PicIconData
        {
            set
            {
                if (_bitMap == value)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();

                SVBitmap before = _bitMap;
                undoItem.ReDo = () =>
                {
                    _bitMap = value;
                };
                undoItem.UnDo = () =>
                {
                    _bitMap = before;
                };

                _bitMap = value;

                if (UpdateControl != null)
                    UpdateControl(undoItem);
            }

            get
            {
                return _bitMap;
            }
        }

        [CategoryAttribute("数据")]
        [DisplayName("对齐")]
        [DescriptionAttribute("页面中的控件小数点是否对齐\n 值范围:0-6")]
        public Byte IsAlignment
        {
            get { return _isAlignment; }
            set 
            {
                if (_isAlignment == value)
                    return;

                if (_isAlignment < 0 || _isAlignment > 6)
                    return;

                SVRedoUndoItem undoItem = new SVRedoUndoItem();
                Byte before = _isAlignment;
                undoItem.ReDo = () =>
                {
                    _isAlignment = value;
                };
                undoItem.UnDo = () =>
                {
                    _isAlignment = before;
                };

                if (UpdateControl != null)
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

        [Browsable(false)]
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

                _backGroundColor = value;

                if (UpdateControl != null)
                    UpdateControl(undoItem);
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
