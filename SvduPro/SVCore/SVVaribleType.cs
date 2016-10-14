using System;
using System.Collections.Generic;
using System.Data;

namespace SVCore
{
    /// <summary>
    /// 变量4字节
    /// 
    /// 最高位第一个字节: 0 - 接收，1 - 发送
    /// 其余三个字节表示具体偏移
    /// </summary>

    public enum VarType
    {
    	VARTYPE_CHAR,      //1b
	    VARTYPE_CHARQ,     //2b
	    VARTYPE_INT16,     //2b
	    VARTYPE_INT16Q,    //3b
	    VARTYPE_INT32,     //4b
	    VARTYPE_INT32Q,    //5b
	    VARTYPE_FLOAT,     //4b
	    VARTYPE_FLOATQ,    //5b
    }

    public class SVVaribleType
    {
        Dictionary<String, VarType> _dict = null;
        static SVVaribleType _instance = null;
        DataTable _dataTable = null;

        /// <summary>
        /// 全局单例模式
        /// </summary>
        /// <returns></returns>
        public static SVVaribleType instance()
        {
            if (_instance == null)
                _instance = new SVVaribleType();

            return _instance;
        }

        /// <summary>
        /// 根据变量名称来获取当前变量的地址
        /// </summary>
        /// <param name="varName">变量名称</param>
        /// <returns>返回变量地址</returns>
        public UInt32 strToAddress(String varName)
        {
            String selectStr = string.Format("ioblockname='{0}'", varName);
            DataRow[] dr = _dataTable.Select(selectStr);
            if (dr.Length == 0)
                return 0;

            ///首先以字符串的形式得到变量的地址
            String address = dr[0]["addressoffset"].ToString();
            ///从字符串转换为数字
            UInt32 iAddress = UInt32.Parse(address);

            byte[] buffer = BitConverter.GetBytes(iAddress);
            UInt32 result = BitConverter.ToUInt32(buffer, 0);

            return result;
        }

        /// <summary>
        /// 根据当前变量名称来获取变量类型
        /// </summary>
        /// <param name="dataTable"></param>
        public SByte strToType(String varName)
        {
            String selectStr = string.Format("ioblockname='{0}'", varName);
            DataRow[] dr = _dataTable.Select(selectStr);
            if (dr.Length == 0)
                return -1;

            String name = dr[0]["valueType"].ToString();
            if (_dict.ContainsKey(name))
                return (SByte)_dict[name];

            return -1;
        }

        /// <summary>
        /// 判断当前数据库是否打开，并正确读取数据
        /// </summary>
        /// <returns>True: 正确打开
        /// Fasle: 失败</returns>
        public Boolean isOpen()
        {
            return !(_dataTable == null);
        }

        public void setData(DataTable dataTable)
        {
            _dataTable = dataTable;
        }

        public DataTable getData()
        {
            return _dataTable;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SVVaribleType()
        {
            _dict = new Dictionary<string, VarType>() 
            {
            { "BOOL", VarType.VARTYPE_CHAR }, 
            { "BOOL_VAR", VarType.VARTYPE_CHARQ }, 
            { "SHORT_INT", VarType.VARTYPE_INT16 }, 
            { "SHORTINT_VAR", VarType.VARTYPE_INT16Q }, 
            { "INT", VarType.VARTYPE_INT32 }, 
            { "INT_VAR", VarType.VARTYPE_INT32Q }, 
            { "REAL", VarType.VARTYPE_FLOAT }, 
            { "REAL_VAR", VarType.VARTYPE_FLOATQ }};

            if (_dataTable == null)
            {
                SVInterfaceApplication app = SVApplication.Instance;
                SVSqlDataBase sqlDataBase = app.DataBase;
                _dataTable = sqlDataBase.getVarDataList(11);
            }
        }
    }
}
