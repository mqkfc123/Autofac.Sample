using System;


namespace Auth.Infrastructure.Mvc
{
    [Serializable]
    public class DataResult
    {
        public DataResult()
        {
        }
        /// <summary>
        /// 返回状态码 
        /// </summary>
        public int Code;
        /// <summary>
        /// 返回信息 （必填）
        /// </summary>
        public string Message;
        /// <summary>
        ///返回数据 （可选）
        /// </summary>
        public object Data;
    }
}
