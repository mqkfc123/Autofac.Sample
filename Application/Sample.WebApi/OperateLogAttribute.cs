using Autofac;
using Dragon.Core.Log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MutualClass.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OperateLogAttribute : ActionFilterAttribute
    {
        private readonly ILog _Logger = LogHelper.GetLogger(typeof(OperateLogAttribute));
        public readonly string OperateName;
        public OperateLogAttribute(string operateName)
        {
            OperateName = operateName;
        }

        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            try
            {
              //  var _systemLogService = CoreBuilderWork.LifetimeScope.Resolve<ISystemLogService>();
                var result = (dynamic)filterContext.Request;
                string param = string.Empty;

                var method = filterContext.Request.Method.ToString().ToLower();

                var objectContent = filterContext.Request.Properties;
                HttpContextBase context = (HttpContextBase)objectContent["MS_HttpContext"];//获取传统context
                HttpRequestBase request = context.Request;//定义传统request对象

               // MutualClass_PubParams pubParams = null;

                switch (method)
                {
                    case "get":
                        var queryString = filterContext.Request.RequestUri.AbsolutePath;
                        param = queryString != null && queryString.Length > 1 ? queryString.Remove(0, 1) : "";
                        break;
                    case "post":
                        byte[] byts = new byte[request.InputStream.Length];
                        request.InputStream.Read(byts, 0, byts.Length);

                        param = System.Text.Encoding.Default.GetString(byts);
                        dynamic dyn = JsonConvert.DeserializeObject<dynamic>(param);
                        string pubParamsStr = Convert.ToString(dyn.PubParams);

                        if (!string.IsNullOrEmpty(pubParamsStr))
                        {
                            //pubParams = JsonConvert.DeserializeObject<MutualClass_PubParams>(pubParamsStr);
                        }

                        break;
                    case "put":
                    case "delete":
                    case "connect":
                    case "patch":
                        break;
                    default:
                        break;
                }

                var url = filterContext.Request.RequestUri.AbsolutePath.Remove(0, 1);

             

            }
            catch (Exception ex)
            {
                _Logger.Error("OperateLogAttribute", ex);
            }
            
            base.OnActionExecuted(filterContext);
            
        }



    }


}