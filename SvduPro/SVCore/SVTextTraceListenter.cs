using System;
using System.Diagnostics;
using System.IO;

namespace SVCore
{
    public class SVTextTraceListenter : TraceListener
    {
        String _dateString;   //当前日期字符串
        String _path;         //设置当前日志文件记录的目录
        FileStream _fileStream;
        StreamWriter _streamWriter;

        /// <summary>
        /// 初始化日志路径
        /// 如果为空，使用当前路径做为日志存放路径
        /// </summary>
        /// <param name="path">日志路径</param>
        public SVTextTraceListenter(String path = null)
        {
            if ((path == null) || (path == String.Empty))
                _path = System.Environment.CurrentDirectory;
            else
                _path = path;

            initFile(dateTimeToString());
        }

        /// <summary>
        /// 修改默认的日志头信息
        /// 在日志的开头部分加入时间，并将提示信息显示为中文输出
        /// </summary>
        /// <param name="message">日志头信息</param>
        public override void Write(string message)
        {
            if (_streamWriter == null)
                return;

            String vStr = dateTimeToString();
            if (_dateString != vStr)
            {
                _fileStream.Close();
                initFile(vStr);
            }

            String str = String.Format("[{0}] [{1}]: ", dateTimeLongString(), fromMessage(message));
            _streamWriter.Write(str);
        }

        /// <summary>
        /// 输出日志内容
        /// </summary>
        /// <param name="message">日志内容信息</param>
        public override void WriteLine(string message)
        {
            if (_streamWriter == null)
                return;

            _streamWriter.WriteLine(message);
        }

        /// <summary>
        /// 重载Flush函数,将缓冲中的内容写入到文件中
        /// </summary>
        public override void Flush()
        {
            if (_streamWriter == null)
                return;

            _streamWriter.Flush();
        }

        /// <summary>
        /// 重载Dispose
        /// 关闭连接
        /// </summary>
        /// <param name="disposing">没有用到</param>
        protected override void Dispose(bool disposing)
        {
            if (_streamWriter == null)
                return;

            if (_fileStream != null)
            {
                _fileStream.Close();
                _fileStream = null;
            }
        }

        /// <summary>
        /// 修改默认的日志头字符串信息，并返回
        /// </summary>
        /// <param name="str">日志头信息字符串</param>
        /// <returns>要输出的字符串</returns>
        String fromMessage(String str)
        {
            if (str.Contains("Information"))
                return "信息";

            if (str.Contains("Warning"))
                return "警告";

            if (str.Contains("Error"))
                return "错误";

            if (str.Contains("Critical"))
                return "异常";

            return null;
        }

        /// <summary>
        /// 获取当前机器的日期
        /// </summary>
        /// <returns>以字符串的方式返回</returns>
        String dateTimeToString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取当前机器的日期和时间
        /// </summary>
        /// <returns>以字符串的方式返回</returns>
        String dateTimeLongString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff");
        }

        /// <summary>
        /// 根据输入字符串来确定日志的名称
        /// 初始化、打开日志文件句柄
        /// </summary>
        /// <param name="str">日志文件名称</param>
        void initFile(String str)
        {
            _dateString = str;

            String file = String.Format("{0}.txt", _dateString);
            String fileName = Path.Combine(_path, file);

            _fileStream = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            _streamWriter = new StreamWriter(_fileStream);
        }
    }
}
