using NetCoreRabbitMQ.Domain.Entities;

namespace NetCoreRabbitMQ.Infrastructure.Repositories.Products
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
    }
}