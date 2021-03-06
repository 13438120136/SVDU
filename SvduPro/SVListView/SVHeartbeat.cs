﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using SVCore;

namespace SVControl
{
    /// <summary>
    /// 心跳控件
    /// </summary>
    [Serializable]
    public class SVHeartbeat : SVPanel, SVInterfaceBuild, ISerializable
    {
        SVHeartbeatProperties _attrib = new SVHeartbeatProperties();

        public SVHeartbeatProperties Attrib
        {
            get { return _attrib; }
            set { _attrib = value; }
        }

        public SVHeartbeat()
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            refreshPropertyToPanel();
        }

        protected SVHeartbeat(SerializationInfo info, StreamingContext context)
        {
            _attrib = (SVHeartbeatProperties)info.GetValue("stream", typeof(SVHeartbeatProperties));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("stream", _attrib);
        }

        /// <summary>
        /// 重写函数initalizeRedoUndo
        /// 当按钮属性被修改的时候，执行记录
        /// 撤销和重做操作执行的时候，主窗口中控件刷新
        /// </summary>
        override public void initalizeRedoUndo()
        {
            this.RedoUndo.UpdateOperator += () =>
            {
                refreshPropertyToPanel();
            };

            Attrib.UpdateControl += new UpdateControl((item) =>
            {
                RedoUndo.recordOper(item);
            });
        }

        /// <summary>
        /// 创建一个新的ID号
        /// </summary>
        public override void newID()
        {
            _attrib.ID = base.createID();
        }

        /// <summary>
        /// 设置当前对象的起始位置
        /// </summary>
        /// <param Name="pos">点坐标，控件的起始位置</param>
        public override void setStartPos(Point pos)
        {
            _attrib.Rect = new Rectangle(pos.X, pos.Y, this.Width, this.Height);
        }

        /// <summary>
        /// 重写父类方法
        /// 返回心跳对象中的属性对象
        /// </summary>
        /// <returns>属性对象</returns>
        public override object property()
        {
            return _attrib;
        }

        /// <summary>
        /// 根据当前按钮属性对象中的值，更新按钮外观显示
        /// </summary>
        public override void refreshPropertyToPanel()
        {
            this.Id = _attrib.ID;
            this.Width = _attrib.Rect.Width;
            this.Height = _attrib.Rect.Height;
            this.IsMoved = !_attrib.Lock;
            this.Location = new Point(_attrib.Rect.X, _attrib.Rect.Y);

            ///保证心跳控件必须至少设置了一副图片
            if (_attrib.BitMapArray.BitmapArray.Count > 0)
            {
                SVBitmap svbitMap = _attrib.BitMapArray.BitmapArray[0];

                ///判断文件是否已经被删除，如果删除控件不在显示图片
                String file = Path.Combine(SVProData.IconPath, svbitMap.ImageFileName);
                if (!File.Exists(file))
                    return;

                SVPixmapFile pixmapFile = new SVPixmapFile();
                pixmapFile.readPixmapFile(file);
                this.BackgroundImage = pixmapFile.getBitmapFromData();
            }
            else
                this.BackgroundImage = null;
        }

                /// <summary>
        /// 从xml文件中加载当前按钮对象内容
        /// </summary>
        /// <param Name="xml">xml对象</param>
        /// <param Name="isCreate">true-创建新的ID号，false-表示使用文件中的ID</param>
        public override void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement button = xml.CurrentElement;

            if (isCreate)
                newID();
            else
                _attrib.ID = UInt16.Parse(button.GetAttribute("id"));

            int x = int.Parse(button.GetAttribute("x"));
            int y = int.Parse(button.GetAttribute("y"));
            int width = int.Parse(button.GetAttribute("width"));
            int height = int.Parse(button.GetAttribute("height"));
            _attrib.Rect = new Rectangle(x, y, width, height);

            ///遍历节点，循环读取图片文件数据
            XmlNodeList nls = button.ChildNodes;
            foreach (var tmp in nls)
            {
                XmlElement vElement = (XmlElement)tmp;
                SVBitmap svBitmap = new SVBitmap();
                svBitmap.ShowName = vElement.GetAttribute("Name");
                svBitmap.ImageFileName = vElement.GetAttribute("File");
                _attrib.BitMapArray.BitmapArray.Add(svBitmap);
            }
        }

        /// <summary>
        /// 保存当前按钮内容到xml文件中
        /// </summary>
        /// <param Name="xml">xml对象</param>
        override public void saveXML(SVXml xml)
        {
            XmlElement button = xml.createNode(this.GetType().Name);
            button.SetAttribute("id", _attrib.ID.ToString());
            button.SetAttribute("x", _attrib.Rect.X.ToString());
            button.SetAttribute("y", _attrib.Rect.Y.ToString());
            button.SetAttribute("width", _attrib.Rect.Width.ToString());
            button.SetAttribute("height", _attrib.Rect.Height.ToString());

            ///循环保存图片数组
            List<SVBitmap> list = _attrib.BitMapArray.imageArray();
            foreach (var item in list)
            {
                XmlElement picList = xml.crateChildNode("PIC");
                button.AppendChild(picList);

                picList.SetAttribute("Name", item.ShowName);
                picList.SetAttribute("File", item.ImageFileName);
            }
        }

        /// <summary>
        /// 通过序列化的机制来克隆一个当前对象并返回
        /// </summary>
        /// <returns>一个新的按钮对象</returns>
        override public object cloneObject()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, this);
            stream.Position = 0;
            SVHeartbeat result = (SVHeartbeat)binFormat.Deserialize(stream);

            result.refreshPropertyToPanel();

            return result;
        }

        public void buildControlToBin(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            _attrib.make(ref pageArrayBin, ref serialize);
        }

        /// <summary>
        /// 检查当前按钮对象中内容是否合法
        /// 
        /// 不合法-抛出SVCheckValidException异常
        /// </summary>
        public override void checkValid()
        {
            SVPageWidget pageWidget = this.Parent as SVPageWidget;
            String pageName = pageWidget.PageName;

            SVUniqueID uniqueObj = SVUniqueID.instance();

            if (Attrib.ID <= 0 || Attrib.ID >= uniqueObj.MaxID)
            {
                String msg = String.Format("页面 {0} 中,心跳控件ID为:{1}, ID值已经超出最大范围[{2} - {3}]", pageName, Attrib.ID, 1, uniqueObj.MaxID);
                throw new SVCheckValidException(msg);
            }

            if (!isHasParent())
            {
                String msg = String.Format("页面 {0} 中,心跳控件ID为:{1}, 没有在页面控件中", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (this.Parent == null)
            {
                String msg = String.Format("页面 {0} 中,心跳控件ID为:{1}, 没有父控件", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (!this.Parent.ClientRectangle.Contains(this.Bounds))
            {
                String msg = String.Format("页面 {0} 中,心跳控件ID为:{1}, 已经超出页面显示范围", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            var image = Attrib.BitMapArray.BitmapArray;
            if (image.Count == 0)
            {
                String msg = String.Format("页面 {0} 中,心跳控件ID为:{1}, 未设置任何图片", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            for (int i = 0; i < image.Count; i++)
            {
                var tmpImage = image[i];
                if (tmpImage.bitmap() == null)
                {
                    String msg = String.Format("页面 {0} 中,心跳控件ID为:{1}, 第{2}个图片设置有误", pageName, Attrib.ID, i);
                    throw new SVCheckValidException(msg);
                }
            }
        }
    }
}
