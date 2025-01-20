using NetCoreRabbitMQ.Data.Context;
using NetCoreRabbitMQ.Infrastructure.DTOs;
using NetCoreRabbitMQ.Infrastructure.Mapping;
using NetCoreRabbitMQ.Infrastructure.Repositories;

namespace NetCoreRabbitMQ.Infrastructure.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApiDbContext _context;

        public ProductRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDTO>> GetProducts()
        {

            using (var _unitOfWork = new UnitOfWork(_context))
            {
                var products = await _unitOfWork.ProductRepository.Get(query => query.IsDeleted == false);
                return ProductMappers.ToProductDTOList(products);
            }
        }
    }
}