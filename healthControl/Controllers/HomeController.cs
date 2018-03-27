using BLL;
using CommonTool;
using healthControl.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Xml;
using System.Xml.Linq;

namespace healthControl.Controllers
{
    public class HomeController : Controller
    {

        #region 微信支付

        public ActionResult GetOpenId()
        {
            string strCode = Request.QueryString["code"];
            string callBackUrl = Request.QueryString["callBackUrl"];
            string strOpenID = CommonTool.WXOperate.GetOpenID(strCode);
            Session["openid"] = strOpenID;

            return Redirect(callBackUrl);
        }

        public string WxPay(string SendData)
        {
            string strReturn = CommonTool.WXOperate.WxPayFor(SendData);
            return strReturn;
        }

        public string WxConfig(string SendData)
        {
            string strReturn = CommonTool.WXOperate.WxConfig(SendData);
            return strReturn;
        }
        #endregion


        public ActionResult Index(string scene_id)
        {
            string strAppID = CommonTool.WXParam.APP_ID;
            string strTicket = CommonTool.WXOperate.GetTicket();
            string strNoncestr = CommonTool.Common.GetRandom(16);
            string strTimestamp = CommonTool.Common.GetTimeStamp();
            string string1 = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", strTicket, strNoncestr, strTimestamp, "http://www.rusng.cn/");

            //SHA1加密获取签名
            string strSign = CommonTool.Common.getSHA1(string1);

            ViewData["appid"] = strAppID;
            ViewData["timestamp"] = strTimestamp;
            ViewData["nonceStr"] = strNoncestr;
            ViewData["signature"] = strSign;

            return View();
        }

        public ActionResult HealthIndicator()
        {
            return View();
        }

        public ActionResult NutritionAssessment()
        {
            return View();
        }

        public ActionResult AddNutritionData()
        {
            return View();
        }

        public ActionResult SportsReport()
        {
            return View();
        }

        public ActionResult LifeStyleReport()
        {
            return View();
        }

        public ActionResult LifeStyleQuestion()
        {
            return View();
        }

        public ActionResult StressTesting()
        {
            return View();
        }

        public ActionResult StressQuestion() {
            return View();
        }

        public ActionResult UserInfo()
        {
            return View();
        }

        #region 微信设备消息处理
        public string ScanWX()
        {
            {
                string result = "";
                string postString = string.Empty;
                if (Request.HttpMethod.ToUpper() == "POST")
                {
                    using (Stream stream = System.Web.HttpContext.Current.Request.InputStream)
                    {
                        Byte[] postBytes = new Byte[stream.Length];
                        stream.Read(postBytes, 0, (Int32)stream.Length);
                        postString = Encoding.UTF8.GetString(postBytes);
                        //postBytes = CommonTool.Base64Decoder.GetDecoded(postString);
                        CommonTool.WriteLog.Write("postString: " + postString);
                        if (!string.IsNullOrEmpty(postString))
                        {

                            string SendToWx = string.Empty;

                            Dictionary<string, string> dic = CommonTool.JsonHelper.GetParms2(postString);
                            CommonTool.WriteLog.Write("content === " + dic["content"]);
                            byte[] aryData = new Base64Decoder().GetDecoded(dic["content"]);
                            string DE_content = Encoding.UTF8.GetString(aryData);

                            int index = DE_content.IndexOf('{');
                            DE_content = DE_content.Substring(index);
                            CommonTool.WriteLog.Write("DE_content === " + DE_content);
                            Dictionary<string, string> dic2 = CommonTool.JsonHelper.GetParms2(DE_content);
                            string device_id = dic["device_id"];
                            string device_type = dic["device_type"];
                            string msg_id = dic["msg_id"];
                            string msg_type = dic["msg_type"];
                            string open_id = dic["open_id"];
                            string session_id = dic["session_id"];
                            string bat = dic2["bat"];
                            string hrs = dic2["hrs"];
                            string step = dic2["step"];
                            string lslt = dic2["lslt"];
                            string dslt = dic2["dslt"];
                            string btmp = dic2["btmp"];
                            string hbld = dic2["hbld"];
                            string lbld = dic2["lbld"];
                            string oxyg = dic2["oxyg"];
                            string atmp = dic2["atmp"];
                            Device dev = new Device();
                            bool flag =  dev.saveDeviceInfo(device_id, device_type, msg_id, msg_type, open_id, session_id, bat, hrs, step, lslt, dslt, btmp, hbld, lbld, oxyg, atmp);
                            if (flag)
                            {
                                User user = new User();
                                user.AddUser(device_id, open_id);
                            }
                            //这里写方法解析Xml
                            XmlDocument xml = new XmlDocument();
                            xml.LoadXml(postString);
                            XmlElement xmlElement = xml.DocumentElement;
                            //这里进行判断MsgType
                            switch (xmlElement.SelectSingleNode("//MsgType").InnerText)
                            {
                                case "text":
                                    XmlNode content = xmlElement.SelectSingleNode("//Content");
                                    CommonTool.WriteLog.Write("content: " + xmlElement.SelectSingleNode("//Content").InnerText);
                                    SendToWx = WxText.GetWxTextXml(postString);
                                    break;
                            }
                            if (!string.IsNullOrEmpty(SendToWx))
                            {
                                System.Web.HttpContext.Current.Response.Write(SendToWx);
                                System.Web.HttpContext.Current.Response.End();
                            }
                            else
                            {
                                result = "回传数据为空";
                                CommonTool.WriteLog.Write(result);
                            }
                        }
                        else
                        {
                            result = "微信推送的数据为：" + postString;
                            CommonTool.WriteLog.Write(result);
                        }
                    }
                }
                else if (Request.HttpMethod.ToUpper() == "GET")
                {
                    string token = "qweasdzxc";//从配置文件获取Token
                    if (string.IsNullOrEmpty(token))
                    {
                        result = string.Format("微信Token配置项没有配置！");
                        CommonTool.WriteLog.Write(result);
                    }
                    string echoString = System.Web.HttpContext.Current.Request.QueryString["echoStr"];
                    string signature = System.Web.HttpContext.Current.Request.QueryString["signature"];
                    string timestamp = System.Web.HttpContext.Current.Request.QueryString["timestamp"];
                    string nonce = System.Web.HttpContext.Current.Request.QueryString["nonce"];
                    if (CheckSignature(token, signature, timestamp, nonce))
                    {
                        if (!string.IsNullOrEmpty(echoString))
                        {
                            System.Web.HttpContext.Current.Response.Write(echoString);
                            System.Web.HttpContext.Current.Response.End();
                            result = string.Format("微信Token配置成功，你已成为开发者！");
                            CommonTool.WriteLog.Write(result + "\t\nechoString:" + echoString + "  " + signature + "  " + timestamp + "  " + nonce);
                            return echoString;
                        }
                    }
                }
                result = string.Format("页面被访问，没有请求数据！");
                CommonTool.WriteLog.Write(result);
                return result;
            }
        }

