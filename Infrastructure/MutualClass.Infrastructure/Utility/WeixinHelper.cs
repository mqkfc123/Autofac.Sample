using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Configuration;
using Dragon.Core.Log4net;

namespace Auth.Infrastructure.Utility
{

    public class ImgResult
    {
        public string Code { get; set; }
        public string Uri { get; set; }
        public string Massage { get; set; }

    }


    public class WeixinHelper
    {

        private static readonly ILog _Logger = LogHelper.GetLogger(typeof(WeixinHelper));

        public static readonly string FilesUri = CommFunction.StringParse(ConfigurationManager.AppSettings["FilesUri"]);

        public static readonly string ConfigSiteUri = CommFunction.StringParse(ConfigurationManager.AppSettings["ConfigSiteUri"]);

        public static string GetAccessToken()
        {
            return WebHelp.GetMode(ConfigSiteUri + "/Token/Index?k=kFHhsasd77askdVJ28GNAS88ASN5jGJAISJDjsdj8sdf46");
        }


        /// <summary>
        /// 生成二维码
        /// 获取请求创建二维码结果
        /// </summary>
        /// <returns>返回图片地址</returns>
        public static string CreateEwm(int scene_id, string fileType)
        {
            var AppToken = WebHelp.GetMode(ConfigSiteUri + "/Token/Index?k=kFHhsasd77askdVJ28GNAS88ASN5jGJAISJDjsdj8sdf46");

            var time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //_Logger.Error("AppToken：" + AppToken, new Exception());

            string strTiket = WebHelp.PostMode("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + AppToken, "{\"action_name\":\"QR_LIMIT_SCENE\",\"action_info\":{\"scene\":{\"scene_id\":" + scene_id + "}}}");
            dynamic objTiket = JsonConvert.DeserializeObject(strTiket);
            string tiket = objTiket.ticket.ToString();
            var createEwmResult = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + HttpUtility.UrlEncode(tiket);
            var imgArray = GetImgByte(createEwmResult); // 获取二维码 
            string files = Convert.ToBase64String(imgArray);

            WebClient clientObj = new WebClient();
            NameValueCollection PostVars = new NameValueCollection();

            //var fileName = scene_id;
            //这些主要是提交的参数和值
            PostVars.Add("File", fileType);
            PostVars.Add("FileName", scene_id.ToString());
            PostVars.Add("FileByte", files);
            //Post访问接口,返回转为byte[]的josn字符串
            byte[] byRemoteInfo = clientObj.UploadValues(FilesUri + "/Upload/FileUploadEwm.aspx", "POST", PostVars);
            string resultstring = Encoding.Default.GetString(byRemoteInfo);

            var objAll = JsonConvert.DeserializeObject<ImgResult>(resultstring);
            string code = objAll.Code;        //返回代码 10000成功，其他失败
            if (code == "10000")
            {
                return objAll.Uri;
            }
            else
            {
                return "";
            }
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

    }

    public class SHA1Util
    {
        public static String getSha1(String str)
        {
            //建立SHA1对象
            SHA1 sha = new SHA1CryptoServiceProvider();
            //将mystr转换成byte[] 
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(str);
            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);
            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");
            return hash;
        }
    }

    public class MD5Util
    {
        public MD5Util()
        {  }

        /** 获取大写的MD5签名结果 */
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }
    }

    /// <summary>
    /// TenpayUtil 的摘要说明。
    /// 配置文件
    /// </summary>
    public class TenpayUtil
    {
        public string appsecret;
        public string partner;                   //商户号        
        public string key;  //密钥
        public string appid;//appid
        public string appkey;//paysignkey(非appkey) 
        public string tenpay_notify; //支付完成后的回调处理页面,*替换成notify_url.asp所在路径
        public string tenpay = "1";

        public TenpayUtil()
        { }

        public static string getNoncestr()
        {
            Random random = new Random();
            return MD5Util.GetMD5(random.Next(1000).ToString(), "GBK");
        }


        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /** 对字符串进行URL编码 */
        public static string UrlEncode(string instr, string charset)
        {
            //return instr;
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;
                try
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding(charset));
                }
                catch (Exception ex)
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding("GB2312"));
                }
                return res;
            }
        }

        /** 对字符串进行URL解码 */
        public static string UrlDecode(string instr, string charset)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;
                try
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding(charset));
                }
                catch (Exception ex)
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding("GB2312"));
                }
                return res;
            }
        }


        /** 取时间戳生成随即数,替换交易单号中的后10位流水号 */
        public static UInt32 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }
        /** 取随机数 */
        public static string BuildRandomStr(int length)
        {
            Random rand = new Random();
            int num = rand.Next();
            string str = num.ToString();
            if (str.Length > length)
            {
                str = str.Substring(0, length);
            }
            else if (str.Length < length)
            {
                int n = length - str.Length;
                while (n > 0)
                {
                    str.Insert(0, "0");
                    n--;
                }
            }
            return str;
        }

    }
     
    /// <summary>
    ///WxShare 的摘要说明
    /// </summary>
    public class WeixinShare
    {
        public WeixinShare()
        { }

        /// <summary>
        /// 获取分享需要的参数
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="appId"></param>
        /// <param name="timeStamp"></param>
        /// <param name="nonceStr"></param>
        /// <param name="signature"></param>
        public static void GetShareParam(string ticket, ref string timeStamp, ref string nonceStr, ref string signature)
        {

            timeStamp = TenpayUtil.getTimestamp();
            nonceStr = TenpayUtil.getNoncestr();

            StringBuilder sb = new StringBuilder();
            sb.Append("jsapi_ticket=" + ticket);
            sb.Append("&noncestr=" + nonceStr);
            sb.Append("&timestamp=" + timeStamp);
            sb.Append("&url=" + DealUrl(HttpContext.Current.Request.Url));
            signature = SHA1Util.getSha1(sb.ToString()).ToLower();
        }

        /// <summary>
        /// 去除端口号
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string DealUrl(Uri url)
        {
            string urlStr = "";
            urlStr = url.ToString().Replace(":" + url.Port, "");
            return urlStr.Split('#')[0];
        }
    }

}
