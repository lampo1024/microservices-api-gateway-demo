using Consul;

namespace Micro.Services.Orders
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IConsulClient>(consul => new ConsulClient(config =>
             {
                 config.Address = new Uri(configuration["Consul:Host"]);
             }));
            return services;
        }
    }
}