        public bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 接口
        //获取设备所有数据
        public JsonResult getDeviceInfoList(string deviceId, string date)
        {
            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            BLL.Device _device = new BLL.Device();
            DataTable dt = _device.GetDeviceInfoList(deviceId, date);
            List<device> deviceInfoList = new List<device>();
            if (dt.Rows.Count >= 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    device dev = new device();
                    dev.id = dt.Rows[i]["id"].ToString();
                    dev.device_id = dt.Rows[i]["device_id"].ToString();
                    dev.device_type = dt.Rows[i]["device_type"].ToString();
                    dev.msg_id = dt.Rows[i]["msg_id"].ToString();
                    dev.msg_type = dt.Rows[i]["msg_type"].ToString();
                    dev.open_id = dt.Rows[i]["open_id"].ToString();
                    dev.session_id = dt.Rows[i]["session_id"].ToString();
                    dev.bat = dt.Rows[i]["bat"].ToString();
                    dev.hrs = dt.Rows[i]["hrs"].ToString();
                    dev.step = dt.Rows[i]["step"].ToString();
                    dev.lslt = dt.Rows[i]["lslt"].ToString();
                    dev.dslt = dt.Rows[i]["dslt"].ToString();
                    dev.btmp = dt.Rows[i]["btmp"].ToString();
                    dev.hbld = dt.Rows[i]["hbld"].ToString();
                    dev.lbld = dt.Rows[i]["lbld"].ToString();
                    dev.oxyg = dt.Rows[i]["oxyg"].ToString();
                    dev.atmp = dt.Rows[i]["atmp"].ToString();
                    dev.create_time = dt.Rows[i]["create_time"].ToString();
                    deviceInfoList.Add(dev);
                }
                res.Data = new
                {
                    success = true,
                    backData = deviceInfoList
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "地址查询失败"
                };
            }

            return res;
        }

