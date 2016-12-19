using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SVCore;

namespace SVControl
{
    /// <summary>
    /// 自定义的窗体，针对图片数组进行的修改
    /// </summary>
    public partial class SVBitmapArrayWindow : Form
    {
        /// <summary>
        /// 记录当前类内部的图片数组
        /// </summary>
        SVBitmapArray _bitArray = new SVBitmapArray();

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param Name="array">初始化图片数组</param>
        /// <param Name="count">最大图片个数</param>
        public SVBitmapArrayWindow(SVBitmapArray array, Int32 count = 8)
        {
            InitializeComponent();

            ///相关事件注册
            this.dataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
            this.dataGridView.Click += new EventHandler(dataGridView_Click);
            this.propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid1_PropertyValueChanged);

            _bitArray = array;

            ///在表格中，添加最大行
            for (int i = 0; i < count; i++)
            {
                DataGridViewRowChild v = new DataGridViewRowChild();
                dataGridView.Rows.Add(v);
            }

            if (array.BitmapArray == null)
                return;

            Int32 minValue = Math.Min(count, array.BitmapArray.Count);
            for (int i = 0; i < minValue; i++)
            {
                SVBitmap vTmp = array.BitmapArray[i];
                if (vTmp == null)
                    continue;

                DataGridViewRowChild vv = dataGridView.Rows[i] as DataGridViewRowChild;
                vv.Attrib.Bitmap = vTmp;
                dataGridView.Rows[i].SetValues(vTmp.ShowName);
            }
        }

        /// <summary>
        /// 属性窗口改变后，更新表格中界面的显示
        /// </summary>
        /// <param Name="s"></param>
        /// <param Name="e"></param>
        void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            DataGridViewRowChild child = this.propertyGrid1.SelectedObject as DataGridViewRowChild;
            if (child == null)
                return;

            child.SetValues(child.Attrib.Bitmap.ShowName);
        }

        /// <summary>
        /// 表格单元格选中后的事件响应
        /// 这里主要是将表格中选中的单元格对象图片属性进行显示
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void dataGridView_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell vCell in this.dataGridView.SelectedCells)
            {
                DataGridViewRowChild vv = vCell.OwningRow as DataGridViewRowChild;
                if (vv == null)
                    continue;

                if (!vv.Attrib.Bitmap.isValidShow())
                    continue;

                this.propertyGrid1.SelectedObject = vv;
            }
        }

        /// <summary>
        /// 由于默认的表格没有行号的显示，自绘制表格左边纵向控件头部的行号
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            StringFormat stringFormat = new StringFormat();
            //SolidBrush backBrush = new SolidBrush(Color.White);
            //e.Graphics.FillRectangle(backBrush, new Rectangle(e.RowBounds.X, e.RowBounds.Y, dataGridView.RowHeadersWidth, e.RowBounds.Height));
            SolidBrush solidBrush = new SolidBrush(dataGridView.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(),
            e.InheritedRowStyle.Font, solidBrush, 
            new Rectangle(e.RowBounds.X, e.RowBounds.Y, dataGridView.RowHeadersWidth, e.RowBounds.Height), 
            stringFormat);
        }

        public SVBitmapArray bitmapArray()
        {
            return _bitArray;
        }

        /// <summary>
        /// 单击OK按钮的事件响应
        /// 记录当前用户选择的图片数组数据
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            List<SVBitmap> list = new List<SVBitmap>();
            for (int i = 0; i < this.dataGridView.RowCount; i++)
            {
                DataGridViewCell vCell = this.dataGridView[0, i];
                DataGridViewRowChild vv = vCell.OwningRow as DataGridViewRowChild;
                if (vv == null)
                    continue;

                if (!vv.Attrib.Bitmap.isValidShow())
                    continue;

                list.Add(vv.Attrib.Bitmap);
            }

            _bitArray.BitmapArray = list;
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 用户单击退出按钮响应事件
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }
    }

    /// <summary>
    /// 自定义的表格行
    /// </summary>
    public class DataGridViewRowChild : DataGridViewRow, ICustomTypeDescriptor
    {
        private DataAttrib _attrib = new DataAttrib();

        public DataAttrib Attrib
        {
            get { return _attrib; }
            set { _attrib = value; }
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

            PropertyDescriptorCollection tmp = TypeDescriptor.GetProperties(Attrib, attributes);
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
            return Attrib;
        }
    }

    /// <summary>
    /// 自定义属性字段
    /// </summary>
    public class DataAttrib
    {
        SVBitmap _bitmap = new SVBitmap();

        [CategoryAttribute("数据")]
        [DescriptionAttribute("选择图片")]
        [EditorAttribute(typeof(SVBitmapTypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(SVBitmap))]
        [DisplayName("图片")]
        public SVBitmap Bitmap
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }
    }
}
