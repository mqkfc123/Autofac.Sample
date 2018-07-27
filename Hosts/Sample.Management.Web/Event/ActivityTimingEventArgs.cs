using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Event
{
    public class ActivityTimingEventArgs
    {
        public string ActivityId { get; set; }

        public int State { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }


        /// <summary>
        /// 创建一个通知定时的事件参数。
        /// </summary>
        /// <param name="couponMainId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static ActivityTimingEventArgs CreateTimingEventArgs(string activityId, DateTime startTime, DateTime endTime,int state)
        {
            return new ActivityTimingEventArgs
            {
                ActivityId = activityId,
                State = state,
                StartTime = startTime,
                EndTime = endTime
            };
        }


    }
}