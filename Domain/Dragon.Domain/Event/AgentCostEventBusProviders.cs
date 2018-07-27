using Dragon.Core.Log4net;
using Dragon.Domain.IRepositories;
using Dragon.Domain.Model;
using Dragon.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Domain.Event
{
 
    public class AgentCostEventBusProviders : IEventBusProvider
    {
        private readonly ILog _Logger = LogHelper.GetLogger(typeof(AgentCostEventBusProviders));
        private readonly IRepositoryContext _repositoryContext;


        public AgentCostEventBusProviders(IRepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        #region Implementation of IEventBusProvider

        public string EventName { get; } = "Dragon.Agent.ProfitsAssign";

       
        public object Execute(object[] parameters)
        {
            var accountId = Convert.ToString(parameters[0]);
            var agentPrice = Convert.ToInt32(parameters[1]);
            
            try
            {
            
            }
            catch (Exception ex)
            { 
                _Logger.Error($"", ex);
            }
            //Task.Run(async () =>
            //{
            //    //Microsoft.JScript.GlobalObject.escape(
            //}).Wait();
            return null;
        }

        #endregion Implementation of IEventBusProvider
    }

}
