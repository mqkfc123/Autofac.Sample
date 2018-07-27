using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Auth.Infrastructure.Utility
{
    /// </summary>
    public class CommFunction
    {

        #region 字符串处理

        /// <summary>
        /// 获取指定长度的字符串
        /// </summary>
        /// <param name="orgStr">原始字符串</param>
        /// <param name="length">获取长度</param>
        /// <returns></returns>
        public static string Substring(string orgStr, int length)
        {
            return Substring(orgStr, length, "..");
        }

        /// <summary>
        /// 获取指定长度的字符串
        /// </summary>
        /// <param name="orgStr">原始字符串</param>
        /// <param name="length">获取长度</param>
        /// <param name="tailString">截取后的后缀字符</param>
        /// <returns>指定长度的字符串</returns>
        public static string Substring(string orgStr, int length, string tailString)
        {
            string returnValue = orgStr;
            if (!string.IsNullOrEmpty(orgStr))
            {
                if (length >= 0)
                {
                    byte[] bsSrcString = System.Text.Encoding.GetEncoding("GB2312").GetBytes(orgStr);

                    if (bsSrcString.Length > length)
                    {
                        int nRealLength = length;
                        int[] anResultFlag = new int[length];
                        byte[] bsResult = null;

                        int nFlag = 0;
                        for (int i = 0; i < length; i++)
                        {

                            if (bsSrcString[i] > 127)
                            {
                                nFlag++;
                                if (nFlag == 3)
                                {
                                    nFlag = 1;
                                }
                            }
                            else
                            {
                                nFlag = 0;
                            }

                            anResultFlag[i] = nFlag;
                        }

                        if ((bsSrcString[length - 1] > 127) && (anResultFlag[length - 1] == 1))
                        {
                            nRealLength = length + 1;
                        }

                        bsResult = new byte[nRealLength];

                        Array.Copy(bsSrcString, bsResult, nRealLength);

                        returnValue = System.Text.Encoding.GetEncoding("GB2312").GetString(bsResult) + tailString;
                    }
                }
            }
            return returnValue;
        }

        #endregion

        #region 类型转换函数

        /// <summary>
        /// 将对象类型转换为整型值
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <returns>整型值</returns>
        public static int? IntParseByNull(object objValue)
        {
            int? returnValue = null;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = int.Parse(objValue.ToString());
                }
                catch
                {
                    returnValue = null;
                }
            }
            //返回值
            return returnValue;
        }

        /// <summary>
        /// 将字符型类型转换为整型值
        /// </summary>
        /// <param name="objValue">字符型</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>整型值</returns>
        public static int IntParse(string objValue, int defaultValue)
        {
            int returnValue = defaultValue;
            if (!string.IsNullOrEmpty(objValue))
            {
                try
                {
                    returnValue = int.Parse(objValue);
                }
                catch
                {
                    returnValue = defaultValue;
                }
            }

            return returnValue;
        }
        /// <summary>
        /// 将对象类型转换为整型值
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>整型值</returns>
        public static int IntParse(object objValue, int defaultValue)
        {
            int returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = int.Parse(objValue.ToString());
                }
                catch
                {
                    returnValue = defaultValue;
                }
            }

            //返回值
            return returnValue;
        }
        /// <summary>
        /// 将对象类型转换为整型值
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <returns>整型值</returns>
        public static int IntParse(object objValue)
        {
            return IntParse(objValue, 0);
        }
        /// <summary>
        /// 将字符型类型转换为浮点型
        /// </summary>
        /// <param name="objValue">字符串</param>
        /// <param name="defalutValue">默认值</param>
        /// <returns></returns>
        public static float FloatParse(string objValue, float defalutValue)
        {
            float returnValue = defalutValue;
            if (!string.IsNullOrEmpty(objValue))
            {
                try
                {
                    returnValue = float.Parse(objValue);
                }
                catch (Exception)
                {

                    returnValue = defalutValue;
                }

            }
            return returnValue;
        }
        /// <summary>
        /// 将对象类型转换为浮点值
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <returns>浮点值</returns>
        public static float FloatParse(object objValue, float defalutValue)
        {
            float returnValue = defalutValue;
            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = float.Parse(objValue.ToString());
                }
                catch (Exception)
                {

                    returnValue = defalutValue;
                }

            }
            return returnValue;
        }
        public static float FloatParse(object objValue)
        {
            return FloatParse(objValue, 0);
        }
        /// <summary>
        /// 将对象类型转换为日期值
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>日期值</returns>
        public static DateTime DateTimeParse(object objValue, DateTime defaultValue)
        {
            DateTime returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = DateTime.Parse(objValue.ToString());
                }
                catch
                {
                    returnValue = defaultValue;
                }
            }

            //返回值
            return returnValue;
        }


        /// <summary>
        /// 将对象类型转换为日期值
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <param name="defaultValue">无法转换时返回null</param>
        /// <returns>日期值</returns>
        public static DateTime? DateTimeParseByNull(object objValue)
        {
            DateTime? returnValue = null;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = DateTime.Parse(objValue.ToString());
                }
                catch
                {
                    returnValue = null;
                }
            }

            //返回值
            return returnValue;
        }

        /// <summary>
        /// 将对象类型转换为日期值
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <returns>日期值</returns>
        public static DateTime DateTimeParse(object objValue)
        {
            return DateTimeParse(objValue, DateTime.MinValue);
        }


        /// <summary>
        /// 将对象类型转换为字符型
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>字符型</returns>
        public static string StringParse(object objValue, string defaultValue)
        {
            string returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = objValue.ToString();
                }
                catch
                {
                    returnValue = defaultValue; ;
                }

            }

            //返回值
            return returnValue;
        }

        /// <summary>
        /// 将对象类型转换为字符型
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <returns>字符型</returns>
        public static string StringParse(object objValue)
        {
            return StringParse(objValue, string.Empty);
        }


        /// <summary>
        /// 将对象类型转换为GUID
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>GUID</returns>
        public static Guid GuidParse(object objValue, Guid defaultValue)
        {
            Guid returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = new Guid(objValue.ToString());
                }
                catch
                {
                    returnValue = defaultValue; ;
                }

            }

            //返回值
            return returnValue;
        }


        /// <summary>
        /// 将对象类型转换为GUID
        /// </summary>
        /// <param name="objValue">对象类型</param>
        /// <returns>GUID</returns>
        public static Guid GuidParse(object objValue)
        {
            return GuidParse(objValue, Guid.Empty);
        }

        /// <summary>
        /// 类型转换函数
        /// </summary>
        /// <typeparam name="T">目标类型值</typeparam>
        /// <param name="objValue">对象类型</param>
        /// <param name="defaultValue">无法转换时的默认值</param>
        /// <returns>目标类型值</returns>
        public static T Parse<T>(object objValue, T defaultValue)
        {
            T returnValue = defaultValue;

            if (objValue != null && objValue != DBNull.Value)
            {
                try
                {
                    returnValue = (T)objValue;
                }
                catch
                {
                    returnValue = defaultValue;
                }
            }

            //返回值
            return returnValue;
        }

        /// <summary>
        /// 类型转换函数
        /// </summary>
        /// <typeparam name="T">目标类型值</typeparam>
        /// <param name="objValue">对象类型</param>
        /// <returns>目标类型值</returns>
        public static T Parse<T>(object objValue)
        {
            return Parse<T>(objValue, default(T));
        }
        #endregion

        #region 汉字与字母转换

        /// <summary>
        /// 汉字转拼音缩写
        ///<returns>拼音缩写</returns>
        ///</summary>
        public string GetPyString(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    //字母和符号原样保留           
                    tempStr += c.ToString();
                }
                else
                {
                    //累加拼音声母     
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }

        /// <summary>
        /// 取单个字符的拼音声母
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        /// </summary>
        public string GetPYChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "g";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";
            return "*";
        }
        #endregion

        #region 字符串过滤
        /// <summary>
        /// 字符串过滤
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string HtmlStr(string htmlstring)
        {
            if (string.IsNullOrEmpty(htmlstring))
            {
                return string.Empty;
            }
            else
            {
                htmlstring = htmlstring.ToLower();
                htmlstring = htmlstring.Replace("'", "‘");
                htmlstring = htmlstring.Replace("\"", "");
                htmlstring = htmlstring.Replace(";", "；");
                htmlstring = htmlstring.Replace("-", "－");
                htmlstring = htmlstring.Replace("--", "－－");
                htmlstring = htmlstring.Replace("==", "＝＝");
                htmlstring = htmlstring.Replace("=", "＝");
                htmlstring = htmlstring.Replace(">", "&gt;");
                htmlstring = htmlstring.Replace("<", "&lt;");
                htmlstring = htmlstring.Replace("&", "&amp;");

                //删除脚本
                htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                htmlstring = Regex.Replace(htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

                htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);

                //删除与数据库相关的词
                htmlstring = Regex.Replace(htmlstring, "select", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "insert", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "delete from", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "count''", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "drop table", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "asc", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "mid", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "char", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "exec master", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "and", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "exec", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "alter", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "group up", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "user", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "and", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "update", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "delete", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "declare", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "@", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "dbcc", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "if", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "else", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "or", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "add", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "set", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "open", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "close", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "begin", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "retun", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "as", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "go", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "exists", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "kill", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "nchar", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "nvarchar", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "char", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "nvchar", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "text", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "ntext", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "table", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "proc", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "proc", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "master", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "sp_", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "xp_", "", RegexOptions.IgnoreCase);
                htmlstring = Regex.Replace(htmlstring, "sys", "", RegexOptions.IgnoreCase);
                return htmlstring;

            }
        }
        #endregion

        #region 对传入的内容进行代码屏蔽【用于问医生提问和回复】
        /// <summary>
        /// 对传入的内容进行代码屏蔽【用于问医生提问和回复,不过滤sql代码】
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string HTMLEncode(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return string.Empty;
            }
            else
            {
                Htmlstring = Htmlstring.Replace("'", "‘");
                Htmlstring = Htmlstring.Replace(";", "；");
                Htmlstring = Htmlstring.Replace("-", "－");
                Htmlstring = Htmlstring.Replace("--", "－－");
                Htmlstring = Htmlstring.Replace("==", "＝＝");
                Htmlstring = Htmlstring.Replace("=", "＝");
                Htmlstring = Htmlstring.Replace(">", "&gt;");
                Htmlstring = Htmlstring.Replace("<", "&lt;");

                //删除脚本
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "<br/>", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([\n])[\s]+", "<br/>", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"\n", "<br/>", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);


                return Htmlstring;
            }
        }
        /// <summary>
        /// 对传入的内容进行html代码剔除.主要供列表显示用
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string HTMLCheck(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return string.Empty;
            }
            else
            {
                Htmlstring = Htmlstring.ToLower();
                Htmlstring = Htmlstring.Replace("<br/>", "");
                Htmlstring = Htmlstring.Replace("\"", "");
                Htmlstring = Htmlstring.Replace(">", "&gt;");
                Htmlstring = Htmlstring.Replace("<", "&lt;");
                //删除脚本
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([\n])[\s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"\n", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                Htmlstring = Htmlstring.Replace("&", "");
                return Htmlstring;
            }
        }
        #endregion

        #region 将xml文件转为所需要的table对象
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtParam">所需要的table列表列名数组</param>
        /// <param name="noteName">解析列表的字节名称</param>
        /// <param name="xmlStr">xml原始字符串</param>
        /// <returns></returns>
        public static DataTable XMLToDataTable(string[] dtParam, string noteName, string xmlStr)
        {
            DataTable datTable = new DataTable();
            if (dtParam.Length > 0 && noteName.Length > 0)
            {
                for (int i = 0; i < dtParam.Length; i++)
                {
                    DataColumn dc = new DataColumn(dtParam[i]);
                    datTable.Columns.Add(dc);
                }
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    //处理string类型xml文件
                    xmlDoc.LoadXml(xmlStr);
                    System.Xml.XmlNodeList root = xmlDoc.ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes;
                    //string DoctorResultCode = root[0].SelectSingleNode(dtParam[0]).InnerText;
                    //string DoctorMsg = root[0].SelectSingleNode(dtParam[2]).InnerText;
                    //root[0].ChildNodes[0].Name
                    if (root != null)
                    {
                        foreach (XmlNode xn in root)
                        {
                            DataRow dr = datTable.NewRow();
                            for (int i = 0; i < datTable.Columns.Count; i++)
                            {

                                for (int j = 0; j < xn.ChildNodes.Count; j++)
                                {
                                    if (xn.ChildNodes[j].Name == datTable.Columns[i].ColumnName)
                                    {
                                        dr[datTable.Columns[i].ColumnName] = xn.ChildNodes[j].InnerText;
                                    }
                                }
                            }
                            datTable.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        //没有获取到列表
                    }
                }
                catch (Exception e)
                {
                    //显示错误信息                    
                }
            }
            else
            {

            }
            return datTable;
        }
        #endregion

        #region 数据库敏感屏蔽
        /// <summary>
        /// 对传入的内容进行代码屏蔽
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string HTMLSQL(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return string.Empty;
            }
            else
            {
                //Htmlstring = Htmlstring.ToLower();
                Htmlstring = Htmlstring.Replace("'", "‘");
                Htmlstring = Htmlstring.Replace("\"", "“");
                Htmlstring = Htmlstring.Replace("\\", "");
                //Htmlstring = Htmlstring.Replace(";", "；");
                //Htmlstring = Htmlstring.Replace("-", "－");
                //Htmlstring = Htmlstring.Replace("--", "－－");
                //Htmlstring = Htmlstring.Replace("==", "＝＝");
                //Htmlstring = Htmlstring.Replace("=", "＝");
                //Htmlstring = Htmlstring.Replace(">", "&gt;");
                //Htmlstring = Htmlstring.Replace("<", "&lt;");
                //Htmlstring = Htmlstring.Replace("&", "&amp;");

                //删除脚本
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                //Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

                //Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);

                //删除与数据库相关的词
                Htmlstring = Regex.Replace(Htmlstring, "select", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete from", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "count''", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop table", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "asc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "mid", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "char", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "and", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "alter", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "group up", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "user", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "and", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "update", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "declare", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "@", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "/", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "\\", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "dbcc", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "if", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "else", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "or", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "add", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "set", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "open", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "close", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "begin", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "retun", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "as", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "go", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exists", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "kill", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "nchar", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "nvarchar", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "char", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "nvchar", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "text", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "ntext", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring, "table", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "proc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "proc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "sp_", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "sys", "", RegexOptions.IgnoreCase);
                return Htmlstring;

            }
        }
        /// <summary>
        /// 只屏蔽Update、Delete、;等等
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <returns></returns>
        public static string HTMLSQL1(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return string.Empty;
            }
            else
            {
                Htmlstring = Htmlstring.ToLower();
                Htmlstring = Htmlstring.Replace(";", "");
                //删除脚本
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                //删除与数据库相关的词
                Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "group up", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "update", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "declare", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "dbcc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "begin", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "retun", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exists", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "table", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "proc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "sp_", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_", "", RegexOptions.IgnoreCase);
                return Htmlstring;

            }
        }

        #endregion

        #region 转人民币
        public static string FenToYuan(int balance)
        {
            int Balance = balance;
            if (balance < 0)
                Balance = -balance;
            int y = Balance / 100;
            int f = Balance % 100;
            int i = f / 10;
            int j = f % 10;
            if (balance >= 0)
                return y.ToString() + "." + i.ToString() + j.ToString();
            else
                return "-" + y.ToString() + "." + i.ToString() + j.ToString();
        }
        #endregion

        #region 获取星期几
        /// <summary>
        /// 获取星期几
        /// </summary>
        /// <param name="myDateTime">日期</param>
        /// <returns></returns>

        public static string GetWeekDayName(DateTime myDateTime)
        {
            string week = "";
            //获取当前日期是星期几
            string dt = myDateTime.DayOfWeek.ToString();
            //根据取得的星期英文单词返回汉字
            switch (dt)
            {
                case "Monday":
                    week = "星期一";
                    break;
                case "Tuesday":
                    week = "星期二";
                    break;
                case "Wednesday":
                    week = "星期三";
                    break;
                case "Thursday":
                    week = "星期四";
                    break;
                case "Friday":
                    week = "星期五";
                    break;
                case "Saturday":
                    week = "星期六";
                    break;
                case "Sunday":
                    week = "星期日";
                    break;
            }
            return week;
        }
        #endregion

        #region 获取星期几 数字
        /// <summary>
        /// 获取星期几 数字
        /// </summary>
        /// <param name="myDateTime">日期</param>
        /// <returns></returns>

        public static int GetWeekDayIndex(DateTime myDateTime)
        {
            int weekindex = 0;
            //获取当前日期是星期几
            string dt = myDateTime.DayOfWeek.ToString();
            //根据取得的星期英文单词返回汉字
            switch (dt)
            {
                case "Monday":
                    weekindex = 1;
                    break;
                case "Tuesday":
                    weekindex = 2;
                    break;
                case "Wednesday":
                    weekindex = 3;
                    break;
                case "Thursday":
                    weekindex = 4;
                    break;
                case "Friday":
                    weekindex = 5;
                    break;
                case "Saturday":
                    weekindex = 6;
                    break;
                case "Sunday":
                    weekindex = 0;
                    break;
            }
            return weekindex;
        }
        #endregion

        #region 根据weekid获取星期名称
        /// <summary>
        /// 根据weekid获取星期名称
        /// </summary>
        /// <param name="weekid">星期编号</param>
        /// <returns></returns>
        public static string GetWeekDayName(int weekid)
        {
            string week = "";
            switch (weekid)
            {
                case 1:
                    week = "星期一";
                    break;
                case 2:
                    week = "星期二";
                    break;
                case 3:
                    week = "星期三";
                    break;
                case 4:
                    week = "星期四";
                    break;
                case 5:
                    week = "星期五";
                    break;
                case 6:
                    week = "星期六";
                    break;
                case 0:
                case 7:
                    week = "星期日";
                    break;
            }
            return week;
        }
        #endregion

        #region 根据timeid获取时间名称
        /// <summary>
        /// 根据timeid获取时间名称
        /// </summary>
        /// <param name="timeid"></param>
        /// <returns></returns>
        public static string GetTimeName(int timeid)
        {
            string time = "";
            switch (timeid)
            {
                case 1:
                    time = "上午";
                    break;
                case 2:
                    time = "下午";
                    break;
                case 3:
                    time = "晚上";
                    break;
            }
            return time;
        }
        #endregion

        #region 返回当前本周，下周 星期几
        /// <summary>
        /// 返回当前本周，下周 星期几
        /// </summary>
        /// <param name="myDateTime">日期</param>
        /// <returns></returns>
        public static string WeekDay(DateTime myDateTime)
        {
            string weekdays = string.Empty;
            DateTime currentDateTime = DateTime.Now;
            int currentDayOfWeek;            //今天星期几
            DateTime currentStartWeek;       //本周周一
            DateTime currentEndWeek;         //本周周日
            //DateTime nextStartWeek;          //下周周一
            DateTime nextEndWeek;            //下周周日
            currentDayOfWeek = Convert.ToInt32(currentDateTime.DayOfWeek.ToString("d"));
            currentStartWeek = currentDateTime.AddDays(1 - ((currentDayOfWeek == 0) ? 7 : currentDayOfWeek));   //本周周一
            currentEndWeek = currentStartWeek.AddDays(6);//本周周日
            nextEndWeek = currentEndWeek.AddDays(7);//下周日
            if (myDateTime > nextEndWeek)
            {
                weekdays = "本周" + GetWeekDayName1(myDateTime);
            }
            else if (myDateTime > currentEndWeek)
            {
                weekdays = "下周" + GetWeekDayName1(myDateTime);
            }
            else
            {
                weekdays = GetWeekDayName(myDateTime);
            }
            return weekdays;
        }

        /// <summary>
        /// 返回当前本周，下周 星期几
        /// </summary>
        /// <param name="myDateTime">日期</param>
        /// <returns></returns>
        public static string WeekDay1(DateTime myDateTime)
        {
            string weekdays = string.Empty;
            DateTime currentDateTime = DateTime.Now;
            int currentDayOfWeek;            //今天星期几
            DateTime currentStartWeek;       //本周周一
            DateTime currentEndWeek;         //本周周日
            //DateTime nextStartWeek;          //下周周一
            DateTime nextEndWeek;            //下周周日
            currentDayOfWeek = Convert.ToInt32(currentDateTime.DayOfWeek.ToString("d"));
            currentStartWeek = currentDateTime.AddDays(1 - ((currentDayOfWeek == 0) ? 7 : currentDayOfWeek));   //本周周一
            currentEndWeek = currentStartWeek.AddDays(6);//本周周日
            nextEndWeek = currentEndWeek.AddDays(7);//下周日
            if (myDateTime <= currentEndWeek)
            {
                weekdays = "本周" + GetWeekDayName1(myDateTime);
            }
            else if (myDateTime > currentEndWeek && myDateTime <= nextEndWeek)
            {
                weekdays = "下周" + GetWeekDayName1(myDateTime);
            }
            else
            {
                weekdays = GetWeekDayName(myDateTime);
            }
            return weekdays;
        }

        /// <summary>
        /// 获取星期几
        /// </summary>
        /// <param name="myDateTime">日期</param>
        /// <returns></returns>

        public static string GetWeekDayName1(DateTime myDateTime)
        {
            string week = "";
            //获取当前日期是星期几
            string dt = myDateTime.DayOfWeek.ToString();
            //根据取得的星期英文单词返回汉字
            switch (dt)
            {
                case "Monday":
                    week = "一";
                    break;
                case "Tuesday":
                    week = "二";
                    break;
                case "Wednesday":
                    week = "三";
                    break;
                case "Thursday":
                    week = "四";
                    break;
                case "Friday":
                    week = "五";
                    break;
                case "Saturday":
                    week = "六";
                    break;
                case "Sunday":
                    week = "日";
                    break;
            }
            return week;
        }

        #endregion

        #region 删除文件
        //删除文件
        public static void DeletePath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path = GetMapPath(path);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.SetAttributes(path, System.IO.FileAttributes.Normal);
                    System.IO.File.Delete(path);
                }
            }
        }
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (strPath.ToLower().StartsWith("http://"))
            {
                return strPath;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region  判断是否为数字
        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <returns>是数字则返回true</returns>
        public static bool IsNumer(string str)
        {
            System.Text.RegularExpressions.Regex regnum = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return regnum.IsMatch(str);
        }
        #endregion

        /// <summary>
        /// 获取GUID
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static string[] GetLocalIpv4()
        {
            //事先不知道ip的个数，数组长度未知，因此用StringCollection储存
            try
            {
                IPAddress[] localIPs;
                localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                StringCollection IpCollection = new StringCollection();
                foreach (IPAddress ip in localIPs)
                {
                    //根据AddressFamily判断是否为ipv4,如果是InterNetWork则为ipv6
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        IpCollection.Add(ip.ToString());
                }
                string[] IpArray = new string[IpCollection.Count];
                IpCollection.CopyTo(IpArray, 0);
                return IpArray;
            }

            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }

    }
}
