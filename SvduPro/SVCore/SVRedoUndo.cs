using System;
using System.Collections.Generic;

namespace SVCore
{
    //控件属性发生变化后
    public delegate void UpdateControl(SVRedoUndoItem item);
    //记录撤销和恢复的操作
    public delegate void DoFunction();
    //当有操作被记录时候，发送该信号
    public delegate void OperatorChanged();

    public class SVRedoUndoItem
    {
        //恢复操作
        public DoFunction ReDo;
        //取消操作
        public DoFunction UnDo;
    }

    public class SVRedoUndo
    {
        private List<SVRedoUndoItem> listItem;
        private Int32 index;
        private Boolean isRecord;
        public DoFunction UpdateOperator;
        public OperatorChanged operChanged;

        public SVRedoUndo()
        {
            isRecord = true;
            index = 0;
            listItem = new List<SVRedoUndoItem>(); 
        }

        /// <summary>
        /// 表明是否记录当前操作
        /// true表示记录 false表示不记录一下操作
        /// </summary>
        /// <param name="en">设置记录使能</param>
        public void setEnabled(Boolean en)
        {
            isRecord = en;
        }

        /// <summary>
        /// 记录当前操作
        /// </summary>
        /// <param name="item">当前操作项</param>
        public void recordOper(SVRedoUndoItem item)
        {
            if (!isRecord)
                return;

            listItem.RemoveRange(index, listItem.Count - index);

            index++;
            listItem.Add(item);
            operChanged();
        }

        /// <summary>
        /// 执行恢复操作
        /// </summary>
        public void Redo()
        {
            if (index >= listItem.Count)
                return;

            index++;
            SVRedoUndoItem item = listItem[index - 1];
            item.ReDo();

            UpdateOperator();
            operChanged();
        }
        
        /// <summary>
        /// 执行撤销操作
        /// </summary>
        public void Undo()
        {
            if (index <= 0)
                return;
            
            SVRedoUndoItem item = listItem[index - 1];            
            item.UnDo();
            index--;

            UpdateOperator();
            operChanged();
        }

        /// <summary>
        /// 获取当前剩余撤销次数
        /// </summary>
        /// <returns>可以撤销的次数</returns>
        public Int32 getUndoCount()
        {
            return index;
        }

        /// <summary>
        /// 记录当前剩余执行恢复的次数
        /// </summary>
        /// <returns>剩余可以恢复的执行次数</returns>
        public Int32 getRedoCount()
        {
            return listItem.Count - index;
        }

        /// <summary>
        /// 判断当前还是否能继续执行撤销
        /// </summary>
        /// <returns>true可以，false不可以</returns>
        public Boolean UndoEnd()
        {
            return (index <= 0);
        }

        /// <summary>
        /// 判断当前操作还是否能继续执行恢复
        /// </summary>
        /// <returns>true可以，false不可以</returns>
        public Boolean RedoEnd()
        {
            return (index >= listItem.Count);
        }
    }
}
