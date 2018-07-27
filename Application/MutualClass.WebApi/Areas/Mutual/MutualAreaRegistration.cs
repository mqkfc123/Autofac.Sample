using System.Web.Http;
using System.Web.Mvc;

namespace MutualClass.WebApi.Areas.WeiXin
{
    /// <summary>
    /// 
    /// </summary>
    public class MutualAreaRegistration : AreaRegistration 
    {
        /// <summary>
        /// 分区名称
        /// </summary>
        public override string AreaName 
        {
            get 
            {
                return "Mutual";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.Routes.MapHttpRoute(
               name: "Mutual_Scene",
               routeTemplate: "api/Mutual/Scene/{controller}/{id}",
               defaults: new { id = RouteParameter.Optional }
            );

            //context.MapRoute(
            //    "WeiXin_default",
            //    "WeiXin/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }

    }
}