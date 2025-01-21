using NetCoreRabbitMQ.Infrastructure.DTOs;
using NetCoreRabbitMQ.Infrastructure.Mapping;

namespace NetCoreRabbitMQ.Infrastructure.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductDTO>> GetProducts()
        {
            var products = await _unitOfWork.ProductRepository.Get(query => query.IsDeleted == false);
            return ProductMappers.ToProductDTOList(products);
        }
    }
}