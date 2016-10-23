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

        public DataTable sysDataTable()
        {
            DataTable systemDataTable = new DataTable();

            ///添加表头
            systemDataTable.Columns.Add("变量名", typeof(String));
            systemDataTable.Columns.Add("变量地址", typeof(UInt32));
            systemDataTable.Columns.Add("变量类型", typeof(String));

            ///表格中的内容
            List<SVVarNode> vList = new List<SVVarNode>() 
            {
                new SVVarNode("当前运行版本0", 0, "INT"),
                new SVVarNode("当前运行软件1", 1, "INT"),
                new SVVarNode("配置文件版本0", 2, "INT"),
                new SVVarNode("配置文件版本1", 3, "INT"),
                new SVVarNode("接收任务时间", 4, "INT"),
                new SVVarNode("接收任务最大时间", 5, "INT"),
                new SVVarNode("显示任务时间", 6, "INT"),
                new SVVarNode("显示任务最大时间", 7, "INT"),
                new SVVarNode("发送任务时间", 8, "INT"),
                new SVVarNode("发送任务最大时间", 9, "INT"),
                new SVVarNode("杂项任务时间", 10, "INT"),
                new SVVarNode("杂项任务最大时间", 11, "INT"),

                new SVVarNode("当前空闲时间", 12, "INT"),
                new SVVarNode("当前周期时间", 13, "INT"),
                new SVVarNode("周期最大设置时间", 14, "INT"),

                new SVVarNode("当前周期个数", 15, "INT"),

                new SVVarNode("平台ROM状态", 16, "INT"),
                new SVVarNode("平台RAM状态", 17, "INT"),
                new SVVarNode("cpu诊断状态", 18, "INT"),
                new SVVarNode("时钟诊断状态", 19, "INT"),

                new SVVarNode("内存使用率", 20, "INT"),
                new SVVarNode("cpu使用率", 21, "INT"),

                new SVVarNode("数据接收状态", 22, "INT"),
                new SVVarNode("数据发送状态", 23, "INT"),
                new SVVarNode("人机交互状态", 24, "INT"),

                new SVVarNode("当前运行模式", 25, "INT"),

                new SVVarNode("接收任务超时", 26, "INT"),
                new SVVarNode("显示任务超时", 27, "INT"),
                new SVVarNode("发送任务超时", 28, "INT"),
                new SVVarNode("杂项任务超时", 29, "INT"),
                new SVVarNode("周期超时", 30, "INT")
            };

            foreach (var item in vList)
            {
                DataRow dr = systemDataTable.NewRow();
                dr["变量名"] = item.Name;
                dr["变量地址"] = item.Address;
                dr["变量类型"] = item.Type;
                systemDataTable.Rows.Add(dr);
            }

            return systemDataTable;
        }

        /// <summary>
        /// 设置变量类型
        /// </summary>
        /// <param name="type"></param>
        public void setDataType(Int32 type)
        {
            if (type == 2)
            {
                _dataTable = sysDataTable();
                return;
            }

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

    public class SVVarNode
    {
        public String Name;
        public UInt32 Address;
        public String Type;

        public SVVarNode(String name, UInt32 address, String type)
        {
            this.Name = name;
            this.Address = address;
            this.Type = type;
        }
    }
}
