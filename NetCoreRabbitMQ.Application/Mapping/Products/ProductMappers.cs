using AutoMapper;
using NetCoreRabbitMQ.Domain.Entities;
using NetCoreRabbitMQ.Application.DTOs.Products;

namespace NetCoreRabbitMQ.Application.Mapping.Products
{
    public static class ProductMappers
    {

        private static MapperConfiguration configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductDTO>().ReverseMap();
        });

        static Mapper mapper = new Mapper(configuration);
        public static ProductDTO ToProductDTO(this Product product)
        {
            return mapper.Map<ProductDTO>(product);
        }

        public static Product ToProduct(this ProductDTO productDTO)
        {
            return mapper.Map<Product>(productDTO);
        }

        public static List<ProductDTO> ToProductDTOList(this List<Product> products)
        {
            return mapper.Map<List<ProductDTO>>(products);
        }

        public static List<Product> ToProductList(this List<ProductDTO> productDTOs)
        {
            return mapper.Map<List<Product>>(productDTOs);
        }
    }
}