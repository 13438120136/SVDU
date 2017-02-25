using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVCore
{
    /// <summary>
    /// 版本类，用来记录和保存配置文件中的版本号
    /// </summary>
    class SVVersion
    {
        public UInt16 Year;         //年
        public Byte Month;          //月
        public Byte Day;            //日
        public Byte Hour;           //小时
        public Byte Minute;         //分钟
        public Byte Second;         //秒
        public Byte rsv;            //预留，保证4字节对齐

        /// <summary>
        /// 序列化当前版本数据
        /// </summary>
        /// <param oldName="serialize">序列化对象</param>
        public void pack(SVSerialize serialize)
        {
            serialize.pack(Year);
            serialize.pack(Month);
            serialize.pack(Day);
            serialize.pack(Hour);
            serialize.pack(Minute);
            serialize.pack(Second);
            serialize.pack(rsv);
        }

        /// <summary>
        /// 反序列化当前版本数据
        /// </summary>
        /// <param oldName="serialize">序列化对象</param>
        public void unpack(SVSerialize serialize)
        {
            serialize.unpack(ref Year);
            serialize.unpack(ref Month);
            serialize.unpack(ref Day);
            serialize.unpack(ref Hour);
            serialize.unpack(ref Minute);
            serialize.unpack(ref Second);
            serialize.unpack(ref rsv);
        }
    }
}
