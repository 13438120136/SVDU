using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SVCore
{
    public class SVConfig
    {
        //记录当前软件配置的唯一实例
        static SVConfig _instance = null;

        public static SVConfig instance()
        {
            if (_instance == null)
                _instance = new SVConfig();

            return _instance;
        }

        //记录最近打开工程文件
        List<String> _rectFileItems = new List<String>();

        public List<String> RectFileItems
        {
            get { return _rectFileItems; }
        }

        public void addRectFilesItem(String file)
        {
            if (_rectFileItems.Contains(file))
                _rectFileItems.Remove(file);

            _rectFileItems.Insert(0, file);
            if (_rectFileItems.Count > _rectCount)
                _rectFileItems.RemoveAt(_rectCount);
        }

        //打开文件最大记录的个数
        Int32 _rectCount = 5;

        public Int32 RectCount
        {
            get { return _rectCount; }
            set 
            {
                if (value < 1 || value > 10)
                    return;

                _rectCount = value; 
            }
        }

        //定时保存时间间隔,默认5分钟
        Int32 _saveInterval = 5;

        public Int32 SaveInterval
        {
            get { return _saveInterval; }
            set 
            {
                if (value < 1 || value > 60)
                    return;

                _saveInterval = value;
            }
        }

        public void loadConfig()
        {
            FileStream fs = new FileStream("data.ini", FileMode.OpenOrCreate);
            BinaryReader br = new BinaryReader(fs);

            //读数据
            Int32 length = (Int32)fs.Length;
            byte[] buffer = new byte[length];
            br.Read(buffer, 0, length);
            if (length == 0)
            {
                br.Close();
                fs.Close();
                return;
            }

            MemoryStream stream = new MemoryStream(buffer);
            BinaryFormatter binFormat = new BinaryFormatter();
            _rectFileItems = binFormat.Deserialize(stream) as List<String>;
            _rectCount = (Int32)binFormat.Deserialize(stream);
            _saveInterval = (Int32)binFormat.Deserialize(stream);

            br.Close();
            fs.Close();
        }

        public void saveConfig()
        {
            FileStream fs = new FileStream("data.ini", FileMode.OpenOrCreate);
            BinaryWriter br = new BinaryWriter(fs);

            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, _rectFileItems);
            binFormat.Serialize(stream, _rectCount);
            binFormat.Serialize(stream, _saveInterval);

            //些数据
            br.Write(stream.ToArray());

            br.Close();
            fs.Close();
        }
    }
}
