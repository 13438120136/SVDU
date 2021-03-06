﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

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

        /// <summary>
        /// 当前软件设置的语言
        /// </summary>
        String _language = "zh";

        /// <summary>
        /// 当前软件的语言,分为中文和英文两种
        /// 
        /// 中文语言，值为:"zh"
        /// 英文语言, 值为:"en"
        /// 
        /// 默认语言为中文。
        /// </summary>
        public String Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// 加载软件配置信息
        /// </summary>
        public void loadConfig()
        {
            String file = Path.Combine(Application.StartupPath, "data.ini");
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
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
            _language = (String)binFormat.Deserialize(stream);

            br.Close();
            fs.Close();
        }

        /// <summary>
        /// 保存软件配置信息
        /// </summary>
        public void saveConfig()
        {
            String file = Path.Combine(Application.StartupPath, "data.ini");
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            BinaryWriter br = new BinaryWriter(fs);

            MemoryStream stream = new MemoryStream();
            BinaryFormatter binFormat = new BinaryFormatter();
            binFormat.Serialize(stream, _rectFileItems);
            binFormat.Serialize(stream, _rectCount);
            binFormat.Serialize(stream, _saveInterval);
            binFormat.Serialize(stream, _language);

            //些数据
            br.Write(stream.ToArray());

            br.Close();
            fs.Close();
        }
    }
}
