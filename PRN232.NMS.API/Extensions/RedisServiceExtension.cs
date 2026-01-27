using StackExchange.Redis;

namespace PRN232.NMS.API.Extensions
{
    public static class RedisServiceExtension
    {
        public static void RedisService(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("Redis");

            services.AddSingleton<IConnectionMultiplexer>(
                sp => ConnectionMultiplexer.Connect(connection!));

        }
    }
}
