using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Utility
{
    public class SystemConfig
    {
        public SystemConfig()
        {

        }

        public static readonly string CookiesKey = "Sample_UserInfo";

        public static readonly string Channel = "Sample_Admin";

        /// <summary>
        /// 资源站点地址
        /// </summary>
        public static readonly string ResourceUri = ConfigurationManager.AppSettings["ResourceUri"];
        /// <summary>
        /// 接口地址
        /// </summary>
        public static readonly string AgentInterfaceUri = ConfigurationManager.AppSettings["AgentInterfaceUri"];


        public static readonly string LocalUri = ConfigurationManager.AppSettings["LocalUri"];


        public static readonly string WebUri = ConfigurationManager.AppSettings["WebUri"];


        public static readonly string FilesUri = ConfigurationManager.AppSettings["FilesUri"];


        public static readonly string ConfigSiteUri = ConfigurationManager.AppSettings["ConfigSiteUri"];


        public static readonly string PushUri = ConfigurationManager.AppSettings["PushUri"];

        /// <summary>
        /// 获取域名主体
        /// </summary>
        /// <returns></returns>
        public static string GetDomain()
        {
            var domain = ".Sample.com";
            try
            {
                var webUri = LocalUri;
                var urls = webUri.Split('.');
                domain = string.Format(".{0}.{1}", urls[1], urls[2]);
            }
            catch (Exception ex)
            {
                domain = ".Sample.com";
            }
            return domain;
        }
    }
}