using MediatR;
using NetCoreRabbitMQ.Application.DTOs.Products;
using NetCoreRabbitMQ.Application.Mapping.Products;
using NetCoreRabbitMQ.Domain.Entities;
using NetCoreRabbitMQ.Infrastructure.Repositories;

namespace NetCoreRabbitMQ.Application.UseCases.Products.Queries
{
    public record GetProductsQuery() : IRequest<List<ProductDTO>>;

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDTO>>
    {

        private readonly IUnitOfWork _unitOfWork;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductDTO>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.ProductRepository.Get(product => product.IsDeleted == false);
            return ProductMappers.ToProductDTOList(products);
        }
    }
}