using Sample.Management.Web.Event;
using Sample.Management.Web.Models;
using Sample.Management.Web.Utility;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dragon.Infrastructure.Services;
using Dragon.Infrastructure.Redis;
using Auth.Infrastructure.Utility;

namespace Sample.Management.Web.Services
{
    public class UserService
    {
        private readonly IMemoryEventBusService _memoryEventBusService;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly string _cookieAccountId;
        public UserService(IMemoryEventBusService memoryEventBusService, IRedisFactory redisFactory)
        {
            _memoryEventBusService = memoryEventBusService;
            _connectionMultiplexer = redisFactory.CreateConnection();
            _cookieAccountId = CookieHelper.GetCookieValue(SystemConfig.CookiesKey);
        }
        /// <summary>
        /// 判断用户是否登入
        /// </summary>
        /// <returns></returns>
        public bool ExistsSignIn(string sessionId)
        {
            var redisDb = _connectionMultiplexer.GetDatabase();
            var str = redisDb.HashGetAsync(sessionId + "_" + SystemConfig.Channel, _cookieAccountId).Result;

            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public UserInfo GetUserInfo()
        {
            var sessionId = HttpContext.Current.Session.SessionID;
            var redisDb = _connectionMultiplexer.GetDatabase();
            var str = redisDb.HashGetAsync(sessionId + "_" + SystemConfig.Channel, _cookieAccountId).Result;
            if (!string.IsNullOrEmpty(str))
            {
                return JsonConvert.DeserializeObject<UserInfo>(str);
            }
            else
            {
                return null;
            }
        }


        public void RepeatSignIn(UserInfo user, string sessionId)
        {
             _memoryEventBusService.PublishUserSignIn(user, sessionId);
        }
    }
}