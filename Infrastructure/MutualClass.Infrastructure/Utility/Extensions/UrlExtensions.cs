using Auth.Infrastructure.FileSystems;
using System;
using System.Web.Mvc;

namespace Auth.Infrastructure.Utility.Extensions
{
    public static class UrlExtensions
    {
        public static string ImageContent(this UrlHelper url, string contentPath)
        {
            return Content<IUploadImageFolder>(url, contentPath);
        }

        public static string FileContent(this UrlHelper url, string contentPath)
        {
            return Content<IUploadFileFolder>(url, contentPath);
        }

        private static string Content<T>(this UrlHelper url, string contentPath) where T : IUploadFolder
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));
            if (string.IsNullOrWhiteSpace(contentPath))
                throw new ArgumentNullException(nameof(contentPath));

            //var folder = url.RequestContext.GetWorkContext().Resolve<T>();
            //return !folder.CheckPath(ref contentPath) ? contentPath : folder.GetUrl(contentPath);

            return contentPath;
        }
    }
}