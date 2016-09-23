using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SVCore
{
    public class SVTextBoxTraceListenter : TraceListener
    {
        //当前TextBox对象
        TextBox _textBox;

        /// <summary>
        /// 将日志信息输出到TextBox
        /// </summary>
        /// <param name="textBox">记录日志的TextBox控件窗口</param>
        public SVTextBoxTraceListenter(TextBox textBox)
        {
            _textBox = textBox;
        }

        /// <summary>
        /// 修改默认的日志头信息
        /// 在日志的开头部分加入时间，并将提示信息显示为中文输出
        /// </summary>
        /// <param name="message">日志头信息</param>
        public override void Write(string message)
        {
            if (_textBox == null)
                return;

            String str = String.Format("[{0}] [{1}]: ", dateTimeLongString(), fromMessage(message));
            _textBox.AppendText(str);
        }

        /// <summary>
        /// 输出日志内容
        /// </summary>
        /// <param name="message">日志内容信息</param>
        public override void WriteLine(string message)
        {
            if (_textBox == null)
                return;

            _textBox.AppendText(message + "\n");
        }

        /// <summary>
        /// 获取当前机器的日期和时间
        /// </summary>
        /// <returns>以字符串的方式返回</returns>
        String dateTimeLongString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
    }
}
