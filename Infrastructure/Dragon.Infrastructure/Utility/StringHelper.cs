using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Auth.Infrastructure.Utility
{
    /// <summary>
    /// 字符串操作帮助类
    /// </summary>
    public class StringHelper
    {
        private const string regSafeSql = @"[^\S]{+}delete[^\S]{1}|[^\S]{+}drop[^\S]{1}|[^\S]{+}update[^\S]{1}|[^\S]{+}truncate[^\S]{1}|[^\S]{+}create[^\S]{1}|[^\S]{+}xp_cmdshell[^\S]{1}|[^\S]{+}insert[^\S]{1}|[^\S]{+}--[^\S]{1}";

        /// <summary>
        /// SQL 特殊字符过滤,防SQL注入
        /// </summary>
        /// <param name="Contents"></param>
        /// <returns></returns>
        public static string SqlFilter(string Contents)
        {
            if (Regex.IsMatch(Contents.ToLower(), regSafeSql, RegexOptions.IgnoreCase))
            {
                Contents = Regex.Replace(Contents.ToLower(), regSafeSql, " ", RegexOptions.IgnoreCase);
            }
            return Contents;
        }

        #region 验证

        /// <summary>
        /// 检查字符串是否能转为日期
        /// </summary>
        /// <param name="value">验证的字符串</param>
        /// <returns></returns>
        public static bool IsStringDate(string value)
        {
            DateTime time;

            if (DateTime.TryParse(value, out time))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///检查字符串是否是纯数字构成
        /// </summary>
        /// <param name="value">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^\d+$");
        }

        /// <summary>
        /// 检查字符串是否由字符和数字构成
        /// </summary>
        /// <param name="value">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsLetterOrNumber(string value)
        {
            return Regex.IsMatch(value, @"^[A-Za-z0-9]+$");
        }

        /// <summary>
        /// 检查字符串是否是数字，包含小数和整数
        /// </summary>
        /// <param name="value">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string value)
        {
            return Regex.IsMatch(value, @"^(0|([1-9]+[0-9]*))(.[0-9]+)?$");
        }

        /// <summary>
        /// 检查字符串是否是邮件
        /// </summary>
        /// <param name="value">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            return Regex.IsMatch(value, @"^\w+([-+.]\w+)*@(\w+([-.]\w+)*\.)+([a-zA-Z]+)+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 检查字符串是否为手机号码
        /// </summary>
        /// <param name="value">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsMobile(string value)
        {
            return Regex.IsMatch(value, @"^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|2|3|5|6|7|8|9])\d{8}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 检查字符串是否为电话号码
        /// </summary>
        /// <param name="value">需要验证的字符串</param>
        /// <returns></returns>
        public static bool IsTelephone(string value)
        {
            return Regex.IsMatch(value, @"^(86)?(-)?(0\d{2,3})?(-)?(\d{7,8})(-)?(\d{3,5})?$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 检查日期是否过期
        /// </summary>
        /// <param name="myDate">检查的日期</param>
        /// <returns>过期返回True，</returns>
        public static bool ValidDate(string myDate)
        {
            return CompareDate(myDate, DateTime.Now.ToShortDateString()) < 0;
        }

        #endregion 验证

        #region 字符串转换

        /// <summary>
        /// 把字符串转为日期
        /// </summary>
        /// <param name="value">转换字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime StrToDate(string value, DateTime defaultValue)
        {
            if (!IsStringDate(value))
            {
                return defaultValue;
            }
            else
            {
                return DateTime.Parse(value);
            }
        }

        /// <summary>
        /// 把字符串转为整型
        /// </summary>
        /// <param name="value">转换字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int StrToInt(string value, int defaultValue)
        {
            int returnValue;

            if (int.TryParse(value, out returnValue))
            {
                return returnValue;
            }

            return defaultValue;
        }

        /// <summary>
        /// 把字符串转为Double类型
        /// </summary>
        /// <param name="value">转换字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static double StrToDouble(string value, double defaultValue)
        {
            double returnValue;

            if (double.TryParse(value, out returnValue))
            {
                return returnValue;
            }

            return defaultValue;
        }

        /// <summary>
        /// 比较两个时间的大小
        /// </summary>
        /// <param name="firstDate">第一个日期</param>
        /// <param name="secondDate">第二个日期</param>
        /// <returns>返回：0-相等 1-大于 -1-小于</returns>
        public static int CompareDate(string firstDate, string secondDate)
        {
            DateTime dtime1;
            DateTime dtime2;

            if (DateTime.TryParse(firstDate, out dtime1))
            {
                throw new Exception(firstDate + "不是有效的DateTime");
            }

            if (DateTime.TryParse(secondDate, out dtime2))
            {
                throw new Exception(secondDate + "不是有效的DateTime");
            }

            TimeSpan ts = dtime1 - dtime2;

            return ts.TotalDays.CompareTo(0);
        }

        /// <summary>
        /// 字符串数组转换整型数组
        /// </summary>
        /// <param name="arr_str">字符串数组</param>
        /// <returns></returns>
        public static int[] StrToIntArray(string[] arr_str)
        {
            if (arr_str != null)
            {
                int[] arr_int = new int[arr_str.Length];
                for (int i = 0; i < arr_str.Length; i++)
                {
                    arr_int[i] = Convert.ToInt32(arr_str[i].ToString());
                }
                return arr_int;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 字符串数组转换Double数组
        /// </summary>
        /// <param name="arr_str">字符串数组</param>
        /// <returns></returns>
        public static double[] StrToDoubleArray(string[] arr_str)
        {
            if (arr_str != null)
            {
                double[] arr_double = new double[arr_str.Length];
                for (int i = 0; i < arr_str.Length; i++)
                {
                    arr_double[i] = Convert.ToDouble(arr_str[i].ToString());
                }
                return arr_double;
            }
            else
            {
                return null;
            }
        }

        #endregion 字符串转换

        #region html特殊字符互转

        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本。</param>
        /// <returns>替换完的文本。</returns>
        public static string HtmlEncode(string theString)
        {
            string String = "";
            if (!string.IsNullOrEmpty(theString))
            {
                theString = theString.Replace(">", "&gt;");
                theString = theString.Replace("<", "&lt;");
                theString = theString.Replace("  ", " &nbsp;");
                theString = theString.Replace("\"", "&quot;");
                theString = theString.Replace("'", "&#39;");
                theString = theString.Replace("\r\n", "<br/> ");
                String = theString;
            }
            else
            {
                String = theString;
            }
            return String;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本。</param>
        /// <returns>恢复好的文本。</returns>
        public static string HtmlDecode(string theString)
        {
            string String = "";
            if (!string.IsNullOrEmpty(theString))
            {
                theString = theString.Replace("&gt;", ">");
                theString = theString.Replace("&lt;", "<");
                theString = theString.Replace("&nbsp;", "  ");
                theString = theString.Replace("&amp;nbsp;", "  ");
                theString = theString.Replace("&quot;", "\"");
                theString = theString.Replace("&#39;", "'");
                theString = theString.Replace("<br/> ", "\r\n");
                String = theString;
            }
            else
            {
                String = theString;
            }
            return String;
        }

        #endregion html特殊字符互转

        #region 去除HTML标记

        ///<summary>
        ///去除HTML标记
        ///</summary>
        ///<param name="NoHTML">包括HTML的源码</param>
        ///<returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return string.Empty;
            }
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "“", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "”", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring = Htmlstring.Replace("<", "&lt;");
            Htmlstring = Htmlstring.Replace(">", "&gt;");
            return Htmlstring;
        }

        public static string RemoveHTML(string data, int length)
        {
            Regex htmlReg = new Regex(@"<[^>]+>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex htmlSpaceReg = new Regex("\\&nbsp;\\;", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex spaceReg = new Regex("\\s{2,}|\\ \\;", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex styleReg = new Regex(@"<style(.*?)</style>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex scriptReg = new Regex(@"<script(.*?)</script>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            data = styleReg.Replace(data, string.Empty);
            data = scriptReg.Replace(data, string.Empty);
            data = htmlReg.Replace(data, string.Empty);
            data = htmlSpaceReg.Replace(data, " ");
            data = spaceReg.Replace(data, " ");

            return data.Length > length && length > 0 ? data.Trim().Substring(0, length) : data.Trim();
        }

        #endregion 去除HTML标记

        #region 分割字符

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str">待分割的字符串</param>
        /// <param name="ch">分割的字符</param>
        /// <returns></returns>
        public static string[] SplitString(string str, char ch)
        {
            if (!string.IsNullOrEmpty(str))
            {
                char[] chArr = new char[1];
                chArr[0] = ch;

                return str.Split(chArr, StringSplitOptions.RemoveEmptyEntries);
            }

            return new string[0];
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str">待分割的字符串</param>
        /// <param name="split">分割的字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string str, string split)
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] splitArr = new string[1] { split };

                return str.Split(splitArr, StringSplitOptions.RemoveEmptyEntries);
            }

            return new string[0];
        }

        #endregion 分割字符

        #region 字符串截取

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾
                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

        #endregion 字符串截取

        /// <summary>
        /// 构造分页HTML(列表分页) 企业站会员中心 如：newslist-1-[page].html
        /// </summary>
        /// <param name="path">url路径</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="dataCount">总数据量</param>
        /// <param name="parms">其它条件</param>
        /// <returns></returns>
        public static string GetPageHtml(string path, int currentPage, int pageSize, int dataCount, string parms)
        {
            int pageNum = dataCount % pageSize > 0 ? (dataCount / pageSize) + 1 : dataCount / pageSize; ;//求得页面数
            string tempurl = string.Empty;//

            tempurl = path;
            pageNum = pageNum == 0 ? 1 : pageNum;
            if (currentPage > pageNum)
                currentPage = pageNum;
            else if (currentPage < 1)
                currentPage = 1;

            int nextPage = (currentPage + 1);
            int prePage = (currentPage - 1);
            int inextMark = currentPage + 2;
            int ipreMark = currentPage - 2;

            StringBuilder strHtml = new StringBuilder();
            strHtml.Remove(0, strHtml.Length);

            if (prePage < 1)
            {
                strHtml.Append("<span class=\"dis-pagefirst\"></span>");
            }
            else
            {
                strHtml.Append("<a href=\"" + tempurl.Replace("[page]", (prePage < 1 ? 1 : prePage).ToString()) + parms + "\" class=\"pagefirst\">上一页</a>");
            }

            #region 小于等于5或当前页小于5

            if (pageNum <= 5 || currentPage < 5)
            {
                inextMark = pageNum < 5 ? pageNum : 5;
                for (int pageIndex = 1; pageIndex <= inextMark; pageIndex++)
                {
                    if (pageIndex == currentPage)
                        strHtml.Append("<span class=\"oncurr\">" + currentPage + "</span>");
                    else
                        strHtml.Append("<a href=\"" + tempurl.Replace("[page]", pageIndex.ToString()) + parms + "\">" + pageIndex + "</a>");
                }
            }

            #endregion 小于等于5或当前页小于5

            #region 大于5且当前页大于5

            else if (pageNum > 5 && currentPage >= 5)
            {
                if (inextMark > pageNum)
                {
                    ipreMark = ipreMark - (inextMark - pageNum);
                    inextMark = pageNum;
                }
                for (int preIndex = ipreMark; preIndex < currentPage; preIndex++)
                {
                    strHtml.Append("<a href=\"" + tempurl.Replace("[page]", preIndex.ToString()) + parms + "\">" + preIndex + "</a>");
                }
                strHtml.Append("<span class=\"oncurr\">" + currentPage + "</span>");
                for (int nextIndex = nextPage; nextIndex <= inextMark; nextIndex++)
                {
                    strHtml.Append("<a href=\"" + tempurl.Replace("[page]", nextIndex.ToString()) + parms + "\">" + nextIndex + "</a>");
                }
            }

            #endregion 大于5且当前页大于5

            if (nextPage > pageNum)
            {
                strHtml.Append("<span class=\"dis-pagelast\"></span>");
            }
            else
            {
                strHtml.Append("<a href=\"" + tempurl.Replace("[page]", (nextPage > pageNum ? pageNum : nextPage).ToString()) + parms + "\" class=\"pagelast\">下一页</a>");
            }
            return strHtml.ToString();
        }

        #region 获取ip 省市详细地址

        /// <summary>
        /// 获取ip 省市详细地址
        /// </summary>
        /// <param name="ipInfo">ip信息</param>
        /// <returns></returns>
        public static string[] GetIpAddress(string ipInfo)
        {
            string[] str = new string[2];
            var countryInfoModel = countryInfoList.Where(p => ipInfo.Contains(p.MatchProvince)).FirstOrDefault();
            if (countryInfoModel != null)
            {
                if (countryInfoModel.Type == 3)
                {
                    str[0] = countryInfoModel.Province;
                    str[1] = ipInfo;
                }
                else if (countryInfoModel.Type == 4)
                {
                    str[0] = countryInfoModel.Province;
                    str[1] = countryInfoModel.Province + "市";
                }
                else
                {
                    str[0] = countryInfoModel.Province;
                    str[1] = ipInfo.Replace(countryInfoModel.Province, "");
                    if (str[1].Length == ipInfo.Length)
                        str[1] = ipInfo.Replace(countryInfoModel.MatchProvince, "");
                }
            }
            else if (ipInfo.Contains("省"))
            {
                string[] arr = ipInfo.Split('省');
                str[0] = arr[0] + "省";
                str[1] = arr[1];
            }
            else if (ipInfo.Contains("市") && ipInfo.Contains("区"))
            {
                string[] arr = ipInfo.Split('市');
                str[0] = arr[0] + "市";
                str[1] = arr[1];
            }
            else
            {
                str[0] = ipInfo;
                str[1] = "";
            }
            return str;
        }

        private static List<ProvinceInfo> countryInfoList = new List<ProvinceInfo>()
        {
             new ProvinceInfo(){ MatchProvince="河北", Province="河北省", Type=1},
             new ProvinceInfo(){ MatchProvince="山西", Province="山西省", Type=1},
             new ProvinceInfo(){ MatchProvince="辽宁", Province="辽宁省", Type=1},
             new ProvinceInfo(){ MatchProvince="吉林", Province="吉林省", Type=1},
             new ProvinceInfo(){ MatchProvince="黑龙江", Province="黑龙江省", Type=1},
             new ProvinceInfo(){ MatchProvince="江苏", Province="江苏省", Type=1},
             new ProvinceInfo(){ MatchProvince="浙江", Province="浙江省", Type=1},
             new ProvinceInfo(){ MatchProvince="安徽", Province="安徽省", Type=1},
             new ProvinceInfo(){ MatchProvince="福建", Province="福建省", Type=1},
             new ProvinceInfo(){ MatchProvince="江西", Province="江西省", Type=1},
             new ProvinceInfo(){ MatchProvince="山东", Province="山东省", Type=1},
             new ProvinceInfo(){ MatchProvince="河南", Province="河南省", Type=1},
             new ProvinceInfo(){ MatchProvince="湖北", Province="湖北省", Type=1},
             new ProvinceInfo(){ MatchProvince="湖南", Province="湖南省", Type=1},
             new ProvinceInfo(){ MatchProvince="广东", Province="广东省", Type=1},
             new ProvinceInfo(){ MatchProvince="海南", Province="海南省", Type=1},
             new ProvinceInfo(){ MatchProvince="四川", Province="四川省", Type=1},
             new ProvinceInfo(){ MatchProvince="贵州", Province="贵州省", Type=1},
             new ProvinceInfo(){ MatchProvince="云南", Province="云南省", Type=1},
             new ProvinceInfo(){ MatchProvince="陕西", Province="陕西省", Type=1},
             new ProvinceInfo(){ MatchProvince="甘肃", Province="甘肃省", Type=1},
             new ProvinceInfo(){ MatchProvince="青海", Province="青海省", Type=1},
             new ProvinceInfo(){ MatchProvince="台湾", Province="台湾省", Type=1},
             new ProvinceInfo(){ MatchProvince="广西", Province="广西", Type=2},
             new ProvinceInfo(){ MatchProvince="内蒙古", Province="内蒙古", Type=2},
             new ProvinceInfo(){ MatchProvince="西藏", Province="西藏", Type=2},
             new ProvinceInfo(){ MatchProvince="宁夏", Province="宁夏", Type=2},
             new ProvinceInfo(){ MatchProvince="新疆", Province="新疆", Type=2},
             new ProvinceInfo(){ MatchProvince="香港", Province="香港", Type=3},
             new ProvinceInfo(){ MatchProvince="澳门", Province="澳门", Type=3},
             new ProvinceInfo(){ MatchProvince="北京", Province="北京", Type=4},
             new ProvinceInfo(){ MatchProvince="天津", Province="天津", Type=4},
             new ProvinceInfo(){ MatchProvince="上海", Province="上海", Type=4},
             new ProvinceInfo(){ MatchProvince="重庆", Province="重庆", Type=4},
        };

        public class ProvinceInfo
        {
            /// <summary>
            /// 匹配名称
            /// </summary>
            public string MatchProvince { get; set; }

            /// <summary>
            /// 输出名称
            /// </summary>
            public string Province { get; set; }

            /// <summary>
            /// 类型 1-省(23个)  2-自治区(5个) 3-特别行政区(2个)  4-直辖市(4个)
            /// </summary>
            public int Type { get; set; }
        }

        #endregion 获取ip 省市详细地址
        
    }
}