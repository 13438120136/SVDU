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
        /// <param name="type">表示变量类型</param>
        /// <returns>返回变量地址</returns>
        public UInt32 strToAddress(String varName, Byte type)
        {
            String selectStr = string.Format("变量名='{0}'", varName);
            DataRow[] dr = _dataTable.Select(selectStr);
            if (dr.Length == 0)
                return 0;

            ///首先以字符串的形式得到变量的地址
            String address = dr[0]["变量地址"].ToString();
            ///从字符串转换为数字
            UInt32 iAddress = UInt32.Parse(address);

            byte[] buffer = BitConverter.GetBytes(iAddress);
            buffer[3] = type;
            UInt32 result = BitConverter.ToUInt32(buffer, 0);

            return result;
        }

        /// <summary>
        /// 根据当前变量名称来获取变量类型
        /// </summary>
        /// <param name="dataTable"></param>
        public SByte strToType(String varName)
        {
            String selectStr = string.Format("变量名='{0}'", varName);
            DataRow[] dr = _dataTable.Select(selectStr);
            if (dr.Length == 0)
                return 0;

            String name = dr[0]["变量类型"].ToString();
            if (_dict.ContainsKey(name))
                return (SByte)_dict[name];

            return 0;
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

        /// <summary>
        /// 设置变量表数据
        /// </summary>
        /// <param name="dataTable"></param>
        public void setData(DataTable dataTable)
        {
            _dataTable = dataTable;
        }

        /// <summary>
        /// 设置变量类型
        /// </summary>
        /// <param name="type"></param>
        public void setDataType(Int32 type)
        {
            if (type == 0)
                _dataTable.Columns.RemoveAt(2);
            else
                _dataTable.Columns.RemoveAt(1);

            ///表头进行重命名
            _dataTable.Columns["ioblockname"].ColumnName = "变量名";

            if (_dataTable.Columns.Contains("BusAddress_RE"))
                _dataTable.Columns["BusAddress_RE"].ColumnName = "变量地址";

            if (_dataTable.Columns.Contains("BusAddress_SEND"))
                _dataTable.Columns["BusAddress_SEND"].ColumnName = "变量地址";

            _dataTable.Columns["valueType"].ColumnName = "变量类型";
        }

        /// <summary>
        /// 从数据库中加载所有变量数据
        /// </summary>
        /// <returns></returns>
        public DataTable loadVariableData()
        {
            SVInterfaceApplication app = SVApplication.Instance;
            SVSqlDataBase sqlDataBase = app.DataBase;
            _dataTable = sqlDataBase.getVarDataList(SVProData.stationID);

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
        }
    }
}
