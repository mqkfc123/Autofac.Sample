using Dragon.Infrastructure.Services;
using Sample.Management.Web.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Services
{
    public class PublishSampleCouponRecycService
    {
        private readonly IDistributedEventBusService _distributedEventBusService;
        public PublishSampleCouponRecycService(IDistributedEventBusService distributedEventBusService)
        {
            _distributedEventBusService = distributedEventBusService;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="couponMainId">优惠券ID</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void PublishSampleCouponRecycling(string couponMainId, DateTime startTime, DateTime endTime)
        {
            _distributedEventBusService.PublishSampleCouponRecycling(couponMainId,  startTime, endTime);
        }


    }
}