        //获取膳食所有数据
        public JsonResult getFoodInfoList()
        {
            var res = new JsonResult();
            BLL.Food _food = new BLL.Food();
            DataTable dt = _food.GetFoodInfoList();
            List<food> foodInfoList = new List<food>();
            if (dt.Rows.Count >= 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    food f = new food();
                    f.id = dt.Rows[i]["id"].ToString();
                    f.type = dt.Rows[i]["type"].ToString();
                    f.sub_type = dt.Rows[i]["sub_type"].ToString();
                    f.title = dt.Rows[i]["title"].ToString();
                    f.energy_kilocal = Convert.ToDouble(dt.Rows[i]["energy_kilocal"].ToString());
                    f.ca_mg = Convert.ToDouble(dt.Rows[i]["ca_mg"].ToString());
                    f.protein_g = Convert.ToDouble(dt.Rows[i]["protein_g"].ToString());
                    f.riboflavin_mg = Convert.ToDouble(dt.Rows[i]["riboflavin_mg"].ToString());
                    f.mg_mg = Convert.ToDouble(dt.Rows[i]["mg_mg"].ToString());
                    f.fat_g = Convert.ToDouble(dt.Rows[i]["fat_g"].ToString());
                    f.niacin_mg = Convert.ToDouble(dt.Rows[i]["niacin_mg"].ToString());
                    f.fe_mg = Convert.ToDouble(dt.Rows[i]["fe_mg"].ToString());
                    f.vc_mg = Convert.ToDouble(dt.Rows[i]["vc_mg"].ToString());
                    f.mn_mg = Convert.ToDouble(dt.Rows[i]["mn_mg"].ToString());
                    f.ve_mg = Convert.ToDouble(dt.Rows[i]["ve_mg"].ToString());
                    f.zn_mg = Convert.ToDouble(dt.Rows[i]["zn_mg"].ToString());
                    f.cu_mg = Convert.ToDouble(dt.Rows[i]["cu_mg"].ToString());
                    f.k_mg = Convert.ToDouble(dt.Rows[i]["k_mg"].ToString());
                    f.p_mg = Convert.ToDouble(dt.Rows[i]["p_mg"].ToString());
                    f.re_ug = Convert.ToDouble(dt.Rows[i]["re_ug"].ToString());
                    f.na_mg = Convert.ToDouble(dt.Rows[i]["na_mg"].ToString());
                    f.se_ug = Convert.ToDouble(dt.Rows[i]["se_ug"].ToString());

                    foodInfoList.Add(f);
                }
                res.Data = new
                {
                    success = true,
                    backData = foodInfoList
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "膳食查询失败"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //获取用户信息
        public JsonResult getUserInfo(string deviceId)
        {
            var res = new JsonResult();
            BLL.User _user = new BLL.User();
            DataTable dt = _user.getUserInfo(deviceId);
            if (dt.Rows.Count > 0)
            {
                user u = new user();
                u.id = dt.Rows[0]["id"].ToString();
                u.open_id = dt.Rows[0]["open_id"].ToString();
                u.name = dt.Rows[0]["name"].ToString();
                u.age = Convert.ToInt32(dt.Rows[0]["age"].ToString());
                u.sex = dt.Rows[0]["sex"].ToString();
                u.telephone = dt.Rows[0]["telephone"].ToString();
                u.period = dt.Rows[0]["period"].ToString();
                u.height = Convert.ToSingle(dt.Rows[0]["height"].ToString());
                u.weight = Convert.ToSingle(dt.Rows[0]["weight"].ToString());
                u.waistline = Convert.ToSingle(dt.Rows[0]["waistline"].ToString());
                u.workingType = dt.Rows[0]["workingType"].ToString();

                res.Data = new
                {
                    success = true,
                    backData = u
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "用户查询失败"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //更新用户信息
        public JsonResult updateUserInfo(string deviceId, string fieldName, string fieldValue)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            bool flag = user.UpdateUserInfo(deviceId, fieldName, fieldValue);
            if (flag)
            {
                res.Data = new
                {
                    success = true
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "修改失败，请重试"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //新增压力测试结果
        public JsonResult addScoreResult(string deviceId, string score)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            bool flag = user.AddScoreResult(deviceId, score);
            if (flag)
            {
                res.Data = new
                {
                    success = true
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "提交失败，请重试"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //获取最新压力测试结果
        public JsonResult getLatestPresureScoreResult(string deviceId, int month)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            DataTable dt = user.GetLatestPresureScoreResult(deviceId, month);
            if (dt.Rows.Count > 0)
            {
                res.Data = new
                {
                    success = true,
                    backData = CommonTool.JsonHelper.DataTableToJSON(dt)
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "请先进行压力测试问卷"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //新增生活方式评测结果
        public JsonResult addLifeResult(string device_id, string smoke_score, string drink_score, string life_score, string exercise_score, string sleep_score)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            bool flag = user.AddLifeResult(device_id, smoke_score, drink_score, life_score, exercise_score, sleep_score);
            if (flag)
            {
                res.Data = new
                {
                    success = true
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "提交失败，请重试"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        //获取最新生活方式评测结果
        public JsonResult getLatestLifeStyleResult(string deviceId, int month)
        {
            var res = new JsonResult();
            BLL.User user = new BLL.User();
            DataTable dt = user.GetLatestLifeStyleResult(deviceId, month);
            if (dt.Rows.Count > 0)
            {
                res.Data = new
                {
                    success = true,
                    backData = CommonTool.JsonHelper.DataTableToJSON(dt)
                };
            }
            else
            {
                res.Data = new
                {
                    success = false,
                    backMsg = "请先进行生活方式评测"
                };
            }
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。

            return res;
        }

        #endregion
    }
}
