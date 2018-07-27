using Dragon.Core;
using Dragon.Core.Log4net;
using Auth.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using Dragon.Infrastructure.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dragon.Infrastructure.Services
{
    /// <summary>
    /// 一个分布式事件总线服务。
    /// </summary>
    public interface IDistributedEventBusService : IEventBusService , IDependency
    {
    }

    public sealed class DistributedEventBusService : IDistributedEventBusService, IDisposable
    {

        private readonly ILog _Logger = LogHelper.GetLogger(typeof(DistributedEventBusService));

        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ISubscriber _subscriber;

        public DistributedEventBusService(IRedisFactory redisFactory, IMemoryEventBusService memoryEventBusService)
        { 
            _connectionMultiplexer = redisFactory.CreateConnection();
            _subscriber = _connectionMultiplexer.GetSubscriber();

            _subscriber.Subscribe(GetChannel(), (channel, json) =>
            {
                var jObj = JObject.Parse(json);
                var eventName = jObj.Value<string>("EventName");
                var parameters = (JArray)jObj["Parameters"];

                var invokeParameters = new List<object>(parameters.Count);
                foreach (var parameter in parameters)
                {
                    var typeName = parameter.Value<string>("TypeName");
                    var content = parameter.Value<string>("Content");

                    var type = Type.GetType(typeName, false);
                    if (type == null)
                        continue;

                    invokeParameters.Add(JsonConvert.DeserializeObject(content, type));
                }
                try
                {
                    memoryEventBusService.PublishRequest(eventName, invokeParameters.ToArray());
                }
                catch (Exception ex)
                {
                    _Logger.Error("DistributedEventBusService_Exception:" , ex);
                }
            });
        }

        #region Implementation of IEventBusService

        public object[] PublishRequest(string eventName, params object[] parameters)
        {
            parameters = parameters?.Where(i => i != null).ToArray() ?? new object[0];

            var content = JsonConvert.SerializeObject(new
            {
                EventName = eventName,
                Parameters = parameters.Select(i => new { TypeName = i.GetType().AssemblyQualifiedName, Content = JsonConvert.SerializeObject(i) })
            });
            _subscriber.Publish(GetChannel(), content);
            return new object[0];
        }

        #endregion Implementation of IEventBusService

        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _connectionMultiplexer.Dispose();
        }

        #endregion Implementation of IDisposable

        #region Private Method

        private string GetChannel()
        { 
            return "Dragon";
        }

        #endregion Private Method
    }
}