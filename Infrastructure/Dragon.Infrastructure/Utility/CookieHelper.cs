using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Auth.Infrastructure.Utility
{
    public class CookieHelper
    {
        #region Set Cookie

        /// <summary>
        /// 添加一个永久Cookie
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        public static void SetForeverCookie(string cookiename, string cookievalue, string cookiesDomain = null, string path = null)
        {
            SetCookie(cookiename, cookievalue, DateTime.MaxValue, cookiesDomain, path);
        }

        /// <summary>
        /// 添加一个会话Cookie
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        /// <param name="cookiesDomain"></param>
        public static void SetSessionCookie(string cookiename, string cookievalue, string cookiesDomain = null, string path = null)
        {
            SetCookie(cookiename, cookievalue, null, cookiesDomain, path);
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookiename">cookie名</param>
        /// <param name="cookievalue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        public static void SetCookie(string cookiename, string cookievalue, DateTime expires)
        {
            SetCookie(cookiename, cookievalue, expires, null, null);
        }

        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="cookiename"></param>
        /// <param name="cookievalue"></param>
        /// <param name="cookiesDomain"></param>
        /// <param name="expires"></param>
        /// <param name="path"></param>
        /// <remarks>
        /// modify by:majian
        /// 如果需要设置的cookie的信息中，值一致则不重新写入cookie（防止导致缓存失效）
        /// </remarks>
        public static void SetCookie(string cookiename, string cookievalue, DateTime? expires = null, string cookiesDomain = null, string path = null)
        {
            var context = HttpContext.Current;
            var request = context.Request;
            var response = context.Response;

            var currentCookie = request.Cookies.Get(cookiename);

            var cookie = new HttpCookie(cookiename)
            {
                Value = cookievalue
            };
            if (expires.HasValue)
            {
                cookie.Expires = expires.Value;
            }
            if (cookiesDomain != null)
            {
                cookie.Domain = cookiesDomain;
            }
            if (path != null)
            {
                cookie.Path = path;
            }
            cookie.Secure = false;

            //如果需要设置的cookie的信息中，值一致则不重新写入cookie
            if (currentCookie != null && currentCookie.Value == cookie.Value && currentCookie.Secure == cookie.Secure)
                return;
            response.Cookies.Add(cookie);
        }

        #endregion Set Cookie

        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        public static void ClearCookie(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 获取指定Cookie值
        /// </summary>
        /// <param name="cookiename">cookiename</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookiename)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookiename];
            string str = string.Empty;
            if (cookie != null)
            {
                str = cookie.Value;
            }
            return str;
        }
    }
}
