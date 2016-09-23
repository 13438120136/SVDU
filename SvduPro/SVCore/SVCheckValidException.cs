using System;

//自定义的异常类，用来判断和检查属性值的合法性
namespace SVCore
{
    public class SVCheckValidException : ApplicationException
    {
        public SVCheckValidException(string message)
            : base(message) 
        {
        }  
  
        public override string Message  
        {  
            get  
            {  
                return base.Message;  
            }
        } 
    }
}
