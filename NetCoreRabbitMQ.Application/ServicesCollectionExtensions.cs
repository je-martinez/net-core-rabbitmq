using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreRabbitMQ.Application.Providers;
using NetCoreRabbitMQ.Application.Services;

namespace NetCoreRabbitMQ.Application.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IBrokerProvider, BrokerProvider>();
            services.AddScoped<IOrderCalculationsService, OrderCalculationsService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}