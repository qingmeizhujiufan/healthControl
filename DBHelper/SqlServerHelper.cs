using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using ConfigHelper;

namespace DBHelper
{
    /// <summary>
    /// SqlServer的数据库帮助类
    /// </summary>
    public class SqlServerHelper : ISqlHelper
    {
        #region 实现接口行为部分  

        public string ConnectionString
        {
            get
            {
                return ConfigHelper.ConfigHelper.DBConfigModal.CurrentConnectionString;
            }
        }

        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string strSql)
        {
            DataSet ds = new DataSet();
            using (this.conn = this.GetSqlConnection())
            {
                SqlCommand cmd = new SqlCommand(strSql, this.conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {
                    this.conn.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    //CommonTool.WriteLog.Write(ex.Message);
                    //throw ex;
                }
                finally
                {
                    this.conn.Close();
                    this.conn.Dispose();
                    da.Dispose();
                    cmd.Dispose();
                }
            }
            return ds;
        }

        /// <summary>
        /// 获取 DataTable
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string strSql)
        {
            DataTable dt = new DataTable();
            using (this.conn = this.GetSqlConnection())
            {
                SqlCommand cmd = new SqlCommand(strSql, this.conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {
                    this.conn.Open();
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //CommonTool.WriteLog.Write(ex.Message);
                    //throw ex;
                }
                finally
                {
                    this.conn.Close();
                    this.conn.Dispose();
                    da.Dispose();
                    cmd.Dispose();
                }
            }
            return dt;
        }

        /// <summary>
        /// 执行sql命令，完成增，删，改功能
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public int ExecuteSql(string strSql)
        {
            int intRtn = -1;
            using (this.conn = this.GetSqlConnection())
            {
                SqlCommand cmd = new SqlCommand(strSql, this.conn);
                try
                {
                    this.conn.Open();
                    intRtn = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //CommonTool.WriteLog.Write(ex.Message);
                    //throw ex;
                }
                finally
                {
                    this.conn.Close();
                    this.conn.Dispose();
                    cmd.Dispose();
                }
            }
            return intRtn;
        }

        public string GetDataItemString(string strSql)
        {
            string strRtn = string.Empty;
            
            using (this.conn = this.GetSqlConnection())
            {
                SqlCommand cmd = new SqlCommand(strSql, this.conn);
                try
                {
                    this.conn.Open();
                    
                        object obj = cmd.ExecuteScalar();
                        strRtn = obj == null ? "" : obj.ToString();
                  
                   
                }
                catch (Exception ex)
                {
                    //CommonTool.WriteLog.Write(ex.Message);
                    //throw ex;
                }
                finally
                {
                    this.conn.Close();
                    this.conn.Dispose();
                    cmd.Dispose();
                }
            }
           
            return strRtn;
        }

        public double GetDataItemDouble(string strSql)
        {
            double dblRtn = 0.00;

            using (this.conn = this.GetSqlConnection())
            {
                SqlCommand cmd = new SqlCommand(strSql, this.conn);
                try
                {
                    this.conn.Open();
                    object obj = cmd.ExecuteScalar();
                    dblRtn = obj == null ? 0.00 : Convert.ToDouble(obj.ToString());
                }
                catch (Exception ex)
                {
                    //CommonTool.WriteLog.Write(ex.Message);
                    //throw ex;
                }
                finally
                {
                    this.conn.Close();
                    this.conn.Dispose();
                    cmd.Dispose();
                }
            }

            return dblRtn;
        }

        public DateTime GetDataItemDateTime(string strSql)
        {
            DateTime dtmRtn = new DateTime();

            using (this.conn = this.GetSqlConnection())
            {
                SqlCommand cmd = new SqlCommand(strSql, this.conn);
                try
                {
                    this.conn.Open();
                    object obj = cmd.ExecuteScalar();
                    dtmRtn = obj == null ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(obj.ToString());
                }
                catch (Exception ex)
                {
                    //CommonTool.WriteLog.Write(ex.Message);
                    //throw ex;
                }
                finally
                {
                    this.conn.Close();
                    this.conn.Dispose();
                    cmd.Dispose();
                }
            }

            return dtmRtn;
        }

        public bool Exist(string strTableName, string strFieldName, string strValue)
        {
            bool bRtn = true;
            if(string.IsNullOrEmpty(strTableName) || string.IsNullOrEmpty(strFieldName))
            {
                bRtn = false;
                return bRtn;
            }
            string strSql = "select 1 from {0} where {1} = '{2}'";
            strSql = string.Format(strSql, strTableName, strFieldName, strValue);
            string str = this.GetDataItemString(strSql);
            if (str == "1")
            {
                bRtn = true;
            }
            else
            {
                bRtn = false;
            }

            return bRtn;
        }

        public int Update(string tableName, string id, string idValue, string field, string fieldValue)
        {
            int intTag = 0;
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(idValue) || string.IsNullOrEmpty(field))
            {
                intTag = 0;
                return intTag;
            }
            string strSql = "update {0} set {1} = '{2}' where {3} = '{4}'";
            strSql = string.Format(strSql, tableName, field, fieldValue, id, idValue);
            intTag = this.ExecuteSql(strSql);

            return intTag;
        }

        public int Update(string tableName, string id, string idValue, string field, int fieldValue)
        {
            int intTag = 0;
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(idValue) || string.IsNullOrEmpty(field))
            {
                intTag = 0;
                return intTag;
            }
            string strSql = "update {0} set {1} = {2} where {3} = '{4}'";
            strSql = string.Format(strSql, tableName, field, fieldValue, id, idValue);
            intTag = this.ExecuteSql(strSql);

            return intTag;
        }

        public int Delete(string tableName, string id, string idValue)
        {
            int intTag = 0;
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(idValue))
            {
                intTag = 0;
                return intTag;
            }
            string strSql = "delete from {0} where {1} = '{2}'";
            strSql = string.Format(strSql, tableName, id, idValue);
            intTag = this.ExecuteSql(strSql);

            return intTag;
        }

        #endregion 

        #region 类内部数据和方法

        protected SqlConnection conn = null;
        
        protected SqlConnection GetSqlConnection()
        {
            this.conn = new SqlConnection(this.ConnectionString);
            return this.conn;
        }

        #endregion 
    }
}
