using System;
using System.IO;
using System.Text;

namespace SVCore
{
    public class SVBuildFile
    {
        /// <summary>
        /// 文件头部数据定义
        /// </summary>
        Byte[] _name = new Byte[8];     //等价于后面的UInt32[] _name = new UInt32[2]
        //SVVersion _version;             //版本号
        Byte[] _version = new Byte[8];
        UInt32 _type;                   //类型
        UInt32 _fileSize;               //当前文件长度，包括文件头
        UInt32 _crc;                    //实际数据的CRC值
        UInt32 _imagePos;               //图片起始偏移位置
        UInt32 _iamgeSize;              //图片大小
        UInt32[] _rsv = new UInt32[7];

        /// <summary>
        /// 文件内容部分数据定义
        /// </summary>
        byte[] _data;                   //实际数据

        /// <summary>
        /// 当前生成下装文件的文件名称
        /// </summary>
        String _fileName;

        /// <summary>
        /// 生成下装配置文件的构造函数
        /// </summary>
        public SVBuildFile()
        {
            init();
        }

        /// <summary>
        /// 生成下装配置文件的构造函数
        /// </summary>
        /// <param name="fileName">当前要生成的下装文件名</param>
        public SVBuildFile(String fileName)
        {
            init();
            setFileName(fileName);
        }

        /// <summary>
        /// 设置下装文件路径
        /// </summary>
        /// <param name="fileName">当前要生成的下装文件名</param>
        public void setFileName(String fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// 初始化当前对象头部数据
        /// </summary>
        void init()
        {
            byte[] byteArray = Encoding.ASCII.GetBytes("PFCFG");
            Array.Copy(byteArray, _name, byteArray.Length);

            //_version = new SVVersion();
            ///写入文件版本号
            byte[] tmpVersion = Encoding.ASCII.GetBytes("1.1.0");
            Array.Copy(tmpVersion, _version, tmpVersion.Length);

            _type = 0x5555AAAA;
            _imagePos = 0;
            _iamgeSize = 0;
        }

        /// <summary>
        /// 设置当前文件要保存的数据内容
        /// </summary>
        /// <param name="data">实际数据内容</param>
        public void setDataByteArray(byte[] data)
        {
            _data = data;
            _fileSize = (UInt32)data.Length;

            //计算CRC值
            SVCrc32 crc = new SVCrc32();
            _crc = crc.calculateCrc32(data, _fileSize);

            ///文件总长度包含头部长度，所以这里需要加上64个字节
            _fileSize += 64;
        }

        /// <summary>
        /// 得到当前对象中的文件数据
        /// </summary>
        /// <returns>文件内容数据</returns>
        public byte[] getDataByteArray()
        {
            return _data;
        }

        /// <summary>
        /// 执行实际的写数据
        /// </summary>
        /// <returns>
        /// 成功 - true
        /// 失败 - false
        /// </returns>
        public Boolean write()
        {
            //DateTime now = DateTime.Now;
            //_version.Year = (UInt16)now.Year;
            //_version.Month = (Byte)now.Month;
            //_version.Day = (Byte)now.Day;
            //_version.Hour = (Byte)now.Hour;
            //_version.Minute = (Byte)now.Minute;
            //_version.Second = (Byte)now.Second;

            SVSerialize svSerialize = new SVSerialize();
            svSerialize.pack(_name);
            //_version.pack(svSerialize);
            svSerialize.pack(_version);
            svSerialize.pack(_type);
            svSerialize.pack(_fileSize);
            svSerialize.pack(_crc);
            svSerialize.pack(_imagePos);
            svSerialize.pack(_iamgeSize);
            foreach (var item in _rsv)
                svSerialize.pack(item);
            svSerialize.pack(_data);

            FileStream fileStream = new FileStream(_fileName, FileMode.Create);
            try
            {
                byte[] allData = svSerialize.ToArray();
                fileStream.Write(allData, 0, allData.Length);
            }
            catch (IOException ex)
            {
                return false;
            }
            finally
            {
                fileStream.Close();
            }

            return true;
        }

        /// <summary>
        /// 从当前指定的文件中文件内容
        /// </summary>
        /// <returns>成功-true  失败-false</returns>
        public Boolean read()
        {
            int length = 0;
            byte[] allData = null;

            FileStream fileStream = new FileStream(_fileName, FileMode.Open);
            try
            {                
                ///length = (Int32)(fileStream.Length - 32);
                length = (Int32)(fileStream.Length);
                allData = new byte[fileStream.Length];
                fileStream.Read(allData, 0, (Int32)fileStream.Length);
            }
            catch (IOException ex)
            {
                return false;
            }
            finally
            {
                fileStream.Close();
            }

            if (allData == null)
                return false;

            SVSerialize svSerialize = new SVSerialize(allData);
            svSerialize.unpack(ref _name);
            //_version.unpack(svSerialize);
            svSerialize.unpack(ref _version);
            svSerialize.unpack(ref _type);
            svSerialize.unpack(ref _fileSize);
            svSerialize.unpack(ref _crc);
            svSerialize.unpack(ref _imagePos);
            svSerialize.unpack(ref _iamgeSize);

            for (int i = 0; i < 7; i++)
                svSerialize.unpack(ref _rsv[i]);

            ///需要减去当前头部长度
            _data = new byte[length - 64];
            svSerialize.unpack(ref _data);

            return true;
        }

        /// <summary>
        /// 设置当前图片数据的起始位置以及数据长度
        /// </summary>
        /// <param name="pos">起始位置</param>
        /// <param name="Length">图片长度</param>
        public void setImageOffsetAndLen(UInt32 pos, UInt32 Length)
        {
            ///偏移头部
            _imagePos = pos + 64;
            _iamgeSize = Length;
        }

        /// <summary>
        /// 得到图片数据起始位置
        /// </summary>
        /// <returns>当前图片数据位置</returns>
        public UInt32 getIamgePos()
        {
            return _imagePos;
        }

        /// <summary>
        /// 获取当前文件中图片数据内容长度
        /// </summary>
        /// <returns>图片数据内容长度</returns>
        public UInt32 getImageLength()
        {
            return _iamgeSize;
        }
    }
}
