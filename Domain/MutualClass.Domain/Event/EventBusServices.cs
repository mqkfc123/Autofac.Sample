using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dragon.Infrastructure.Services;

namespace Dragon.Domain.Event
{
    public static class EventBusServices
    {

        public const string AgentProfitsAssign = "Dragon.Agent.ProfitsAssign";
        

        /// <summary>
        ///   
        /// </summary>
        /// <param name="memoryEventBusService"></param>
        /// <param name="accountId"></param>
        public static void PublishAgentProfitsAssign(this IMemoryEventBusService memoryEventBusService, string accountId, int agentPrice)
        {
            var parameters = new[] { accountId, agentPrice.ToString() };
            memoryEventBusService.PublishRequest(AgentProfitsAssign, parameters);
        }
        
    }
}
