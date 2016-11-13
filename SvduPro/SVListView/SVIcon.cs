using System;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SVCore;
using System.IO;

namespace SVControl
{
    [Serializable]
    public class SVIcon : SVPanel, SVInterfacePanel, ISerializable, SVInterfaceBuild
    {
        SVIconProperties _attrib = new SVIconProperties();

        public SVIconProperties Attrib
        {
            set
            {
                _attrib = value;
            }
            get { return _attrib; }
        }

        public SVIcon()
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

        public override void newID()
        {
            _attrib.ID = base.createID();
        }

        public override void setStartPos(Point pos)
        {
            _attrib.Rect = new Rectangle(pos.X, pos.Y, this.Width, this.Height);
        }

        public override object property()
        {
            return _attrib;
        }

        protected SVIcon(SerializationInfo info, StreamingContext context)
        {
            _attrib = (SVIconProperties)info.GetValue("stream", typeof(SVIconProperties));
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
            SVIcon result = (SVIcon)binFormat.Deserialize(stream);

            result.refreshPropertyToPanel();

            return result;
        }

        public override void refreshPropertyToPanel()
        {
            this.Id = _attrib.ID;
            this.Width = _attrib.Rect.Width;
            this.Height = _attrib.Rect.Height;
            this.Location = new Point(_attrib.Rect.X, _attrib.Rect.Y);
            this.BackColor = Color.Red;
            this.IsMoved = !_attrib.Lock;

            ///根据静态图是否设置了图片来决定外观显示
            this.BackgroundImage = Attrib.PicIconData.bitmap();
        }

        override public void loadXML(SVXml xml, Boolean isCreate = false)
        {
            XmlElement icon = xml.CurrentElement;

            if (isCreate)
                newID();
            else
                _attrib.ID = UInt16.Parse(icon.GetAttribute("ID"));

            int x = int.Parse(icon.GetAttribute("X"));
            int y = int.Parse(icon.GetAttribute("Y"));
            int width = int.Parse(icon.GetAttribute("Width"));
            int height = int.Parse(icon.GetAttribute("Height"));
            _attrib.Rect = new Rectangle(x, y, width, height);
            _attrib.PicIconData.ImageFileName = icon.GetAttribute("ImageFile");
            _attrib.PicIconData.ShowName = icon.GetAttribute("ImageShowName");
        }

        override public void saveXML(SVXml xml)
        {
            XmlElement icon = xml.createNode(this.GetType().Name);

            icon.SetAttribute("ID", _attrib.ID.ToString());
            icon.SetAttribute("X", _attrib.Rect.X.ToString());
            icon.SetAttribute("Y", _attrib.Rect.Y.ToString());
            icon.SetAttribute("Width", _attrib.Rect.Width.ToString());
            icon.SetAttribute("Height", _attrib.Rect.Height.ToString());
            icon.SetAttribute("ImageFile", _attrib.PicIconData.ImageFileName);
            icon.SetAttribute("ImageShowName", _attrib.PicIconData.ShowName);
        }

        public void buildControlToBin(ref PageArrayBin pageArrayBin, ref SVSerialize serialize)
        {
            _attrib.make(ref pageArrayBin, ref serialize);
        }

        /// <summary>
        /// 检查静态图中的合法项
        /// </summary>
        public override void checkValid() 
        {
            SVPageWidget pageWidget = this.Parent as SVPageWidget;
            String pageName = pageWidget.PageName;

            SVUniqueID uniqueObj = SVUniqueID.instance();

            if (Attrib.ID <= 0 || Attrib.ID >= uniqueObj.MaxID)
            {
                String msg = String.Format("页面 {0} 中,静态图ID为:{1}, ID值已经超出合法范围[{2} - {3}]", pageName, Attrib.ID, 1, uniqueObj.MaxID);
                throw new SVCheckValidException(msg);
            }

            if (Attrib.PicIconData.bitmap() == null)
            {
                String msg = String.Format("页面 {0} 中,静态图ID为:{1}, 图片属性没有设置!", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }

            if (!this.Parent.ClientRectangle.Contains(this.Bounds))
            {
                String msg = String.Format("页面 {0} 中,静态图ID为:{1}, 已经超出页面显示范围", pageName, Attrib.ID);
                throw new SVCheckValidException(msg);
            }
        }
    }
}
