using NetCoreRabbitMQ.Domain.Entities;

namespace NetCoreRabbitMQ.Infrastructure.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Product>> GetProducts() => await _unitOfWork.ProductRepository.Get(query => query.IsDeleted == false);

    }
}