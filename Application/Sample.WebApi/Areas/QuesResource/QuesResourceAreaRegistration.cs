using System.Web.Http;
using System.Web.Mvc;

namespace MutualClass.WebApi.Areas.Admin
{
    /// <summary>
    /// 
    /// </summary>
    public class QuesResourceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QuesResource";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            
            context.Routes.MapHttpRoute(
               name: "QuesResource",
               routeTemplate: "api/QuesResource/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
             );

            //context.MapRoute(
            //    "Admin_default",
            //    "Admin/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

        }
    }
}