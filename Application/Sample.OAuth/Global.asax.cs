using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Dragon.Core;

namespace Sample.OAuth
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var coreBuilder = new CoreBuilder();
            coreBuilder.Build();
        }
    }
}