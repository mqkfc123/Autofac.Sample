using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using Autofac;
using Dragon.Infrastructure.Redis;
using Dragon.Core;
using Sample.Management.Web.Services;

namespace Sample.Management.Web
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AdminAttribute : ActionFilterAttribute
    {
         
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public AdminAttribute()
        { 
        }
        public AdminAttribute(IRedisFactory redisFactory)
        {
            _connectionMultiplexer = redisFactory.CreateConnection();
        }
         
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            // 判断账户是否已经登入
            var _sessionId = filterContext.RequestContext.HttpContext.Session.SessionID;
            var _userService = CoreBuilderWork.LifetimeScope.Resolve<UserService>();

            if (!_userService.ExistsSignIn(_sessionId))
            {
                //filterContext.RequestContext.HttpContext.Response.Redirect("/Account/SignIn");
                filterContext.Result = new RedirectResult("/Account/SignIn");
            }
            else
            {
                var user = _userService.GetUserInfo();
                _userService.RepeatSignIn(user, _sessionId);
            }
            base.OnActionExecuting(filterContext); 
        }

    }

}