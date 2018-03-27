using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Food
    {
        public DataTable GetFoodInfoList()
        {
            string strSql = @"SELECT   [id]
                                      ,[type]
                                      ,[sub_type]
                                      ,[title]
                                      ,[energy_kilocal]
                                      ,[ca_mg]
                                      ,[protein_g]
                                      ,[riboflavin_mg]
                                      ,[mg_mg]
                                      ,[fat_g]
                                      ,[niacin_mg]
                                      ,[fe_mg]
                                      ,[vc_mg]
                                      ,[mn_mg]
                                      ,[ve_mg]
                                      ,[zn_mg]
                                      ,[cu_mg]
                                      ,[k_mg]
                                      ,[p_mg]
                                      ,[re_ug]
                                      ,[na_mg]
                                      ,[se_ug]
                                  FROM [HealthControlDB].[dbo].[hc_food]";
            strSql = string.Format(strSql);
            DataTable dt = DBHelper.SqlHelper.GetDataTable(strSql);
            return dt;
        }
    }
}
