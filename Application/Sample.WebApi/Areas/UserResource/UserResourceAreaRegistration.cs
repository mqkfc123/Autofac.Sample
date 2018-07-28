using System.Web.Http;
using System.Web.Mvc;

namespace Sample.WebApi.Areas.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class UserResourceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UserResource";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
                      name: "UserResource",
                      routeTemplate: "api/UserResource/{controller}/{id}",
                      defaults: new { id = RouteParameter.Optional }
                  );

            //context.MapRoute(
            //    "Web_default",
            //    "Web/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}