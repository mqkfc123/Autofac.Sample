using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections;

namespace Auth.Infrastructure.Utility
{
    public class PagedJsonHelper
    {
        readonly static JavaScriptSerializer _json = new JavaScriptSerializer();

        readonly static string[] typeString = new string[] { 
            typeof(string).FullName,
            typeof(bool).FullName,
            typeof(int).FullName,
            typeof(long).FullName,
            typeof(double).FullName,
            typeof(DateTime).FullName,
        };
        readonly static Dictionary<string, string> typeDict = new Dictionary<string, string>
        {
            {typeof(string).FullName,"string"},
            {typeof(bool).FullName,"bool"},
            {typeof(int).FullName,"int"},
            {typeof(long).FullName,"int"},
            {typeof(double).FullName,"double"},
            {typeof(DateTime).FullName,"date"},
        };

        private static string ConvertType(object obj)
        {
            if (obj == null) return "null";
            var type = obj.GetType();
            if (typeDict.ContainsKey(type.FullName)) return typeDict[type.FullName];
            if (obj is IEnumerable) return "array";
            return "object";
        }

        private static string ToJsonString(object obj)
        {
            return ToJsonString(obj, p => p);
        }

        private static string ToJsonString(object obj, Func<string, string> property)
        {
            string result = string.Empty;
            string typeName = ConvertType(obj);
            switch (typeName)
            {
                case "array":
                    IEnumerable list = (IEnumerable)obj;
                    foreach (object o in list)
                    {
                        result += ToJsonString(o, property);
                    }
                    return string.Format("[{0}]", result);
                case "object": var pros = obj.GetType().GetProperties();
                    foreach (var p in pros)
                    {
                        result += string.Format(",\"{0}\":{1}",
                            property(p.Name),
                            ToJsonString(p.GetValue(obj, null), property));
                    }
                    return "{" + (result.Length > 0 ? result.Substring(1) : result) + "}";
                case "double":
                case "int":
                    return obj.ToString();
                case "bool":
                    return obj.ToString().ToLower();
                case "date":
                    DateTime d = (DateTime)obj;
                    var time = (d.Ticks - new DateTime(1970, 1, 1, 8, 0, 0).Ticks) / 10000;
                    return string.Format("new Date({0})", time < 0 ? 0 : time);
                case "null":
                    return "null";
                case "string":
                default:
                    return string.Format("\"{0}\"", obj.ToString());

            }
        }

        /// <summary>
        /// 将分页转换为 JSON 字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paged"></param>
        /// <returns></returns>
        public static string Serialize<T>(PagedResult<T> paged)
        {
            //return ToJsonString(new
            //{
            //    size = paged.PageSize,
            //    total = paged.TotalRecords,
            //    page = paged.PageNumber,
            //    rows = paged.Data
            //});

            return _json.Serialize(new
            {
                size = paged.PageSize,
                total = paged.TotalRecords,
                page = paged.PageNumber,
                rows = paged.Data
            });
        }

        /// <summary>
        /// 将分页转换为 JSON 字符串。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="paged"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static string Serialize<T, TResult>(PagedResult<T> paged, Func<T, TResult> selector)
        {

            //return ToJsonString(new
            //{
            //    size = paged.PageSize,
            //    total = paged.TotalRecords,
            //    page = paged.PageNumber,
            //    rows = paged.Data.Select(selector).ToList()
            //});

            return _json.Serialize(new
            {
                size = paged.PageSize,
                total = paged.TotalRecords,
                page = paged.PageNumber,
                rows = paged.Data.Select(selector).ToList(),
                totalPage = paged.TotalPages
            });
        }


        //public static string Serialize<T, TResult>(PagedResult<T> paged, Func<T, TResult> selector, Func<string, string> property)
        //{

        //    return ToJsonString(new
        //    {
        //        size = paged.PageSize,
        //        total = paged.TotalRecords,
        //        page = paged.PageNumber,
        //        rows = paged.Data.Select(selector).ToList()
        //    }, property);
        //}

        //public static string SerializeShield<T, TResult>(PagedResult<T> paged, Func<T, TResult> shield)
        //{

