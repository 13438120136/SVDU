using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SVCore
{
    /// <summary>
    /// 自定义panel抽象类
    /// </summary>
    [Serializable]
    abstract public class SVPanel : SVBasePanel, SVInterfacePanel, ICustomTypeDescriptor
    {
        Boolean _bTransparent;
        private Point _minSize = new Point();
        private UInt16 _parentID;
        private UInt16 _id;        
        
        /// <summary>
        /// 获取撤销和恢复对象
        /// </summary>
        public SVRedoUndo RedoUndo { private set; get; }

        /// <summary>
        /// 是否透明
        /// true-透明
        /// false-不透明
        /// </summary>
        public Boolean BTransparent
        {
            get { return _bTransparent; }
            set { _bTransparent = value; }
        }

        /// <summary>
        /// 设置和获取当前控件的ID号
        /// </summary>
        public UInt16 Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 获取当前控件的父ID号，这里通常为页面ID号
        /// </summary>
        public UInt16 ParentID 
        {
            get { return _parentID; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SVPanel()
        {
            _id = 0;
            _parentID = 0;

            ///默认不透明
            this.BTransparent = false;
            ///文本居中对齐显示
            this.TextAlign = ContentAlignment.MiddleCenter;
            ///将属性应用到外观显示
            refreshPropertyToPanel();

            ///控件坐标或者尺寸发生改变,更新
            this.SizeChangedEvent += ()=>
            {
                Point vPoint = new Point(this.Location.X, this.Location.Y);
                this.setStartPos(vPoint);
            };
        }

        /*
         * 是否包含父节点，并且父节点为页面控件
         * true - 有父对象
         * false - 还未加入页面控件
         */
        public Boolean isHasParent()
        {
            return (ParentID != 0);
        }

        /// <summary>
        /// 设置页面的撤销和恢复操作
        /// </summary>
        /// <param oldName="value"></param>
        public void setRedoUndoObject(SVRedoUndo value)
        {
            RedoUndo = value;
            initalizeRedoUndo();
        }

        /// <summary>
        /// 设置父ID
        /// </summary>
        /// <param oldName="value">父ID号</param>
        public void setParentID(UInt16 value)
        {
            _parentID = value;
        }

        /// <summary>
        /// 设置控件最小尺寸范围
        /// </summary>
        /// <param oldName="x"></param>
        /// <param oldName="y"></param>
        public void setMinSize(int x, int y)
        {
            _minSize = new Point(x, y);
        }

        /*
         * 将当前控件在页面控件中处于最前显示状态
         */
        public void showOnTheTop()
        {
            this.Parent.Controls.SetChildIndex(this, 0);
        }

        /// <summary>
        /// 重写绘制事件
        /// </summary>
        /// <param oldName="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (BTransparent)
            {
                base.OnPaint(e);
            }
            else
            {
                if (this.BackgroundImage != null)
                    drawBackGroundImage(e);
                else
                {
                    drawBackGroundColor(e);
                    panelOnPaint(e);
                }
                
                drawText(e);
            }
        }

        /// <summary>
        /// 用来扩展子类的绘制
        /// </summary>
        /// <param oldName="e"></param>
        virtual protected void panelOnPaint(PaintEventArgs e)
        {
        }

        /// <summary>
        /// 绘制背景颜色
        /// </summary>
        /// <param oldName="e"></param>
        void drawBackGroundColor(PaintEventArgs e)
        {
            if (this.BackgroundImage != null)
                return;

            Graphics graphics = e.Graphics;
            ///绘制背景颜色
            SolidBrush brush = new SolidBrush(this.BackColor);
            graphics.FillRectangle(brush, this.ClientRectangle);
        }

        /// <summary>
        /// 绘制文本
        /// </summary>
        /// <param oldName="e"></param>
        void drawText(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            SizeF sizeF = graphics.MeasureString(this.Text, this.Font);
            SolidBrush fontBrush = new SolidBrush(this.ForeColor);

            switch (this.TextAlign)
            {
                case ContentAlignment.TopLeft:
                    {
                        StringFormat strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Near;
                        strFormat.LineAlignment = StringAlignment.Near;
                        graphics.DrawString(this.Text, this.Font, fontBrush, this.ClientRectangle, strFormat);
                        break;
                    }
                case ContentAlignment.TopRight:
                    {
                        StringFormat strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Far;
                        strFormat.LineAlignment = StringAlignment.Near;
                        graphics.DrawString(this.Text, this.Font, fontBrush, this.ClientRectangle, strFormat);
                        break;
                    }
                case ContentAlignment.TopCenter:
                    {
                        StringFormat strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Center;
                        strFormat.LineAlignment = StringAlignment.Near;
                        graphics.DrawString(this.Text, this.Font, fontBrush, this.ClientRectangle, strFormat);
                        break;
                    }
                case ContentAlignment.MiddleCenter:
                    {
                        StringFormat strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Center;
                        strFormat.LineAlignment = StringAlignment.Center;
                        graphics.DrawString(this.Text, this.Font, fontBrush, this.ClientRectangle, strFormat);
                        break;
                    }
                default:
                    return;
            }
        }

        /// <summary>
        /// 绘制背景图片
        /// </summary>
        /// <param oldName="e"></param>
        void drawBackGroundImage(PaintEventArgs e)
        {
            if (this.BackgroundImage == null)
                return;

            Graphics graphics = e.Graphics;
            graphics.DrawImage(this.BackgroundImage, this.ClientRectangle);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<PropertyDescriptor> resultList = new List<PropertyDescriptor>();

            PropertyDescriptorCollection tmp = TypeDescriptor.GetProperties(property(), attributes);
            foreach (PropertyDescriptor des in tmp)
                resultList.Add(des);
            return new PropertyDescriptorCollection(resultList.ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return property();
        }

        /// <summary>
        /// 自定义快捷键
        /// 添加针对单个控件的[Ctrl+C]组合键，执行控件的拷贝功能
        /// </summary>
        /// <param oldName="msg"></param>
        /// <param oldName="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.C:
                    SVSelectPanelObjs.copyOperator();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 创建一个页面新的ID号
        /// </summary>
        public UInt16 createID()
        {
            if (this.Parent == null)
                return 0;

            HashSet<UInt16> idList = new HashSet<UInt16>();
            foreach (var item in this.Parent.Controls)
            {
                SVPanel panel = item as SVPanel;
                if (panel == null)
                    continue;

                idList.Add(panel.Id);
            }

            for (UInt16 i = 1; i < 10000; i++)
            {
                if (!idList.Contains(i))
                {
                    this.Id = i;
                    return i;
                }
            }

            return 0;
        }

        //设置起始位置
        abstract public void setStartPos(Point pos);
        //获取属性对象
        abstract public object property();
        abstract public void refreshPropertyToPanel();
        abstract public void loadXML(SVXml element, Boolean isCreate = false);
        abstract public void saveXML(SVXml element);
        abstract public object cloneObject();
        abstract public void newID();
        //设置恢复和重做相关函数
        abstract public void initalizeRedoUndo();
        //检查控件属性的合法性
        abstract public void checkValid();
    }
}
