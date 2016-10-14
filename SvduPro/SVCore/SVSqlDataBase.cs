using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace SVCore
{
    /// <summary>
    /// 定义一个操作数据库的类,这里仅对mysql数据库操作进行包装
    /// </summary>
    public class SVSqlDataBase
    {
        MySqlConnection _mysqlConnect = new MySqlConnection();

        /// <summary>
        /// 连接mysql数据库
        /// </summary>
        /// <param name="ip">数据库ip地址</param>
        /// <param name="database">数据库中对应的表</param>
        /// <param name="user">用户名</param>
        /// <param name="passwd">密码</param>
        /// <param name="charset">字符集，默认UTF-8</param>
        /// <returns>true连接成功，false连接失败</returns>
        public Boolean connect(String ip, String database, String user, String passwd, String charset = "utf8")
        {
            if (String.IsNullOrEmpty(ip)
                || String.IsNullOrEmpty(database)
                || String.IsNullOrEmpty(user)
                || String.IsNullOrEmpty(passwd)
                || String.IsNullOrEmpty(charset))
                return false;

            //String connName = "Database='safeware';Data Source='192.168.2.103';User Id='lixiaolong';Password='123456';charset='utf8';pooling=true";
            String connName = String.Format(@"Database={0};Data Source={1};
                  User Id={2};Password={3};charset={4};
                  pooling = true; Connection Timeout = 1", database, ip, user, passwd, charset);

            try
            {
                _mysqlConnect.ConnectionString = connName;
                _mysqlConnect.Open();

                SVLog.WinLog.Info("打开数据库连接成功");
            }
            catch (MySqlException ex)
            {
                SVLog.WinLog.Warning("打开数据库连接失败");
                SVLog.TextLog.Exception(ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取当前工程的路径
        /// </summary>
        /// <returns>返回路径</returns>
        public String getProPath()
        {
            String sql = String.Format(@"select value_str from sys_parameter where name = '{0}'", "project_path");
            MySqlCommand mysqlcom = new MySqlCommand(sql, _mysqlConnect);

            MySqlDataReader reader = null;

            try
            {
                reader = mysqlcom.ExecuteReader();
                reader.Read();
                return reader.GetString(0);
            }
            catch (MySqlException ex)
            {
                SVLog.TextLog.Exception(ex);
                return String.Empty;
            }
            catch (Exception ex)
            {
                SVLog.TextLog.Exception(ex);
                return String.Empty;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// 获取当前站点的变量名称及地址
        /// </summary>
        /// <param name="stationID">站号</param>
        /// <returns>变量对应的内存表格</returns>
        public DataTable getVarDataList(Int32 stationID)
        {
            String sql = String.Format(@"select table_channelinfo.ioblockname, table_netinfo.addressoffset, table_valuetype.valueType
                from table_varinfo, table_channelinfo, table_netinfo, table_valuetype
                where table_varinfo.stationid = 11 
                and (table_varinfo.varname = 'IN' or table_varinfo.varname = 'Y') 
                and table_varinfo.ioblockid = table_channelinfo.uid 
                and table_varinfo.uid = table_netinfo.connid
                and table_netinfo.netvarid = table_valuetype.uid", stationID);

            MySqlCommand mysqlcom = new MySqlCommand(sql, _mysqlConnect);

            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = mysqlcom;
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
            catch (MySqlException ex)
            {
                SVLog.TextLog.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            _mysqlConnect.Clone();
            _mysqlConnect.Dispose();

            SVLog.WinLog.Info("关闭数据库连接");
        }
    }
}
