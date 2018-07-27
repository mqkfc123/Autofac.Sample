using Dragon.Infrastructure.Services;
using Sample.Management.Web.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Services
{
    public class PublishSampleActivityRecycService
    {
        private readonly IDistributedEventBusService _distributedEventBusService;
        public PublishSampleActivityRecycService(IDistributedEventBusService distributedEventBusService)
        {
            _distributedEventBusService = distributedEventBusService;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId">活动ID</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void PublishSampleActivityRecycling(string activityId, DateTime startTime, DateTime endTime,int state)
        {
            _distributedEventBusService.PublishSampleActivityRecycling(activityId, startTime, endTime, state);
        }

    }
}