        //    return ToJsonString(new
        //    {
        //        size = paged.PageSize,
        //        total = paged.TotalRecords,
        //        page = paged.PageNumber,
        //        rows = paged.Data
        //    }, p => p.ToLower());

        //    //return _json.Serialize(new
        //    //{
        //    //    size = paged.PageSize,
        //    //    total = paged.TotalRecords,
        //    //    page = paged.PageNumber,
        //    //    rows = paged.Data.Select(selector).ToList()
        //    //});
        //}





        /// <summary>
        /// 将对象转换成JSON字符串
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Serialize(object o)
        {
            return _json.Serialize(o);
        }

        /// <summary>
        /// 将指定的 JSON 字符串转换为 T 类型的对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string input)
        {
            return _json.Deserialize<T>(input);
        }
        /// <summary>
        /// 将数据表转换成JSON类型串
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="size">每月显示条数</param>
        /// <param name="page">页码</param>
        /// <param name="count">总条数</param>
        /// <returns></returns>
        public static StringBuilder DataTableToJson(System.Data.DataTable dt, int size, int page, int count)
        {
            StringBuilder str = new StringBuilder();
            str.Append("{");
            str.AppendFormat("size:{0},", size);
            str.AppendFormat("total:{0},", count);
            str.AppendFormat("page:{0},", page);
            str.Append("rows:");
            str.Append(DataTableToJson(dt));
            str.Append("}");
            return str;
        }
        /// <summary>
        /// 将数据表转换成JSON类型串
        /// </summary>
        /// <param name="dt">要转换的数据表</param>
        /// <returns></returns>
        public static StringBuilder DataTableToJson(System.Data.DataTable dt)
        {
            return DataTableToJson(dt, true);
        }

        /// <summary>
        /// 将数据表转换成JSON类型串
        /// </summary>
        /// <param name="dt">要转换的数据表</param>
        /// <param name="dispose">数据表转换结束后是否dispose掉</param>
        /// <returns></returns>
        public static StringBuilder DataTableToJson(System.Data.DataTable dt, bool dispose)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[\r\n");

            //数据表字段名和类型数组
            string[] dt_field = new string[dt.Columns.Count];
            int i = 0;
            string formatStr = "{{";
            string fieldtype = "";
            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                dt_field[i] = dc.Caption.ToLower().Trim();
                formatStr += "'" + dc.Caption.ToLower().Trim() + "':";
                fieldtype = dc.DataType.ToString().Trim().ToLower();
                //if (fieldtype.IndexOf("int") > 0 || fieldtype.IndexOf("deci") > 0 ||
                //    fieldtype.IndexOf("floa") > 0 || fieldtype.IndexOf("doub") > 0 ||
                //    fieldtype.IndexOf("bool") > 0)
                //{
                //    formatStr += "{" + i + "}";
                //}
                //else
                //{
                formatStr += "'{" + i + "}'";
                //}
                formatStr += ",";
                i++;
            }

            if (formatStr.EndsWith(","))
                formatStr = formatStr.Substring(0, formatStr.Length - 1);//去掉尾部","号

            formatStr += "}},";

            i = 0;
            object[] objectArray = new object[dt_field.Length];
            foreach (System.Data.DataRow dr in dt.Rows)
            {

                foreach (string fieldname in dt_field)
                {   //对 \ , ' 符号进行转换 
                    objectArray[i] = dr[dt_field[i]].ToString().Trim().Replace("\\", "\\\\").Replace("'", "\\'");
                    switch (objectArray[i].ToString())
                    {
                        case "True":
                            {
                                objectArray[i] = "true"; break;
                            }
                        case "False":
                            {
                                objectArray[i] = "false"; break;
                            }
                        default: break;
                    }
                    i++;
                }
                i = 0;
                stringBuilder.Append(string.Format(formatStr, objectArray));
            }
            if (stringBuilder.ToString().EndsWith(","))
                stringBuilder.Remove(stringBuilder.Length - 1, 1);//去掉尾部","号

            if (dispose)
                dt.Dispose();

            return stringBuilder.Append("\r\n]");
        }
    }
}
