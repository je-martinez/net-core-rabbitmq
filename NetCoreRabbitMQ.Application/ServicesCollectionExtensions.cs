using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreRabbitMQ.Application.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}