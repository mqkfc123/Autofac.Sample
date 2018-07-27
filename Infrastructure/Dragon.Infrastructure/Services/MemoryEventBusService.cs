
using Dragon.Core;
using Dragon.Core.Log4net;
using Dragon.Infrastructure.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dragon.Infrastructure.Services
{
    /// <summary>
    /// 一个内存服务总线。
    /// </summary>
    public interface IMemoryEventBusService : IEventBusService, IDependency
    {
    }

    public sealed class MemoryEventBusService : IMemoryEventBusService
    {
        private readonly Lazy<IEnumerable<IEventBusProvider>> _providers;

        private readonly ILog _Logger = LogHelper.GetLogger(typeof(MemoryEventBusService));
        public MemoryEventBusService(Lazy<IEnumerable<IEventBusProvider>> providers)
        {
            _providers = providers;
        }
         
        #region Implementation of IEventBusService

        public object[] PublishRequest(string eventName, params object[] parameters)
        {
            var providers = _providers.Value.Where(i => string.Equals(eventName, i.EventName, StringComparison.OrdinalIgnoreCase));
            return providers.Invoke(i => i.Execute(parameters)).ToArray();
        }

        #endregion Implementation of IEventBusService
    }
}