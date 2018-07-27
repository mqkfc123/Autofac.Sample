using Auth.Infrastructure.Mvc;
using Auth.Infrastructure.Utility;
using Autofac;
using Newtonsoft.Json;
using Sample.Management.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sample.Management.Web.Models;
using System.Configuration;
using Dragon.Core;

namespace Sample.Management.Web.Controllers
{
    public class BaseController : Controller
    {
        public static readonly string JsVersion = "v=" + ConfigurationManager.AppSettings["JsVersion"];
        public static readonly string CssVersion = "v=" + ConfigurationManager.AppSettings["CssVersion"];
        public UserInfo User { get; private set; }

        DataResult FailData = new DataResult { Code = 0, Message = "系统繁忙，请稍后再试！" };

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.filterContext = filterContext;
            ViewBag.JsVersion = JsVersion;
            ViewBag.CssVersion = CssVersion;

            var _userService = CoreBuilderWork.LifetimeScope.Resolve<UserService>();
            User = _userService.GetUserInfo();

            base.OnActionExecuting(filterContext);
        }


        #region 访问 接口
        /// <summary>
        /// 访问 接口
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="parameters"></param>
        /// <param name="outDataResponse"></param>
        /// <param name="channelId">苹果：1000000，安卓：1000001，微信：1000060，QQ：1000061，QQ微博：1000062，新浪微博：1000063</param>
        /// <returns></returns>
        [NonAction]
        public bool InvokeAgentService(string apiName, Dictionary<string, object> parameters, out string outDataResponse, int channelId)
        {
            var _webApiAgentService = CoreBuilderWork.LifetimeScope.Resolve<WebApiAgentService>();
            return _webApiAgentService.InvokeAgentService(apiName, parameters, out outDataResponse, channelId);

        }
        #endregion
    }
}