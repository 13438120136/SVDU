//
// 创建唯一的ID号，此ID号用来区分唯一的页面和控件元素等。所有ID都唯一不能重复
// ID号的范围为[1-1500]
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SVCore
{
    [Serializable]
    public class SVUniqueID
    {
        UInt16 _maxID = 1500;
        Stack<UInt16> _data = null;
        String _UniqueFile = null;

        //定义全局唯一配置对象
        static SVUniqueID _obj = null;

        public static SVUniqueID instance()
        {
            if (_obj == null)
                _obj = new SVUniqueID();

            return _obj;
        }

        public String UniqueFile
        {
            get { return _UniqueFile; }
            set { _UniqueFile = value; }
        }

        //获取最大ID号的值
        public UInt16 MaxID
        {
            get { return _maxID; }
        }

        public void initUniqueID()
        {
            List<UInt16> tmpList = new List<UInt16>();
            for (UInt16 i = 1; i <= _maxID; i++)
                tmpList.Add(i);

            tmpList.Sort((a, b) => { return b - a; });
            _data = new Stack<UInt16>(tmpList);
        }

        // 获取一个新的唯一ID号。
        // 如果不成功将返回 0
        public UInt16 newUniqueID()
        {
            if (_data.Count() == 0)
                return 0;

            return _data.Pop();
        }

        //回收不用的ID号，供后面使用。
        public void delUniqueID(UInt16 id)
        {
            //ID号不在合法范围内也不会进行回收
            if (id < 1 || id > _maxID)
                return;

            _data.Push(id);

            ///回收后进行排序
            List<UInt16> tmpList = new List<UInt16>(_data);
            tmpList.Sort((a, b) => { return b - a; });
            _data = new Stack<UInt16>(tmpList);
        }

        //将当前类序列化
        public byte[] getBuffer()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, _data);

            return stream.ToArray();
        }

        //反序列化为当前对象
        public void setBuffer(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BinaryFormatter binFormat = new BinaryFormatter();
            _data = binFormat.Deserialize(stream) as Stack<UInt16>;
        }

        public void saveFile()
        {
            if (UniqueFile == null)
                return;

            FileStream fs = new FileStream(UniqueFile, FileMode.OpenOrCreate);
            BinaryWriter br = new BinaryWriter(fs);
            br.Write(getBuffer());

            br.Close();
            fs.Close();
        }

        public void loadFile()
        {
            FileStream fs = new FileStream(UniqueFile, FileMode.OpenOrCreate);
            BinaryReader br = new BinaryReader(fs);

            Int32 length = (Int32)fs.Length;
            byte[] buffer = new byte[length];
            br.Read(buffer, 0, length);
            setBuffer(buffer);

            br.Close();
            fs.Close();
        }
    }
}
