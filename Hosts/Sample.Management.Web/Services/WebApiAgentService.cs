using Auth.Infrastructure.Mvc;
using Auth.Infrastructure.Utility;
using Newtonsoft.Json;
using Sample.Management.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Services
{
    public class WebApiAgentService
    {

        DataResult FailData = new DataResult { Code = 0, Message = "系统繁忙，请稍后再试！" };

        public WebApiAgentService()
        {

        }

        public bool InvokeAgentService(string apiName, Dictionary<string, object> parameters, out string outDataResponse, int channelId = 1000060)
        {
            outDataResponse = JsonConvert.SerializeObject(FailData);
            try
            {
                string ip = CommFunction.StringParse(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"], "");
                if (string.IsNullOrEmpty(ip))
                {
                    ip = CommFunction.StringParse(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"], "");
                }

                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }
                if (!parameters.ContainsKey("PubParams"))
                    parameters.Add("PubParams", new { Ip = ip, ChannelId = channelId });

                //获取请求地址
                var result = WebHelp.PostMode(SystemConfig.AgentInterfaceUri + "/" + apiName, JsonConvert.SerializeObject(parameters));
                if (string.IsNullOrEmpty(result))
                {
                    return false;
                }
                else
                {
                    outDataResponse = result;
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}