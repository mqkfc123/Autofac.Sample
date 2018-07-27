using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Event
{
    public class QuesTimingEventArgs
    {
        public string QuesAireMainId { get; set; }

        /// <summary>
        /// 问卷类型  1 空白 2 模板 3 奖励 4 试题
        /// </summary>
        //public EnumQuesAireType QuesAireType { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 创建一个通知定时的事件参数。
        /// </summary>
        /// <param name="record">商品记录。</param>
        /// <returns>商品变更事件参数。</returns>
        public static QuesTimingEventArgs CreateTimingEventArgs(string quesAireMainId, int quesAireType, DateTime endTime, DateTime startTime)
        {
            return new QuesTimingEventArgs
            {
                QuesAireMainId = quesAireMainId,
                EndTime = endTime,
                StartTime = startTime
            };
        }


    }
}