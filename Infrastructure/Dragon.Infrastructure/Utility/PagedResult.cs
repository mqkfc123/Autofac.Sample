using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Auth.Infrastructure.Utility
{
    /// <summary>
    /// 表示包含了分页信息的集合类型。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T> 
    {

        #region Ctor
        /// <summary>
        /// 初始化一个新的<c>PagedResult{T}</c>类型的实例。
        /// </summary>
        public PagedResult()
        {
            Data = new List<T>();
        }

        public PagedResult(int totalRecords, int pageSize, int pageNumber, List<T> data)
        {
            this.TotalRecords = totalRecords;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.Data = data;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// 获取或设置总记录数。
        /// </summary>
        public int TotalRecords { get; set; }
        /// <summary>
        /// 获取页数。
        /// </summary>
        public int TotalPages
        {
            get
            {
                return this.TotalRecords % this.PageSize > 0 ? (this.TotalRecords / this.PageSize) + 1 : (this.TotalRecords / this.PageSize);
            }
        }
        /// <summary>
        /// 获取或设置页面大小。
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 获取或设置页码。
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// 获取或设置当前页面的数据。
        /// </summary>
        public List<T> Data { get; set; }
        #endregion

    }
}
