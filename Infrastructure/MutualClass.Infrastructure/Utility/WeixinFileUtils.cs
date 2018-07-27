using Dragon.Core.Log4net; 
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Utility
{

    /// <summary>
    /// 将微信上的文件转存到我方服务器上
    /// </summary>
    public class WeixinFileUtils
    {

        private static readonly ILog _Logger = LogHelper.GetLogger(typeof(WeixinFileUtils));

        public WeixinFileUtils()
        {  }

        /// <summary>
        /// 获取微信方文件转换为我方图片Url
        /// </summary>
        /// <param name="weixinFileId">微信文件ID</param>
        /// <returns>成功=返回我方文件url</returns>
        public static WeixinFileInfo ConvertToLocalUrl(string weixinFileId, string token)
        {
            WeixinFileInfo result = new WeixinFileInfo { Code = -14444, Message = "未知异常" };

            try
            {
                string fileId = weixinFileId;
                Exception ex = new Exception();
                bool _iswater = false; //默认不打水印
                bool _isthumbnail = false; //默认不生成缩略图
                string imgUrl = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + token + "&media_id=" + fileId;
                //LogHelp.SysLog(imgUrl,ex ,"微信上传图片功能");
                string api = "weixin";
                result = fileSaveAs(imgUrl, _isthumbnail, _iswater, false, false, true, api);
                result.WeixinId = fileId;
                return result;
            }
            catch (Exception ex)
            {
                _Logger.Error("ConvertToLocalUrl:"+ ex.ToString(), ex);
                result.Code = -10000;
                result.Message = ex.ToString();
                result.Data = "";
                //return "{\"msg\": 0, \"msgbox\": \"上传过程中发生意外错误！\"}";
            }
            return result;

        }

        /// <summary>
        /// 文件上传方法C
        /// </summary>
        /// <param name="postedFile">文件流</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否打水印</param>
        /// <param name="isReOriginal">是否返回文件原名称</param>
        /// <param name="_isMuint"></param>
        /// <param name="myfilename">定义好的文件名</param>
        /// <returns>服务器文件路径</returns>
        private static WeixinFileInfo fileSaveAs(string imgUrl, bool isThumbnail, bool isWater, bool _isImage, bool _isReOriginal, bool _isMuint, string api)
        {
            WeixinFileInfo result = new WeixinFileInfo { Code = -14444, Message = "未知异常" };
            try
            {
                string guidName = Guid.NewGuid().ToString() + ".jpg";
                byte[] imgData = GetImgByte(imgUrl);
                string resultJosn = UpFileInterface(imgData, guidName, api);
                dynamic objAll = JsonConvert.DeserializeObject<dynamic>(resultJosn);
                string code = objAll.Code;        //返回代码 10000成功，其他失败
                if (code == "10000")
                {
                    result.Code = 10000;
                    result.ImageUrl = objAll.Uri;
                    result.FileSize = imgData.Length;
                    result.Message = "OK";
                    return result;
                }
                else
                {
                    //return "{\"msg\": 0, \"msgbox\": \"该图片含有无法解析的未知格式，请处理后重新上传!\"}";
                    result.Code = -10000;
                    result.Message = "获取图片失败。" + resultJosn;
                    result.Data = "";
                    return result;
                }
            }
            catch (Exception ex)
            {

                _Logger.Error("fileSaveAs" ,ex);
                result.Code = -10000;
                result.Message = ex.ToString();
                result.Data = "";
                //return "{\"msg\": 0, \"msgbox\": \"上传过程中发生意外错误！\"}";
            }
            return result;
        }

        private static byte[] GetImgByte(string url)
        {
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            HttpWebResponse myResponse = (System.Net.HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);

            var bytes = default(byte[]);
            using (var memstream = new MemoryStream())
            {
                var buffer = new byte[512];
                var bytesRead = default(int);
                while ((bytesRead = reader.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                    memstream.Write(buffer, 0, bytesRead);
                bytes = memstream.ToArray();
            }
            reader.Close();
            return bytes;
        }

        /// <summary>
        /// 图片上传接口 【调用接口保存图片方法】
        /// </summary>
        /// <param name="imgArray">图片字节流</param>
        /// <param name="imgName">图片名称</param>
        /// <returns></returns>
        private static string UpFileInterface(byte[] imgArray, string imgName, string api)
        {
            try
            {
                //这些主要是提交的参数和值
                //PostVars.Add("File", "zixun");
                //PostVars.Add("FileName", "aaaaaaaaa.jpg");
                //PostVars.Add("FileByte", files);

                //API版本号。版本号为整型，从数字1开始递增.
                string files = Convert.ToBase64String(imgArray);        //文件字节流
                //_Logger.Error("files" + files);

                WebClient clientObj = new WebClient();
                NameValueCollection PostVars = new NameValueCollection();
                PostVars.Add("File", api);
                PostVars.Add("FileName", imgName);
                PostVars.Add("FileByte", files);

                //Post访问接口,返回转为byte[]的josn字符串

                var filesUri = CommFunction.StringParse(ConfigurationManager.AppSettings["FilesUri"]); 
                byte[] byRemoteInfo = clientObj.UploadValues(filesUri + "/Upload/FileUpload.aspx", "POST", PostVars);
                string resultstring = Encoding.Default.GetString(byRemoteInfo);
                return resultstring;
            }
            catch (Exception ex)
            {
                _Logger.Error("UpFileInterface", ex);
                return ex.ToString();
            }
        }
    }

    [Serializable]
    public class WeixinFileInfo
    {
        public WeixinFileInfo()
        {
        }
        /// <summary>
        /// 返回状态码 （必须：10000正常 -14444 异常 404 没用找到 -1 连接服务器失败）
        /// </summary>
        public int Code;
        /// <summary>
        /// 返回信息 （必填）
        /// </summary>
        public string Message;
        /// <summary>
        /// 额外的数据 （可选）
        /// </summary>
        public string Data;
        /// <summary>
        /// 由微信生成的媒体文件id
        /// </summary>
        public string WeixinId;
        /// <summary>
        /// 我方图片服务器生成的url
        /// </summary>
        public string ImageUrl;
        /// <summary>
        /// 文件大小字节
        /// </summary>
        public int FileSize;
    }
}
