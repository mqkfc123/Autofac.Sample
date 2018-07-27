using StackExchange.Redis;
using System.Threading.Tasks;

namespace Dragon.Infrastructure.Redis
{
    public interface IRedisFactory
    {
        Task<IConnectionMultiplexer> CreateConnectionAsync();

        IConnectionMultiplexer CreateConnection();
    }

    public class RedisFactory : IRedisFactory
    {
        private readonly string _connectionString;
        private static IConnectionMultiplexer _connectionMultiplexer;
        public RedisFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Implementation of IRedisFactory

        public async Task<IConnectionMultiplexer> CreateConnectionAsync()
        {
            var connectionString = _connectionString;
            if (_connectionMultiplexer != null)
            {
                return _connectionMultiplexer;
            }
            _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(connectionString);
            return _connectionMultiplexer;
        }

        public IConnectionMultiplexer CreateConnection()
        {
            var connectionString = _connectionString;
            if (_connectionMultiplexer != null)
            {
                return _connectionMultiplexer;
            }
            _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            return _connectionMultiplexer;

        }

        #endregion Implementation of IRedisFactory
    }
}