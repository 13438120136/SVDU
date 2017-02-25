using System;

//自定义的异常类，用来判断和检查属性值的合法性
namespace SVCore
{
    public class SVCheckValidException : ApplicationException
    {
        /// <summary>
        /// 自定义的异常处理类
        /// 
        /// 记录发生异常时刻，对页面及控件检查不通过的项
        /// </summary>
        /// <param oldName="message">异常信息</param>
        public SVCheckValidException(string message)
            : base(message) 
        {
        }  
  
        /// <summary>
        /// 重载获取当前异常提示信息
        /// </summary>
        public override string Message  
        {  
            get  
            {  
                return base.Message;  
            }
        } 
    }
}
