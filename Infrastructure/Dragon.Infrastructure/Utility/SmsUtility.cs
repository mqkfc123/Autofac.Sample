using Dragon.Core.Log4net;
using Auth.Infrastructure.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dragon.Infrastructure.Utility
{

    public class SmsModel
    {
        //{"CODE":"1","TASKID":"183741720170612105232877","RESULT":"发送成功"}
        public int CODE { get; set; }

        public string TASKID { get; set; }

        public string RESULT { get; set; }
    }


    public class SmsUtility
    {
        private static readonly ILog _Logger = LogHelper.GetLogger(typeof(SmsUtility));

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tel"></param>
        /// <returns></returns>
        public static bool SendSms(string context, string tel)
        {
            string postData = @"action=Send&username=15080453548&password=617899BEADC8A0C73C2A7B566EF7E9E6&gwid=86&mobile=" + tel + "&message=" + HttpUtility.UrlEncode(context, Encoding.GetEncoding("GBK"));
            string smsUrl = ConfigurationManager.AppSettings["SmsUrl"]; 
            try
            {
                var result = WebHelp.PostModeGBK(smsUrl, postData);
                if (result.Length > 0)
                {
                    var sms = JsonConvert.DeserializeObject<SmsModel>(result);
                    return sms.CODE == 1 ? true : false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error($"短信接口{smsUrl} 异常 ", ex);
                return false;
            }
        }

    }


}
