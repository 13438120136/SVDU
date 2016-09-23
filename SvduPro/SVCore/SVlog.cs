using System;
using System.Diagnostics;

namespace SVCore
{
    /// <summary>
    /// 记录日志
    /// </summary>
    public class TraceLog
    {
        TraceSource _traceSource = new TraceSource("Log", SourceLevels.All);

        /// <summary>
        /// 日志监听的写日志类
        /// </summary>
        /// <returns></returns>
        public TraceListenerCollection Listeners()
        {
            return _traceSource.Listeners;
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Info(String message)
        {
            _traceSource.TraceData(TraceEventType.Information, 0, message);
            _traceSource.Flush();
        }

        /// <summary>
        /// 输出警告
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Warning(String message)
        {
            _traceSource.TraceData(TraceEventType.Warning, 0, message);
            _traceSource.Flush();
        }

        /// <summary>
        /// 输出错误
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Error(String message)
        {
            _traceSource.TraceData(TraceEventType.Error, 0, message);
            _traceSource.Flush();
        }

        /// <summary>
        /// 输出程序崩溃
        /// </summary>
        /// <param name="message">日志信息</param>
        public void Critical(String message)
        {
            _traceSource.TraceData(TraceEventType.Critical, 0, message);
            _traceSource.Flush();
        }

        /// <summary>
        /// 记录异常详细信息
        /// </summary>
        /// <param name="ex">异常对象</param>
        public void Exception(Exception ex)
        {
            String msg = String.Format("异常类型:{0} {1}\r\n\t{2}", ex.GetType().ToString(), ex.Message, ex.StackTrace);
            Critical(msg);
        }
    }


    /// <summary>
    /// 日志类
    /// </summary>
    public static class SVLog
    {
        /// <summary>
        /// 记录文件中的日志
        /// </summary>
        static public TraceLog TextLog { get { return textLog; } }

        /// <summary>
        /// 当前窗口和文件同时记录
        /// </summary>
        static public TraceLog WinLog { get { return winLog; } }

        static TraceLog textLog = new TraceLog();
        static TraceLog winLog = new TraceLog();
    }
}
