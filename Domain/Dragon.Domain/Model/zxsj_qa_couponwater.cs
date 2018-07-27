using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surevy.Domain.Model
{

    /// <summary>
    /// 优惠券领用表
    /// </summary>
    public class ZXSJ_QA_CouponWater
    {
        public string CouponWaterId { get; set; }
        /// <summary>
        /// 领用者ID  前端用户，GUID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 1正常 0删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 1 未使用 2已使用
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 优惠券有效期（开始）
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 优惠券有效期（结束）
        /// </summary>
        public DateTime EndTime { get; set; }

        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 优惠券模板ID
        /// </summary>
        public string CouponMainId { get; set; }

        public ZXSJ_QA_CouponMain CouponMain { get; set; }

    }


}
