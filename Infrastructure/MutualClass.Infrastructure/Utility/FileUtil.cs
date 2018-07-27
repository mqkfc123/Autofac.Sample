using System.IO;
using System.Text;

namespace Auth.Infrastructure.Utility
{
    public static class FileUtil
    {
        /// 读取文件
        /// </summary>
        /// <param name="relativePath">相对应用程序路径</param>
        /// <param name="encode">编码格式</param>
        /// <returns></returns>
        public static string ReadFileText(string relativePath, Encoding encode)
        {
            var path = DirectoryUtil.GetApplicationRootPath() + relativePath;
            var key = $"RainHyacinth_Utils_ReadFileText_{path}";
            var content = CacheUtil.CacheDependency.GetCacheOjbect<string>(key);
            if (!string.IsNullOrEmpty(content)) return content;
            content = File.ReadAllText(path, encode);
            CacheUtil.CacheDependency.Add(key, path, content);
            return content;
        }
        /// <summary>
        /// 根据相对路径读取文件内容，默认UTF-8
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string ReadFileText(string relativePath)
        {
            return ReadFileText(relativePath, Encoding.UTF8);
        }

        /// <summary>
        /// 根据完整文件路径获取FileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStream GetFileStream(string fileName)
        {
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open);
            }
            return fileStream;
        }
    }
}
