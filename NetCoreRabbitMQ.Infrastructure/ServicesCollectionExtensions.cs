using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreRabbitMQ.Data.Context;
using NetCoreRabbitMQ.Infrastructure.Repositories;
using NetCoreRabbitMQ.Infrastructure.Repositories.Products;

namespace NetCoreRabbitMQ.Infrastructure.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApiDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("NetCoreRabbitMQ.Data")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}