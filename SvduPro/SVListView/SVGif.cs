using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;
using SVCore;

namespace SVControl
{
    [Serializable]
    public class SVGif : SVPanel, SVInterfacePanel, ISerializable, SVInterfaceBuild
    {
        SVGifProperties _attrib = new SVGifProperties();

        public SVGifProperties Attrib
        {
            set
            {
                _attrib = value;
            }
            get { return _attrib; }
        }

        public SVGif() :
            base()
        {
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        override public void initalizeRedoUndo()
        {
            this.RedoUndo.UpdateOperator += () =>
            {
                refreshPropertyToPanel();
            };

            _attrib.UpdateControl += new UpdateControl((item) =>
            {
                RedoUndo.recordOper(item);
            });
        }

        public override void createID()
        {
            _attrib.ID = (UInt16)SVUniqueID.instance().newUniqueID();
        }

        public override void delID()
        {
            SVUniqueID.instance().delUniqueID((Int16)_attrib.ID);
        }

        public override void setStartPos(Point pos)
        {
            _attrib.Rect = new Rectangle(pos.X, pos.Y, this.Width, this.Height);
        }

        public override object property()
        {
            return _attrib;
        }

        protected SVGif(SerializationInfo info, StreamingContext context)
        {
            _attrib = (SVGifProperties)info.GetValue("stream", typeof(SVGifProperties));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("stream", _attrib);
        }

        //建立一个副本
        override public object cloneObject()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, this);
            stream.Position = 0;
            SVGif result = (SVGif)binFormat.Deserialize(stream);

            result.refreshPropertyToPanel();

            return result;
        }

        public override void refreshPropertyToPanel()
        {
            this.Id = _attrib.ID;
            this.Width = _attrib.Rect.Width;
            this.Height = _attrib.Rect.Height;
            this.Location = new Point(_attrib.Rect.X, _attrib.Rect.Y);
            this.BackColor = Color.Green;
            this.IsMoved = !_attrib.Lock;

            if (_attrib.VarName.Count != 0)
                _attrib.VarList = String.Join(",", _attrib.VarName);
            else
                _attrib.VarList = "未设置变量";

            if (_attrib.Pic.BitmapArray.Count > 0)
            {
                SVBitmap svbitMap = _attrib.Pic.BitmapArray[0];

                String file = Path.Combine(SVProData.IconPath, svbitMap.ImageFileName);
                if (!File.Exists(file))
                    return;

                SVPixmapFile pixmapFile = new SVPixmapFile();
                pixmapFile.readPixmapFile(file);
                this.BackgroundImage = pixmapFile.getBitmapFromData();
            }
        }

        override public void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement gif = xml.CurrentElement;

            if (isCreate)
                createID();
            else
                _attrib.ID = UInt16.Parse(gif.GetAttribute("ID"));

            int x = int.Parse(gif.GetAttribute("X"));
            int y = int.Parse(gif.GetAttribute("Y"));
            int width = int.Parse(gif.GetAttribute("Width"));
            int height = int.Parse(gif.GetAttribute("Height"));
            _attrib.Rect = new Rectangle(x, y, width, height);

            ///读取错误图片数据
            _attrib.PicError.ImageFileName = gif.GetAttribute("ErrorFile");
            _attrib.PicError.ShowName = gif.GetAttribute("ErrorShowName");

            XmlDocument docment = gif.OwnerDocument;

            ///读取背景图片数组
            XmlNodeList nls = gif.GetElementsByTagName("PIC");
            foreach (var tmp in nls)
            {
                XmlElement vElement = (XmlElement)tmp;
                SVBitmap svBitmap = new SVBitmap();
                svBitmap.ShowName = vElement.GetAttribute("Name");
                svBitmap.ImageFileName = vElement.GetAttribute("File");
                _attrib.Pic.BitmapArray.Add(svBitmap);
            }

            ///读取变量名列表
            nls = gif.GetElementsByTagName("VarName");
            foreach (var tmp in nls)
            {
                XmlElement vElement = (XmlElement)tmp;
                String vName = vElement.GetAttribute("VName");
                Byte vType = Byte.Parse(vElement.GetAttribute("VType"));
                _attrib.VarName.Add(vName);
                _attrib.VarType.Add(vType);
            }
        }

        override public void saveXML(SVXml xml)
        {
            XmlElement gif = xml.createNode(this.GetType().Name);

            gif.SetAttribute("ID", _attrib.ID.ToString());
            gif.SetAttribute("X", _attrib.Rect.X.ToString());
            gif.SetAttribute("Y", _attrib.Rect.Y.ToString());
            gif.SetAttribute("Width", _attrib.Rect.Width.ToString());
            gif.SetAttribute("Height", _attrib.Rect.Height.ToString());

            ///出错图片保存
            gif.SetAttribute("ErrorFile", _attrib.PicError.ImageFileName);
            gif.SetAttribute("ErrorShowName", _attrib.PicError.ShowName);

            ///保存变量
            Int32 i = 0;
            foreach (var name in _attrib.VarName)
            {
                XmlElement nameList = xml.crateChildNode("VarName");
                gif.AppendChild(nameList);

                nameList.SetAttribute("VName", name);
                nameList.SetAttribute("VType", _attrib.VarType[i].ToString());

                i++;
            }

            ///保存背景图片数据
            List<SVBitmap> list = _attrib.Pic.imageArray();
            foreach (var item in list)
            {
                XmlElement picList = xml.crateChildNode("PIC");
                gif.AppendChild(picList);

                picList.SetAttribute("Name", item.ShowName);
                picList.SetAttribute("File", item.ImageFileName);
            }
        }

        /// <summary>
        /// 生成下装文件信息
        /// </summary>
        /// <param Name="pageArrayBin"></param>
        /// <param Name="serialize"></param>
        public void buildControlToBin(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            _attrib.make(ref pageArrayBin, ref serialize);
        }

        /// <summary>
        /// 检查其合法性
        /// </summary>
        public override void checkValid()
        {
            SVPageWidget pageWidget = this.Parent as SVPageWidget;
            String pageName = pageWidget.PageName;

            if (!this.Parent.ClientRectangle.Contains(this.Bounds))
            {
                String msg = String.Format("页面{0}中的动态图ID为:{1}, 已经超出页面显示范围", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (_attrib.VarName.Count == 0)
            {
                String msg = String.Format("页面 {0} 中的动态图ID为:{1}, 没有设置变量!", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }
        }
    }
}
