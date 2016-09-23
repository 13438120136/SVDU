using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace SVControl
{
    public class SVSelectItem
    {
        public object _obj { get; set; }
    }

    public class SVListView : ListView
    {
        delegate void Func();
        Dictionary<int, Func> _funcDict = new Dictionary<int, Func>();

        public SVListView()
            : base()
        {
            initControl();
        }

        /// <summary>
        /// 执行具体的拖拽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void slotItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem item = (ListViewItem)e.Item;
            if (_funcDict.ContainsKey(item.Index))
                _funcDict[item.Index]();
        }

        //初始化要创建控件的对应关系
        void initControl()
        {
            ///显示项
            ListViewItem buttonItem = this.Items.Add("按钮");
            ListViewItem textItem = this.Items.Add("文本");
            ListViewItem iconItem = this.Items.Add("图标");
            ListViewItem gifItem = this.Items.Add("动态图");
            ListViewItem vLineItem = this.Items.Add("水平线条");
            ListViewItem hLineItem = this.Items.Add("垂直线条");
            ListViewItem curvesItem = this.Items.Add("曲线图");
            ListViewItem analogItem = this.Items.Add("模拟量");
            ListViewItem binaryItem = this.Items.Add("开关量");
            ListViewItem heartBeatItem = this.Items.Add("心跳控件");

            ///设置图片
            this.View = System.Windows.Forms.View.LargeIcon;
            this.LargeImageList = new ImageList();
            LargeImageList.Images.Add("AreaText", Resource.Text);
            LargeImageList.Images.Add("Button", Resource.button);
            LargeImageList.Images.Add("gif", Resource.gif);
            LargeImageList.Images.Add("Trendchart", Resource.Trendchart);
            LargeImageList.Images.Add("analog", Resource.analog);
            LargeImageList.Images.Add("binary", Resource.binary);
            LargeImageList.Images.Add("line", Resource.line);
            LargeImageList.Images.Add("ico", Resource.ico);
            this.LargeImageList.ImageSize = new Size(80, 40);// 设置行高 20 //分别是宽和高  

            ///设置图片
            buttonItem.ImageKey = "Button";
            textItem.ImageKey = "AreaText";
            iconItem.ImageKey = "ico";
            curvesItem.ImageKey = "Trendchart";
            gifItem.ImageKey = "gif";
            analogItem.ImageKey = "analog";
            binaryItem.ImageKey = "binary";
            vLineItem.ImageKey = "line";

            //线条
            _funcDict.Add(vLineItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                item._obj = (object)(new SVLine());
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });

            _funcDict.Add(hLineItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                SVLine line = new SVLine();
                line.Attrib.ShowType = false;
                item._obj = (object)(line);
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });
            

            //创建按钮
            _funcDict.Add(buttonItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                item._obj = (object)(new SVButton());
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });

            //创建文本
            _funcDict.Add(textItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                item._obj = (object)(new SVLabel());
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });

            //创建图标
            _funcDict.Add(iconItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                item._obj = (object)(new SVIcon());
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });

            //创建动态图
            _funcDict.Add(gifItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                item._obj = (object)(new SVGif());
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });

            //趋势图
            _funcDict.Add(curvesItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                item._obj = (object)(new SVCurve());
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });

            //模拟量显示
            _funcDict.Add(analogItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                item._obj = (object)(new SVAnalog());
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });

            //开关量显示
            _funcDict.Add(binaryItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                item._obj = (object)(new SVBinary());
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });

            //心跳显示
            _funcDict.Add(heartBeatItem.Index, () =>
            {
                SVSelectItem item = new SVSelectItem();
                item._obj = (object)(new SVHeartbeat());
                this.DoDragDrop(item, DragDropEffects.Copy | DragDropEffects.Move);
            });

            //执行事件
            this.ItemDrag += new ItemDragEventHandler(slotItemDrag);
        }
    }
}
