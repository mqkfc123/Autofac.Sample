using Dragon.Core;
using Dragon.Infrastructure.Redis;
using Newtonsoft.Json;
using Sample.OAuth.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sample.OAuth.Services
{

    public interface ITokenService : IDependency
    {
        Task<TokenModel> GenerateToken(AuthorizationModel authorization);

        Task<bool> CheckToken(string accessToken);
    }

    public class TokenService : ITokenService, IDisposable
    {
        private readonly IDatabase _database;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly int _channelId;

        public TokenService(IRedisFactory redisFactory)
        {
            _connectionMultiplexer = redisFactory.CreateConnection();
            _database = _connectionMultiplexer.GetDatabase();
            _channelId = 0019;
        }

        #region Implementation of ITokenService

        private string GetHashId()
        {
            return $"oauth_tokens_{_channelId}";
        }

        public async Task<TokenModel> GenerateToken(AuthorizationModel authorization)
        {
            var token = TokenModel.Create(authorization);
            var content = JsonConvert.SerializeObject(token);
            await _database.HashSetAsync(GetHashId() + authorization.UserName, authorization.UserName, content);
            return token;
        }

        public async Task<bool> CheckToken(string accessToken)
        {
            accessToken = accessToken.Split('$')[0];
            var userName = TokenModel.GetUserNameByToken(accessToken);

            var content = await _database.HashGetAsync(GetHashId() + userName, userName);
            var token = JsonConvert.DeserializeObject<TokenModel>(content);

            return token?.Check(accessToken) ?? false;
        }

        #endregion Implementation of ITokenService

        #region Implementation of IDisposable

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _connectionMultiplexer.Dispose();
        }

        #endregion Implementation of IDisposable
    }
}