using Dragon.Core;
using Autofac;

namespace Dragon.Infrastructure.Redis
{
    public static class RedisKernelBuilderExtensions
    {
        public static void UserRedis(this ICoreBuilder coreBuilder, string connectionString)
        {
            coreBuilder.OnStarting(builder =>
            {
                builder.RegisterInstance(new RedisFactory(connectionString)).As<IRedisFactory>();
            });
        }
    }
}