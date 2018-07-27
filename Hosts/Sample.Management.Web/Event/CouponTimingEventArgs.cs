using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Event
{
    public class CouponTimingEventArgs
    {
        public string CouponMainId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 创建一个通知定时的事件参数。
        /// </summary>
        /// <param name="couponMainId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static CouponTimingEventArgs CreateTimingEventArgs(string couponMainId, DateTime startTime, DateTime endTime)
        {
            return new CouponTimingEventArgs
            {
                CouponMainId = couponMainId,
                StartTime = startTime,
                EndTime = endTime
            };
        }

    }
}