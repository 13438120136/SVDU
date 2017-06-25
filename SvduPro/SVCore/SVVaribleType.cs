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

        private DataTable _systemDataTable = new DataTable(); //系统变量表格
        private DataTable _recvTable = new DataTable(); //接收地址表格
        private DataTable _sendTable = new DataTable(); //发送地址表格

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
        /// <param oldName="varName">变量名称</param>
        /// <param oldName="type">表示变量类型</param>
        /// <returns>返回变量地址</returns>
        public UInt32 strToAddress(String varName, Byte type)
        {
            if (String.IsNullOrWhiteSpace(varName))
                return UInt32.MaxValue;

            DataTable dataTable = null;

            switch (type)
            {
                case 0:
                    {
                        dataTable = loadRecvDataTable();
                        break;
                    }
                case 1:
                    {
                        dataTable = loadSendDataTable();
                        break;
                    }
                case 2:
                    {
                        dataTable = loadSystemDataTable();
                        break;
                    }
                case 3:
                    {
                        UInt32 uname = UInt32.Parse(varName);
                        byte[] tmpBuffer = BitConverter.GetBytes(uname);
                        tmpBuffer[3] = type;
                        return BitConverter.ToUInt32(tmpBuffer, 0);
                    }
            }

            String selectStr = string.Format("ioblockname='{0}'", varName);
            DataRow[] dr = dataTable.Select(selectStr);
            if (dr.Length == 0)
                return 0;

            ///首先以字符串的形式得到变量的地址
            String address = dr[0][1].ToString();
            ///从字符串转换为数字
            UInt32 iAddress = UInt32.Parse(address);

            ///减去180的十六进制
            if (type == 0 || type == 1)
                iAddress -= 384;

            byte[] buffer = BitConverter.GetBytes(iAddress);
            buffer[3] = type;
            UInt32 result = BitConverter.ToUInt32(buffer, 0);

            return result;
        }

        /// <summary>
        /// 根据当前变量名称来获取变量类型
        /// </summary>
        /// <param oldName="dataTable"></param>
        public SByte strToType(String varName, Byte type)
        {
            String strType = strToTypeString(varName, type);
            if (String.IsNullOrWhiteSpace(strType))
                return 0;

            if (_dict.ContainsKey(strType))
                return (SByte)_dict[strType];

            return 0;
        }

        /// <summary>
        /// 根据变量名称来获取对应的字符串表示的变量类型
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public String strToTypeString(String varName, Byte type)
        {
            if (String.IsNullOrWhiteSpace(varName))
                return null;

            DataTable dataTable = null;

            switch (type)
            {
                case 0:
                    {
                        dataTable = loadRecvDataTable();
                        break;
                    }
                case 1:
                    {
                        dataTable = loadSendDataTable();
                        break;
                    }
                case 2:
                    {
                        dataTable = loadSystemDataTable();
                        break;
                    }
                case 3:
                    return null;
            }

            String selectStr = string.Format("ioblockname='{0}'", varName);
            DataRow[] dr = dataTable.Select(selectStr);
            if (dr.Length == 0)
                return null;

            String name = dr[0]["valueType"].ToString();
            return name;
        }

        /// <summary>
        /// 根据字符串类型来获取所有相关变量名称
        /// </summary>
        /// <param name="strType"></param>
        public List<String> varFromStringType(String strType)
        {
            if (String.IsNullOrWhiteSpace(strType))
                return null;

            DataTable recvTable = loadRecvDataTable();
            DataTable sendTable = loadSendDataTable();
            DataTable sysTable = loadSystemDataTable();

            DataTable result = new DataTable();
            if (recvTable != null)
                result.Merge(recvTable);

            if (sendTable != null)
                result.Merge(sendTable);

            if (sysTable != null)
                result.Merge(sysTable);

            String selectStr = string.Format("valueType='{0}'", strType);
            DataRow[] dr = result.Select(selectStr);
            if (dr.Length == 0)
                return null;

            List<String> varReulstList = new List<String>();
            for (int i = 0; i < dr.Length; i++)
                varReulstList.Add(dr[i][0].ToString());

            return varReulstList;
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
        /// <param oldName="dataTable"></param>
        public void setData(DataTable dataTable)
        {
            _dataTable = dataTable;
        }

        public DataTable loadSystemDataTable()
        {
            DataTable systemDataTable = new DataTable();

            ///添加表头
            systemDataTable.Columns.Add("ioblockname", typeof(String));
            systemDataTable.Columns.Add("varAddress", typeof(UInt32));
            systemDataTable.Columns.Add("valueType", typeof(String));

            ///表格中的内容
            List<SVVarNode> vList = new List<SVVarNode>() 
            {
                new SVVarNode("接收任务时间",         0, "INT"),
                new SVVarNode("接收任务最大时间",     1, "INT"),
                new SVVarNode("显示任务时间",         2, "INT"),
                new SVVarNode("显示任务最大时间", 3, "INT"),
                new SVVarNode("发送任务时间", 4, "INT"),
                new SVVarNode("发送任务最大时间", 5, "INT"),
                new SVVarNode("杂项任务时间", 6, "INT"),
                new SVVarNode("杂项任务最大时间", 7, "INT"),

                new SVVarNode("当前空闲时间", 8, "INT"),
                new SVVarNode("当前周期时间", 9, "INT"),
                new SVVarNode("周期最大设置时间", 10, "INT"),

                new SVVarNode("当前周期个数", 11, "INT"),

                new SVVarNode("平台ROM状态", 12, "INT"),
                new SVVarNode("平台RAM状态", 13, "INT"),
                new SVVarNode("cpu诊断状态", 14, "INT"),
                new SVVarNode("时钟诊断状态", 15, "INT"),

                new SVVarNode("内存使用率", 16, "INT"),
                new SVVarNode("cpu使用率", 17, "INT"),

                new SVVarNode("数据接收状态", 18, "INT"),
                new SVVarNode("数据发送状态", 19, "INT"),
                new SVVarNode("人机交互状态", 20, "INT"),

                new SVVarNode("当前运行模式", 21, "INT"),

                new SVVarNode("接收任务超时", 22, "INT"),
                new SVVarNode("显示任务超时", 23, "INT"),
                new SVVarNode("发送任务超时", 24, "INT"),
                new SVVarNode("杂项任务超时", 25, "INT"),
                new SVVarNode("周期超时", 26, "INT")
            };

            foreach (var item in vList)
            {
                DataRow dr = systemDataTable.NewRow();
                dr["ioblockname"] = item.Name;
                dr["varAddress"] = item.Address;
                dr["valueType"] = item.Type;
                systemDataTable.Rows.Add(dr);
            }

            return systemDataTable;
        }

        /// <summary>
        /// 将当前表格中地址偏移到24K之后
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private void dealDataTableAddress(DataTable src)
        {
            int offset = 24 * 1024;

            for (int i = 0; i < src.Rows.Count; i++)
            {
                src.Rows[i][1] = (int)(src.Rows[i][1]) + offset;
            }
        }

        /// <summary>
        /// 加载接收表格地址
        /// </summary>
        /// <returns></returns>
        public DataTable loadRecvDataTable()
        {
            SVInterfaceApplication app = SVApplication.Instance;
            SVSqlDataBase sqlDataBase = app.DataBase;
            DataTable recvIO = sqlDataBase.getRecvAddressForIO(SVProData.stationID);
            DataTable recvIO2 = sqlDataBase.getRecvAddressForIO2(SVProData.stationID);
            DataTable recvNormal = sqlDataBase.getRecvAddressForNormal(SVProData.stationID);
            DataTable recvNormal2 = sqlDataBase.getRecvAddressForNormal2(SVProData.stationID);

            DataTable result = new DataTable();
            ///合并表格
            if (recvIO != null)
                result.Merge(recvIO);

            if (recvIO2 != null)
            {
                dealDataTableAddress(recvIO2);
                result.Merge(recvIO2);
            }

            if (recvNormal != null)
            {
                recvNormal.Columns[0].ColumnName = "ioblockname";
                result.Merge(recvNormal);
            }

            if (recvNormal2 != null)
            {
                recvNormal2.Columns[0].ColumnName = "ioblockname";
                dealDataTableAddress(recvNormal2);
                result.Merge(recvNormal2);
            }

            return result;
        }

        /// <summary>
        /// 获取发送地址表格
        /// </summary>
        /// <returns></returns>
        public DataTable loadSendDataTable()
        {
            SVInterfaceApplication app = SVApplication.Instance;
            SVSqlDataBase sqlDataBase = app.DataBase;

            DataTable sendIO = sqlDataBase.getSendAddressForIO(SVProData.stationID);
            DataTable sendIO2 = sqlDataBase.getSendAddressForIO2(SVProData.stationID);
            DataTable sendNormal = sqlDataBase.getSendAddressForNormal(SVProData.stationID);
            DataTable sendNormal2 = sqlDataBase.getSendAddressForNormal2(SVProData.stationID);

            ///合并表格            
            DataTable result = new DataTable();

            if (sendIO != null)
                result.Merge(sendIO);

            if (sendIO2 != null)
            {
                dealDataTableAddress(sendIO2);
                result.Merge(sendIO2);
            }

            if (sendNormal != null)
            {
                sendNormal.Columns[0].ColumnName = "ioblockname";
                result.Merge(sendNormal);
            }

            if (sendNormal2 != null)
            {
                sendNormal2.Columns[0].ColumnName = "ioblockname";
                dealDataTableAddress(sendNormal2);
                result.Merge(sendNormal2);
            }

            return result;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SVVaribleType()
        {
            _dict = new Dictionary<string, VarType>() 
            {
            { "BOOL", VarType.VARTYPE_CHAR }, 
            { "BOOL_S", VarType.VARTYPE_CHARQ }, 
            { "SHORT_INT", VarType.VARTYPE_INT16 }, 
            { "SHORTINT_S", VarType.VARTYPE_INT16Q }, 
            { "INT", VarType.VARTYPE_INT32 }, 
            { "INT_S", VarType.VARTYPE_INT32Q }, 
            { "REAL", VarType.VARTYPE_FLOAT }, 
            { "REAL_S", VarType.VARTYPE_FLOATQ }};
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

    /// <summary>
    /// 一个变量的定义类
    /// </summary>
    [Serializable]
    public class SVVarDefine : ICloneable
    {
        /// <summary>
        /// 变量名称
        /// </summary>
        public String VarName { get; set; }

        /// <summary>
        /// 变量种类类型,表示接收区、发送区、系统区类型、中间变量
        /// 值分别为0,1,2,3
        /// </summary>
        public Byte VarBlockType { get; set; }

        /// <summary>
        /// 判断当前变量在替换功能中是否合法
        /// </summary>
        /// <returns></returns>
        public Boolean isValid()
        {
            if (String.IsNullOrWhiteSpace(VarName) || VarBlockType == 3)
                return false;

            return true;
        }

        /// <summary>
        /// 用来在结构中能够使用Contains函数来判断重复性
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true-表示已经存在，false-表示不存在</returns>
        public override bool Equals(object other)
        {
            SVVarDefine otherVar = other as SVVarDefine;
            if (otherVar == null)
                return true;

            if (this.VarName == otherVar.VarName 
                && this.VarBlockType == otherVar.VarBlockType)
                return true;

            return false;
        }

        public object Clone()
        {
            SVVarDefine r = new SVVarDefine();
            r.VarName = this.VarName;
            r.VarBlockType = this.VarBlockType;
            return r;
        }
    }
}
