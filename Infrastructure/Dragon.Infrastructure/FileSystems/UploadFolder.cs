using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Auth.Infrastructure.Utility.Extensions;
using System.Net;
using System.Collections.Specialized;
using System.Text;
using System.Configuration;
using Auth.Infrastructure.Utility;

namespace Auth.Infrastructure.FileSystems
{
    public interface IUploadFolder
    {
        /// <summary>
        /// 判断一个虚拟路径的文件是否存在。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        /// <returns>虚拟路径。</returns>
        bool FileExists(string virtualPath);

        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        void DeleteFile(string virtualPath);

        /// <summary>
        /// 判断目录是否存在。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        /// <returns>如果存在则返回true，否则返回false。</returns>
        bool DirectoryExists(string virtualPath);

        /// <summary>
        /// 创建一个目录。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        void CreateDirectory(string virtualPath);

        /// <summary>
        /// 删除目录。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        void DeleteDirectory(string virtualPath);

        /// <summary>
        /// 获取指定路径下的所有文件。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        /// <param name="fileExtensions">文件扩展名。</param>
        /// <param name="skip">跳过数量。</param>
        /// <param name="take">取出数量。</param>
        /// <param name="includeChildren">是否包含子目录文件。</param>
        /// <returns>文件路径集合。</returns>
        IEnumerable<string> ListFiles(string virtualPath, string[] fileExtensions = null, int skip = 0, int take = 1000, bool includeChildren = false);

        /// <summary>
        /// 获取虚拟路径。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>虚拟路径。</returns>
        string GetVirtualPath(string path);

        /// <summary>
        /// 创建文件。
        /// </summary>
        /// <param name="path">文件路径。</param>
        /// <param name="stream">文件流。</param>
        void CreateFile(string path, Stream stream);

        /// <summary>
        /// 创建文件。
        /// </summary>
        /// <param name="path">文件路径。</param>
        /// <param name="stream">文件流。</param>
        string CreateFileUrl(string path, Stream stream);

        /// <summary>
        /// 根据虚拟路径获取文件的Url地址。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        /// <returns>Uri地址。</returns>
        string GetUrl(string virtualPath);

        /// <summary>
        /// 验证路径是否合法，在可能的情况下修正路径。
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        bool CheckPath(ref string virtualPath);
    }

    public interface IUploadImageFolder : IUploadFolder
    {
    }

    public interface IUploadFileFolder : IUploadFolder
    {
    }

    public class UploadFolder : IUploadFolder
    {
        private readonly int _channelId;
        private static HttpClient _client;
        protected virtual string RootPath { get; } = "uploads";
        private readonly string _baseUrl;

      


        public UploadFolder(ILifetimeScope lifetimeScope)
        {
            //
            //var shellSettings = lifetimeScope.Resolve<ShellSettings>();
            _baseUrl = "FilesUri";
            //_client = new HttpClient
            //{
            //    BaseAddress = new Uri(_baseUrl)
            //};
            //_channelId = lifetimeScope.Resolve<ShellSettings>().GetChannelId();
        }

        #region Implementation of IUploadFolder

        /// <summary>
        /// 判断一个虚拟路径的文件是否存在。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        /// <returns>虚拟路径。</returns>
        public bool FileExists(string virtualPath)
        {
            virtualPath = FixPath(virtualPath);
            var result = _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Head,
                RequestUri = new Uri(_baseUrl + virtualPath)
            }).Result;
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        public void DeleteFile(string virtualPath)
        {
            virtualPath = FixPath(virtualPath);
            _client.DeleteAsync(virtualPath).Wait();
        }

