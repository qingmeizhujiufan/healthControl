using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class User
    {
        //查询用户信息
        public DataTable getUserInfo(string deviceId)
        {
            string str = @"select   id,
                                    open_id,
                                    ISNULL(name, '') as name,
                                    ISNULL(age, 0) as age,
                                    ISNULL(sex, '') as sex,
                                    ISNULL(telephone, '') as telephone,
                                    ISNULL(period, '') as period,
                                    ISNULL(height, 0) as height,
                                    ISNULL(weight, 0) as weight,
                                    ISNULL(waistline, 0) as waistline,
                                    ISNULL(workingType, '') as workingType
                            from dbo.hc_user
                            where device_id='{0}'";
            str = string.Format(str, deviceId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //新增用户
        public bool AddUser(string deviceId, string openId)
        {
            string str = string.Empty;
            str = @"select id from dbo.hc_user where device_id = '{0}' and open_id = '{1}'";
            str = string.Format(str, deviceId, openId);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                str = @"insert into dbo.hc_user (device_id, open_id) values ('{0}', '{1}')";
                str = string.Format(str, deviceId, openId);
                int flag = DBHelper.SqlHelper.ExecuteSql(str);

                return flag > 0 ? true : false;
            }
        }

        //更新用户信息
        public bool UpdateUserInfo(string deviceId, string fieldName, string fieldValue)
        {
            bool bRtn = false;

            if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(fieldValue))
            {
                bRtn = false;
                return bRtn;
            }

            string strSql = "update dbo.hc_user set {0} = '{1}' where device_id = '{2}'";
            strSql = string.Format(strSql, fieldName, fieldValue, deviceId);
            int tag = DBHelper.SqlHelper.ExecuteSql(strSql);
            if (tag > 0)
            {
                bRtn = true;
            }

            return bRtn;
        }

        //新增用户答卷分数
        public bool AddScoreResult(string device_id, string score)
        {
            bool bRtn = false;

            if (string.IsNullOrEmpty(device_id) || string.IsNullOrEmpty(score))
            {
                bRtn = false;
                return bRtn;
            }
            string str = @"select open_id from dbo.hc_user where device_id = '{0}'";
            str = string.Format(str, device_id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);
            if (dt.Rows.Count == 1)
            {
                string strSql = "insert into dbo.hc_stress_score (open_id, score) values ('{0}', '{1}')";
                strSql = string.Format(strSql, device_id, score);
                int tag = DBHelper.SqlHelper.ExecuteSql(strSql);
                if (tag > 0)
                {
                    bRtn = true;
                }
            }

            return bRtn;
        }

        //获取用户最新压力测试结果
        public DataTable GetLatestPresureScoreResult(string device_id, int month)
        {
            string str = @"select   id, 
	                                score,
	                                CONVERT(varchar(10) , create_time, 120 ) as create_time 
                                    from dbo.hc_stress_score
                                    where open_id = (
										select open_id 
										from dbo.hc_user 
										where device_id = '{0}'
									) 
                                    and DATEPART(m,create_time) = {1}
                                    order by create_time asc";
            str = string.Format(str, device_id, month);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }

        //新增用户生活方式问卷分数
        public bool AddLifeResult(string device_id, string smoke_score, string drink_score, string life_score, string exercise_score, string sleep_score)
        {
            bool bRtn = false;

            if (string.IsNullOrEmpty(device_id) || string.IsNullOrEmpty(smoke_score) || string.IsNullOrEmpty(drink_score) || string.IsNullOrEmpty(life_score) || string.IsNullOrEmpty(exercise_score) || string.IsNullOrEmpty(sleep_score))
            {
                bRtn = false;
                return bRtn;
            }
            string str = @"select open_id from dbo.hc_user where device_id = '{0}'";
            str = string.Format(str, device_id);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);
            if (dt.Rows.Count == 1)
            {
                string open_id = dt.Rows[0]["open_id"].ToString();
                string strSql = "insert into dbo.hc_life_score (open_id, smoke_score, drink_score, life_score, exercise_score, sleep_score) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')";
                strSql = string.Format(strSql, open_id, smoke_score, drink_score, life_score, exercise_score, sleep_score);
                CommonTool.WriteLog.Write("AddScoreResult == " + strSql);
                int tag = DBHelper.SqlHelper.ExecuteSql(strSql);
                if (tag > 0)
                {
                    bRtn = true;
                }
            }          

            return bRtn;
        }

        //获取用户最新生活方式测试结果
        public DataTable GetLatestLifeStyleResult(string device_id, int month)
        {
            string str = @"select       id, 
                                        smoke_score, 
                                        drink_score, 
                                        life_score, 
                                        exercise_score, 
                                        sleep_score,
                                        CONVERT(varchar(10) , create_time, 120 ) as create_time
                                    from dbo.hc_life_score
                                    where open_id = (
										select open_id 
										from dbo.hc_user 
										where device_id = '{0}'
									)
                                    and DATEPART(m, create_time) = {1}
                                    order by create_time asc";
            str = string.Format(str, device_id, month);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(str);

            return dt;
        }
    }
}
