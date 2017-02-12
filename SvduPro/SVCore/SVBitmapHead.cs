using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SVCore
{
    public class SVBitmapHead
    {
        //UInt32 _id;
        //UInt32 _rsv;
        //UInt32 _palOffset;
        //UInt32 _palCnt;
        //UInt32 _dataOffset;
        //UInt32 _dataCnt;
        byte[] _data;

        public SVBitmapHead()
        {
            //_palOffset = 24;
        }

        /// <summary>
        /// 设置当前的图片对象
        /// </summary>
        /// <param name="image">图片对象</param>
        public SVBitmapHead(Image image)
        {
            //_palOffset = 24;
            setImageObj(image);
        }

        //public void setID(UInt32 id)
        //{
        //    _id = id;
        //}

        /// <summary>
        /// 设置当前的图片对象
        /// </summary>
        /// <param name="image"></param>
        public void setImageObj(Image image)
        {
            //_palCnt = (UInt32)image.Palette.Entries.Length;
            //_dataOffset = _palCnt * 4 + _palOffset;

            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Bmp);
            byte[] byteArrayImage = stream.ToArray();

            //byte[] tmp = new byte[byteArrayImage.Length - 54];
            //Array.Copy(byteArrayImage, 54, tmp, 0, tmp.Length);
            //_dataCnt = (UInt32)(tmp.Length - _palCnt * 4);

            _data = byteArrayImage;
        }

        /// <summary>
        /// 获取图片的内存数据
        /// </summary>
        /// <returns></returns>
        public byte[] data()
        {
            return _data;
            //SVSerialize svSerialize = new SVSerialize();
            //svSerialize.pack(_id);
            //svSerialize.pack(_rsv);
            //svSerialize.pack(_palOffset);
            //svSerialize.pack(_palCnt);
            //svSerialize.pack(_dataOffset);
            //svSerialize.pack(_dataCnt);
            //svSerialize.Write(_data, 0, _data.Length);

            //return svSerialize.ToArray();
        }
    }
}
