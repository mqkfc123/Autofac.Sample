using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sample.Management.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Sample_Account",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_Coupon",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Coupon", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_Ques",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Ques", action = "QuesIndex", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_ShopsTag",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "ShopsTag", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_Tag",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Tag", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_Shops",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Shops", action = "ShopsSet", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_JxShopsProduct",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "JxShopsProduct", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_JxShops",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "JxShops", action = "ShopsSet", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_Bonus",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Bonus", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_ApplyPrice",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "ApplyPrice", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_Money",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Money", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Sample_Advice",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Advice", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "Sample_Activity",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Activity", action = "Index", id = UrlParameter.Optional }
           );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
