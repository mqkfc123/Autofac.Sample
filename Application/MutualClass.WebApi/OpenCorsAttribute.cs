using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Script.Serialization;

namespace MutualClass.WebApi
{
    /// <summary>
    /// 开放cors协议，支持跨域访问
    /// </summary> 
   // [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class OpenCorsAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 开放cors协议，支持跨域访问的初始化
        /// </summary>
        public OpenCorsAttribute()
        {
        }

        /// <summary>
        ///在action渲染之前
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
             

            var response = filterContext.Request.CreateResponse(HttpStatusCode.OK);

            //if (filterContext.Request.Method == HttpMethod.Options)
            //{
            //    filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Accepted);
            //    return;
            //}

            //response.Headers.Add("X-RESULT-COUNT", unreadCount.ToString());
            //response.Headers.Add("Access-Control-Allow-Origin", "*");
            //var responseHead = filterContext.RequestContext.HttpContext.Response.Headers;
            //if (responseHead != null)
            //{
            //    responseHead.Add("Access-Control-Allow-Origin", "*");
            //}
            base.OnActionExecuting(filterContext);
           
        }

        
    }
}