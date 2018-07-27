using Autofac;
using Dragon.Core;
using Sample.OAuth.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Sample.OAuth
{

    public class OAuthAttribute : AuthorizationFilterAttribute
    {
        #region Overrides of AuthorizationFilterAttribute

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (actionContext.Request.RequestUri.Host == "localhost")
                return;

            //var work = actionContext.ControllerContext.GetWorkContext();
            //var tokenService = work.Resolve<ITokenService>();
            var tokenService = CoreBuilderWork.LifetimeScope.Resolve<ITokenService>();

            foreach (var token in GetTokens(actionContext).Where(i => !string.IsNullOrEmpty(i)))
            {
                if (await tokenService.CheckToken(token))
                    return;
            }

            actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "非法请求！");
        }

        #endregion Overrides of AuthorizationFilterAttribute

        #region Private Method

        private static IEnumerable<string> GetTokens(HttpActionContext actionContext)
        {
            yield return GetTokenByRoute(actionContext);
        }

        private static string GetTokenByRoute(HttpActionContext actionContext)
        {
            object token;
            return !actionContext.RequestContext.RouteData.Values.TryGetValue("accessToken", out token) ? null : token?.ToString();
        }

        #endregion Private Method
    }
}