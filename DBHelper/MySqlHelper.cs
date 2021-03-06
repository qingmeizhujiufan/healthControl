﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MySql.Data.MySqlClient;

using ConfigHelper;

namespace DBHelper
{
    class MySqlHelper : ISqlHelper
    {
        public string ConnectionString
        {
            get
            {
                return ConfigHelper.ConfigHelper.DBConfigModal.CurrentConnectionString;
            }
        }

        public DataSet GetDataSet(string strSql)
        {
            DataSet ds = new DataSet();
            using (this.conn = this.GetOleDbConnection())
            {
                MySqlCommand cmd = new MySqlCommand(strSql, this.conn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                try
                {
                    this.conn.Open();
                    da.Fill(ds);
                }
                catch (Exception ex)
                {
                    CommonTool.WriteLog.Write(ex.Message);
                    throw ex;
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

        public DataTable GetDataTable(string strSql)
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public int ExecuteSql(string strSql)
        {
            int intRtn = -1;

            return intRtn;
        }

        public string GetDataItemString(string strSql)
        {
            string strRtn = string.Empty;

            return strRtn;
        }

        public double GetDataItemDouble(string strSql)
        {
            double dblRtn = 0.00;


            return dblRtn;
        }

        public DateTime GetDataItemDateTime(string strSql)
        {
            DateTime dtmRtn = new DateTime();


            return dtmRtn;
        }

        public bool Exist(string strTableName, string strFieldName, string strValue)
        {
            bool bRtn = true;

            return bRtn;
        }

        public int Update(string tableName, string id, string idValue, string field, string fieldValue)
        {
            int intTag = 0;
            

            return intTag;
        }

        public int Update(string tableName, string id, string idValue, string field, int fieldValue)
        {
            int intTag = 0;
        
            return intTag;
        }

        public int Delete(string tableName, string id, string idValue)
        {
            int intTag = 0;

            return intTag;
        }


        #region 类内部数据和方法

        protected MySqlConnection conn = null;

        protected MySqlConnection GetOleDbConnection()
        {
            this.conn = new MySqlConnection(this.ConnectionString);
            return this.conn;
        }

        #endregion 
    }
}
