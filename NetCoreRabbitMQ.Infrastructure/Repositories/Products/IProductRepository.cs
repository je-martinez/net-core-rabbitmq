using NetCoreRabbitMQ.Infrastructure.DTOs;

namespace NetCoreRabbitMQ.Infrastructure.Repositories.Products
{
    public interface IProductRepository
    {
        Task<List<ProductDTO>> GetProducts();
    }
}