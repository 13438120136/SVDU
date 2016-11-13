using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SVCore
{
    public class SVFindTextBox : RichTextBox
    {
        Dictionary<Int32, Object> _selectDict = new Dictionary<Int32, Object>();

        public SVFindTextBox()
        {
            //去掉自动换行
            WordWrap = false;
        }

        public new void Clear() 
        {
            _selectDict.Clear();
            base.Clear();
        }

        int getCurrentLine()
        {
            int index = GetFirstCharIndexOfCurrentLine();
            int lineNum = GetLineFromCharIndex(index);

            return lineNum;
        }
        /*
         * 在当前位置设置标志，用来记录当前触发事件的执行内容
         * 
         * obj - 页面对象
         * */
        public void setMark(Object obj)
        {
            int lineNum = getCurrentLine();
            if (_selectDict.ContainsKey(lineNum))
                _selectDict.Remove(lineNum);

            _selectDict.Add(lineNum, obj);
        }

        /*
         * 获取当前标志处的对象
         * 
         * 如果不存在该标志，就返回null
         * */
        public Object getMarkObject()
        {
            int lineNum = getCurrentLine();
            if (_selectDict.ContainsKey(lineNum))
                return _selectDict[lineNum];

            return null;
        }

        /*
         * 判断当前窗口中是否查找到相关内容
         * */
        public Boolean isMatches()
        {
            if (_selectDict.Count == 0)
                return false;

            return true;
        }
    }
}
