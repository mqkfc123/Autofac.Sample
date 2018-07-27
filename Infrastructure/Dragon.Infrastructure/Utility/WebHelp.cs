using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Auth.Infrastructure.Utility
{

    public class WebHelp
    {

        #region 提交方式
        /// <summary>
        /// 发起通信请求(GET方式)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetMode(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }

        /// <summary>
        /// 发起通信请求(Post方式)
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string PostMode(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            { 
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }



        /// <summary>
        /// 发起通信请求(Post方式)
        /// </summary>
        /// <param name="posturl"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string PostModeGBK(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.GetEncoding("GBK");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.KeepAlive = true;
                request.Method = "POST"; 
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="Url">URL路径</param>
        /// <param name="PostType">POST类型 GET/POST</param>
        /// <param name="codes">编码GB2312 / utf-8</param>
        /// <returns></returns>
        public string GetHttp(string Url, string PostType, string codes)
        {
            string StrHttp = "";
            try
            {
                if (PostType == "POST")
                {
                    string[] TmpUrl = Url.Split('?');
                    string PostStr = TmpUrl[TmpUrl.Length - 1];
                    byte[] requestBytes = Encoding.Default.GetBytes(PostStr);
                    var httpReq = (HttpWebRequest)WebRequest.Create(TmpUrl[0]);
                    httpReq.Timeout = 10000; //设置超时值2秒
                    httpReq.Method = "POST";
                    httpReq.ContentType = "application/x-www-form-urlencoded";
                    httpReq.ContentLength = requestBytes.Length;
                    Stream requestStream = httpReq.GetRequestStream();
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                    requestStream.Close();

                    var res = (HttpWebResponse)httpReq.GetResponse();
                    var sr = new StreamReader(res.GetResponseStream(), Encoding.GetEncoding(codes));
                    StrHttp = sr.ReadToEnd();
                    sr.Close();
                    res.Close();
                    sr = null;
                    res = null;
                }
                else
                {
                    var httpReq = (HttpWebRequest)WebRequest.Create(Url);
                    //HttpWebRequest 类对 WebRequest 中定义的属性和方法提供支持'，也对使用户能够直接与使用 HTTP 的服务器交互的附加属性和方法提供支持。
                    httpReq.ContentType = "application/x-www-form-urlencoded";
                    //httpReq.Headers.Add("Accept-Language", "zh-cn");
                    httpReq.Timeout = 10000; //设置超时值2秒
                    httpReq.Method = "GET";
                    var httpResq = (HttpWebResponse)httpReq.GetResponse();
                    var reader = new StreamReader(httpResq.GetResponseStream(), Encoding.GetEncoding(codes));
                    //如是中文，要设置编码格式为“GB2312”。
                    string respHTML = reader.ReadToEnd(); //respHTML就是页面源代码
                    StrHttp = respHTML;
                    httpResq.Close();
                    httpResq = null;
                }
            }
            catch (Exception ex)
            {
                SysLog("过程GetHttp出错", ex, "");
                StrHttp = "0";
            }
            return StrHttp;
        }

        
        /// <summary>
        ///     错误日志写入
        /// </summary>
        /// <param name="ex">错误串</param>
        public static void SysLog(string log, string logStr)
        {
            StreamWriter writeAdapter;
            Stream fileStream = GetLogFileStream();
            writeAdapter = new StreamWriter(fileStream, Encoding.Default);
            writeAdapter.WriteLine("***********" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "************");
            writeAdapter.WriteLine("日志备注::::" + log);
            writeAdapter.WriteLine("错误报告::::" + logStr);
            writeAdapter.WriteLine("***********End******************************************");
            writeAdapter.WriteLine("");
            writeAdapter.Close();
            fileStream.Close();
        }

        /// <summary>
        /// 获取log文件流
        /// </summary>
        /// <param name="strmenu">log类型</param>
        /// <returns></returns>
        private static Stream GetLogFileStream(string strmenu = "")
        {
            string logfile = HttpContext.Current.Server.MapPath("~/App_Data/log/") + DateTime.Now.ToString("yyyyMM");
            string logurl = string.Empty;
            if (string.IsNullOrEmpty(strmenu))
            {
                logurl = HttpContext.Current.Server.MapPath("~/App_Data/log/") + DateTime.Now.ToString("yyyyMM") + "/" +
                         DateTime.Now.ToString("yyyyMMdd") + ".log";
            }
            else
            {
                logurl = HttpContext.Current.Server.MapPath("~/App_Data/log/") + DateTime.Now.ToString("yyyyMM") + "/" +
                         DateTime.Now.ToString("yyyyMMdd") + "_" + strmenu + ".log";
            }
            var dir = new DirectoryInfo(logfile);
            if (!dir.Exists)
                dir.Create();
            Stream fileStream = null;
            fileStream = File.Open(logurl, FileMode.Append, FileAccess.Write, FileShare.Write);
            return fileStream;
        }

        /// <summary>
        ///     错误日志写入
        /// </summary>
        /// <param name="ex">错误串</param>
        public static void SysLog(string log, Exception ex, string strmenu)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Url != null && HttpContext.Current.Request.Url.AbsoluteUri != null)
            {
                string url = string.Empty;
                url = HttpContext.Current.Request.Url.AbsoluteUri;
                url = HttpContext.Current.Server.UrlDecode(url);
                Stream fileStream = GetLogFileStream(strmenu);
                var writeAdapter = new StreamWriter(fileStream, Encoding.Default);
                writeAdapter.WriteLine("***********" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "************");
                writeAdapter.WriteLine("日志备注::::" + log);
                writeAdapter.WriteLine("错误报告::::" + ex.Message);
                writeAdapter.WriteLine("路径:::" + url);
                writeAdapter.WriteLine("Source:::::" + ex.Source);
                writeAdapter.WriteLine("TargetSite:::::" + ex.TargetSite);
                writeAdapter.WriteLine("***********End******************************************");
                writeAdapter.WriteLine("");
                writeAdapter.Close();
                fileStream.Close();
            }

        }
    }
}
