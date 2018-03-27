using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Device
    {
        public DataTable GetDeviceInfoList(string deviceId, string date)
        {
            string strSql = @"select    id,
                                        ISNULL(device_id, '') as device_id,
                                        ISNULL(device_type, '') as device_type,
                                        ISNULL(msg_id, '') as msg_id,
                                        ISNULL(msg_type, '') as msg_type,
                                        ISNULL(open_id, '') as open_id,
                                        ISNULL(session_id, '') as session_id,
                                        ISNULL(bat, '') as bat,
                                        ISNULL(hrs, '') as hrs,
                                        ISNULL(step, '') as step,
                                        ISNULL(lslt, '') as lslt,
                                        ISNULL(dslt, '') as dslt,
                                        ISNULL(btmp, '') as btmp,
                                        ISNULL(hbld, '') as hbld,
                                        ISNULL(lbld, '') as lbld,
                                        ISNULL(oxyg, '') as oxyg,
                                        ISNULL(atmp, '') as atmp,
                                        CONVERT(varchar(19) , create_time, 120 ) as create_time
                                        from dbo.hc_deviceinfo 
                                        where device_id = '{0}' and datediff(day,create_time,'{1}')=0
                                        order by create_time asc";
            strSql = string.Format(strSql, deviceId, date);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }

        public bool saveDeviceInfo(string device_id, string device_type, string msg_id, string msg_type, string open_id, string session_id, string bat, string hrs, string step, string lslt, string dslt, string btmp, string hbld, string lbld, string oxyg, string atmp)
        {
            string str = @"insert into dbo.hc_deviceinfo
	                            (
		                            device_id,
		                            device_type,
		                            msg_id,
		                            msg_type,
		                            open_id,
		                            session_id,
		                            bat,
		                            hrs,
		                            step,
		                            lslt,
		                            dslt,
		                            btmp,
                                    hbld,
                                    lbld,
                                    oxyg,
                                    atmp
	                            )
	                            values
	                            (
		                            '{0}',
		                            '{1}',
		                            '{2}',
		                            '{3}',
		                            '{4}',
		                            '{5}',
		                            '{6}',
		                            '{7}',
		                            '{8}',
		                            '{9}',
		                            '{10}',
		                            '{11}',
                                    '{12}',
                                    '{13}',
                                    '{14}',
                                    '{15}'
	                            )";
            str = string.Format(str, device_id, device_type, msg_id, msg_type, open_id, session_id, bat, hrs, step, lslt, dslt, btmp, hbld, lbld, oxyg, atmp);
            int flag = DBHelper.SqlHelper.ExecuteSql(str);

            return flag > 0 ? true : false;
        }

    }
}
