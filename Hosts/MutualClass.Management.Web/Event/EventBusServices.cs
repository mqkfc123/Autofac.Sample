using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sample.Management.Web.Models;
using Dragon.Infrastructure.Services;

namespace Sample.Management.Web.Event
{
    public static class EventBusServices
    {
        public const string SampleUserSignIn = "Sample.User.SignIn";

        public const string SampleQuesRecycling = "Sample.QuesRecycling";

        public const string SampleCouponRecycling = "Sample.CouponRecycling";
        /// <summary>
        /// 定时任务开启暂停
        /// </summary>
        public const string SampleActivityRecycling = "Sample.ActivityRecycling";

        /// <summary>
        /// 登入信息
        /// </summary>
        /// <param name="memoryEventBusService"></param>
        /// <param name="accountId"></param>
        public static void PublishUserSignIn(this IMemoryEventBusService memoryEventBusService, UserInfo user, string sessionID)
        {
            var parameters = new[] { JsonConvert.SerializeObject(user), sessionID };
            memoryEventBusService.PublishRequest(SampleUserSignIn, parameters);
        }

        /// <summary>
        /// 发布问卷 定时抽奖
        /// </summary>
        /// <param name="distributedEventBusService"></param>
        /// <param name="record"></param>
        public static void PublishSampleQuesRecycling(this IDistributedEventBusService distributedEventBusService, string quesAireMainId, int quesAireType, DateTime startTime, DateTime endTime)
        {
            distributedEventBusService.PublishRequest(SampleQuesRecycling,
                QuesTimingEventArgs.CreateTimingEventArgs(quesAireMainId, quesAireType, endTime, startTime));
        }

        /// <summary>
        ///  优惠券 定时 过期
        /// </summary>
        /// <param name="distributedEventBusService"></param>
        /// <param name="record"></param>
        public static void PublishSampleCouponRecycling(this IDistributedEventBusService distributedEventBusService, string couponMainId, DateTime startTime, DateTime endTime)
        {
            distributedEventBusService.PublishRequest(SampleCouponRecycling,
                CouponTimingEventArgs.CreateTimingEventArgs(couponMainId, startTime, endTime));
        }


        /// <summary>
        ///  活动 定时 开启 过期
        /// </summary>
        /// <param name="distributedEventBusService"></param>
        /// <param name="record"></param>
        public static void PublishSampleActivityRecycling(this IDistributedEventBusService distributedEventBusService, string activityId, DateTime startTime, DateTime endTime,int state)
        {
            distributedEventBusService.PublishRequest(SampleActivityRecycling,
                ActivityTimingEventArgs.CreateTimingEventArgs(activityId, startTime, endTime, state));
        }


    }
}