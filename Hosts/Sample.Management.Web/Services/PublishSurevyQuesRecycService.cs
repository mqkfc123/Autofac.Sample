using Dragon.Infrastructure.Services;
using Sample.Management.Web.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Services
{
    public class PublishSampleQuesRecycService
    {
        private readonly IDistributedEventBusService _distributedEventBusService;
        public PublishSampleQuesRecycService(IDistributedEventBusService distributedEventBusService)
        {
            _distributedEventBusService = distributedEventBusService;

        }

        public void PublishSampleQuesRecycling(string quesAireMainId, int quesAireType, DateTime startTime, DateTime endTime)
        {
            _distributedEventBusService.PublishSampleQuesRecycling(quesAireMainId, (int)quesAireType, startTime, endTime);
        }

    } 
}