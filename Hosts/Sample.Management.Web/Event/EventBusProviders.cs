using Newtonsoft.Json;
using StackExchange.Redis;
using Sample.Management.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Sample.Management.Web.Models;
using Dragon.Infrastructure.Services;
using Dragon.Infrastructure.Redis;
using Auth.Infrastructure.Utility;

namespace Sample.Management.Web.Event
{
    public class SignInEventBusProviders : IEventBusProvider
    {

        private readonly IMemoryEventBusService _memoryEventBusService;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public SignInEventBusProviders(IMemoryEventBusService memoryEventBusService, IRedisFactory redisFactory)
        {
            _memoryEventBusService = memoryEventBusService;
            _connectionMultiplexer = redisFactory.CreateConnection();
        }

        #region Implementation of IEventBusProvider

        public string EventName { get; } = "Sample.User.SignIn";

        public object Execute(object[] parameters)
        {
            var user = JsonConvert.DeserializeObject<UserInfo>(Convert.ToString(parameters[0]));
            //CookieHelper.SetCookie(SystemConfig.CookiesKey, user.AccountId, DateTime.Now.AddSeconds(-1));
            CookieHelper.SetCookie(SystemConfig.CookiesKey, user.AccountId, DateTime.Now.AddHours(3), SystemConfig.GetDomain());
            Task.Run(async () =>
            {
                var _sessionId = parameters[1];
                var redisDb = _connectionMultiplexer.GetDatabase();
                //Microsoft.JScript.GlobalObject.escape(
                //await redisDb.KeyExpireAsync(_sessionId + "_" + SystemConfig.Channel, TimeSpan.FromSeconds(0));
                await redisDb.HashSetAsync(_sessionId + "_" + SystemConfig.Channel, user.AccountId, JsonConvert.SerializeObject(user));
                await redisDb.KeyExpireAsync(_sessionId + "_" + SystemConfig.Channel, DateTime.Now.AddMinutes(130));
            }).Wait();
            return null;
        }

        #endregion Implementation of IEventBusProvider
    }
}