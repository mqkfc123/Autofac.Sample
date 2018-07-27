using System;
using System.IO;

namespace Auth.Infrastructure.Utility
{
    /// <summary>
    /// 路径帮助类
    /// </summary>
    public sealed class DirectoryUtil
    {
        /// <summary>
        /// 获取应用程序根路径
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationRootPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //web应用与console应用取到的结果不同，需要做下处理
            int binPos = baseDirectory.IndexOf("bin", StringComparison.OrdinalIgnoreCase); //找不到-1
            if (binPos < 0)
                return baseDirectory;
            return baseDirectory.Substring(0, binPos);
        }
        /// <summary>
        /// 是否存在路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static bool IsExistPath(string relativePath)
        {
            return Directory.Exists(GetApplicationRootPath() + relativePath);
        }
    }
}