        /// <summary>
        /// 判断目录是否存在。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        /// <returns>如果存在则返回true，否则返回false。</returns>
        public bool DirectoryExists(string virtualPath)
        {
            virtualPath = FixPath(virtualPath);
            var result = _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Head,
                RequestUri = new Uri(_baseUrl + virtualPath)
            }).Result;
            return result.IsSuccessStatusCode;
        }

        /// <summary>
        /// 创建一个目录。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        public void CreateDirectory(string virtualPath)
        {
            virtualPath = FixPath(virtualPath);
            _client.PostAsync(virtualPath, new StringContent(string.Empty)).Wait();
        }

        /// <summary>
        /// 删除目录。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        public void DeleteDirectory(string virtualPath)
        {
            virtualPath = FixPath(virtualPath);
            _client.DeleteAsync(virtualPath).Wait();
        }

        /// <summary>
        /// 获取指定路径下的所有文件。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        /// <param name="fileExtensions">文件扩展名。</param>
        /// <param name="skip">跳过数量。</param>
        /// <param name="take">取出数量。</param>
        /// <param name="includeChildren">是否包含子目录文件。</param>
        /// <returns>文件路径集合。</returns>
        public IEnumerable<string> ListFiles(string virtualPath, string[] fileExtensions = null, int skip = 0, int take = 1000,
            bool includeChildren = false)
        {
            virtualPath = FixPath(virtualPath);
            var extensions = fileExtensions == null ? string.Empty : string.Join(",", fileExtensions);
            var content = _client.GetStringAsync(virtualPath + $"?searchPattern={extensions}&skip={skip}&take={take}&includeChildren={includeChildren}").Result;
            return JsonConvert.DeserializeObject<string[]>(content);
        }

        /// <summary>
        /// 获取虚拟路径。
        /// </summary>
        /// <param name="path">路径。</param>
        /// <returns>虚拟路径。</returns>
        public string GetVirtualPath(string path)
        {
            var rootPath = _channelId + "/" + RootPath;
            //  ~/{RootPath}/xxx to ~/{RootPath}
            if (path.StartsWith("~/" + rootPath, StringComparison.OrdinalIgnoreCase))
                return path;
            //  /{RootPath}/xxx to ~/{RootPath}
            if (path.StartsWith("/" + rootPath, StringComparison.OrdinalIgnoreCase))
                return path.Insert(0, "~");
            //  {RootPath}/xxx to ~/{RootPath}
            if (path.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
                return path.Insert(0, "~/");

            // xxx、~/xxx、/xxx to ~/{RootPath}/xxx
            if (path.StartsWith("~/"))
                path = path.Remove(0, 2);
            else if (path.StartsWith("/"))
                path = path.Remove(0, 1);
            return path.Insert(0, "~/" + rootPath + "/");
        }

        private string FixPath(string path)
        {
            path = GetVirtualPath(path);
            if (path.StartsWith("~/"))
                path = path.Remove(0, 1);

            if (!path.StartsWith("/"))
                path = path.Insert(0, "/");
            return path.ToLower();
        }

        /// <summary>
        /// 创建文件。
        /// </summary>
        /// <param name="path">文件路径。</param>
        /// <param name="stream">文件流。</param>
        public void CreateFile(string path, Stream stream)
        {
            path = FixPath(path);
            var bytes = stream.ReadAllBytes(true);
            var content = new MultipartFormDataContent
            {
                {new ByteArrayContent(bytes), "file", Path.GetFileName(path)}
            };
            Task.Run(async () =>
            {
                var result = await _client.PutAsync(path, content);
                result.EnsureSuccessStatusCode();
            }).Wait();

        }

        /// <summary>
        /// 创建文件。
        /// </summary>
        /// <param name="path">文件路径。</param>
        /// <param name="stream">文件流。</param>
        public string CreateFileUrl(string path, Stream stream)
        {
            path = FixPath(path);
            var bytes = stream.ReadAllBytes(true);
            string files = Convert.ToBase64String(bytes);        //文件字节流

            WebClient clientObj = new WebClient();
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("File", "ueditor");                                //这些主要是提交的参数和值
            PostVars.Add("FileName", Path.GetFileName(path));
            PostVars.Add("FileByte", files);

            //Post访问接口,返回转为byte[]的josn字符串
            var filesUri = CommFunction.StringParse(ConfigurationManager.AppSettings["FilesUri"]);
            byte[] byRemoteInfo = clientObj.UploadValues(filesUri + "/Upload/FileUpload.aspx", "POST", PostVars);
            string resultstring = Encoding.Default.GetString(byRemoteInfo);
            return resultstring;
        }


        /// <summary>
        /// 根据虚拟路径获取文件的Url地址。
        /// </summary>
        /// <param name="virtualPath">虚拟路径。</param>
        /// <returns>Uri地址。</returns>
        public string GetUrl(string virtualPath)
        {
            if (string.IsNullOrWhiteSpace(virtualPath))
                return string.Empty;
            if (virtualPath.StartsWith("http://"))
                return virtualPath;
            virtualPath = GetVirtualPath(virtualPath);
            if (virtualPath.StartsWith("~/"))
                virtualPath = virtualPath.Remove(0, 2);
            if (virtualPath.StartsWith("/"))
                virtualPath = virtualPath.Remove(0, 1);

            //default url
            //            return $"{_baseUrl}{virtualPath}";
            //jkzl url
            return $"http://f1.yihuimg.com/TFS/upfile/realfile/{virtualPath.ToLower()}";
        }

        /// <summary>
        /// 验证路径是否合法，在可能的情况下修正路径。
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public bool CheckPath(ref string virtualPath)
        {
            if (virtualPath == null)
            {
                return false;
            }

            var rootPathKeyword = "/" + RootPath + "/";
            var rootPathIndex = virtualPath.IndexOf(rootPathKeyword, StringComparison.InvariantCultureIgnoreCase);
            if (rootPathIndex > -1)
            {
                virtualPath = virtualPath.Substring(rootPathIndex + rootPathKeyword.Length);
            }

            if (Regex.IsMatch(virtualPath, @"^\w+:"))
            {
                return false;
            }

            virtualPath = GetVirtualPath(virtualPath);
            return true;
        }

        #endregion Implementation of IUploadFolder
    }

    public class UploadImageFolder : UploadFolder, IUploadImageFolder
    {
        public UploadImageFolder(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {
        }

        #region Overrides of UploadFolder

        protected override string RootPath { get; } = "images";

        #endregion Overrides of UploadFolder
    }

    public class UploadFileFolder : UploadFolder, IUploadFileFolder
    {
        public UploadFileFolder(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {
        }

        #region Overrides of UploadFolder

        protected override string RootPath { get; } = "files";

        #endregion Overrides of UploadFolder
    }
}
