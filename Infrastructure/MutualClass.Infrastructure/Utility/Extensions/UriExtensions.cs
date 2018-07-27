using System;
using System.Text;
using System.Web;

namespace Auth.Infrastructure.Utility.Extensions
{
    public static class UriExtensions
    {
        public static string GetUrlPrefix(this Uri uri)
        {
            var builder = new StringBuilder();
            builder.Append(uri.Scheme);
            builder.Append("://");
            builder.Append(uri.Host);
            if (!uri.IsDefaultPort)
            {
                builder.Append(":");
                builder.Append(uri.Port);
            }

            return builder.ToString();
        }

        public static string GetUrlPrefix(this HttpRequest request, bool isAppentApplicationPath = false)
        {
            var prefix = GetUrlPrefix(request.Url);
            if (isAppentApplicationPath)
            {
                prefix += request.ApplicationPath;
            }
            return prefix;
        }

        public static string GetUrlPrefix(this HttpRequestBase request, bool isAppentApplicationPath = false)
        {
            var prefix = GetUrlPrefix(request.Url);
            if (isAppentApplicationPath)
            {
                prefix += request.ApplicationPath;
            }
            return prefix;
        }
    }
}