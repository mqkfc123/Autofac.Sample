using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dragon.Infrastructure.Utility
{

    public class Utils
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptMD5(string str)
        {
            byte[] result = System.Text.Encoding.UTF8.GetBytes(str);    //tbPass为输入密码的文本框
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "").ToLower();  //输出加密文本
        }

        /// <summary>
        ///  参数设定
        /// </summary>
        /// <param name="pars"></param>
        /// <returns></returns>
        public static string ParsToString(Hashtable pars)
        {
            if (pars == null) return "";
            var sb = new StringBuilder();
            foreach (string k in pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(pars[k].ToString()));
            }
            return sb.ToString();
        }

        private static char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static string GenerateRandomNumber(int Length)//调用时想生成几位就几位；Length等于多少就多少位。
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }



        #region 获取 微信签名

        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        ///  获取  微信 前面
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="timeStamp"></param>
        /// <param name="nonceStr"></param>
        /// <param name="signature"></param>

        public static void GetShareParam(string ticket, ref string timeStamp, ref string nonceStr, ref string signature)
        {
            timeStamp = getTimestamp();
            nonceStr = getNoncestr();
            StringBuilder sb = new StringBuilder();
            sb.Append("jsapi_ticket=" + ticket);
            sb.Append("&noncestr=" + nonceStr);
            sb.Append("&timestamp=" + timeStamp);
            sb.Append("&url=" + DealUrl(HttpContext.Current.Request.Url));
            signature = getSha1(sb.ToString()).ToLower();
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

        public static string getNoncestr()
        {
            Random random = new Random();
            return GetMD5(random.Next(1000).ToString(), "GBK");
        }

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



        #endregion


    }
}
