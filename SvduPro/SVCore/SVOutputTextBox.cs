using System;
using System.Windows.Forms;
using System.Drawing;

namespace SVCore
{
    public class SVOutputTextBox : TextBox
    {
        static SVOutputTextBox _textBox = null;

        public static SVOutputTextBox instance()
        {
            if (_textBox == null)
                _textBox = new SVOutputTextBox();

            return _textBox;
        }

        SVOutputTextBox()
        {
            this.Multiline = true;
            this.ReadOnly = true;
            this.BackColor = Color.White;
            this.Location = new Point(0, 23);
            this.Name = "_textBox";
            this.ScrollBars = ScrollBars.Both;
            this.Size = new Size(255, 340);
            this.TabIndex = 2;
            this.WordWrap = false;
        }

        //换行输出
        public void outputMessage(String msg)
        {
            this.AppendText(msg + "\n");
        }

        //隔行输出
        public void outputMessageln(String msg)
        {
            String result = String.Format("\r\n{0}\r\n",  msg);
            this.AppendText(result);
        }

        //按时间输出
        public void outputByTime(String msg)
        {
            String result = String.Format("\r\n[{0}]{1}\r\n", DateTime.Now.ToString(), msg);
            this.AppendText(result);
        }
    }
}
