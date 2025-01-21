using AutoMapper;
using NetCoreRabbitMQ.Domain.Entities;
using NetCoreRabbitMQ.Application.DTOs.Orders;

namespace NetCoreRabbitMQ.Application.Mapping.Orders
{
    public static class OrderMappers
    {

        private static MapperConfiguration configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Order, OrderDTO>().ReverseMap();
            cfg.CreateMap<OrderDetail, OrderDetailsDTO>().ReverseMap();

        });

        static Mapper mapper = new Mapper(configuration);
        public static OrderDTO ToOrderDTO(this Order Order)
        {
            return mapper.Map<OrderDTO>(Order);
        }

        public static Order ToOrder(this OrderDTO OrderDTO)
        {
            return mapper.Map<Order>(OrderDTO);
        }

        public static List<OrderDTO> ToOrderDTOList(this List<Order> Orders)
        {
            return mapper.Map<List<OrderDTO>>(Orders);
        }

        public static List<Order> ToOrderList(this List<OrderDTO> OrderDTOs)
        {
            return mapper.Map<List<Order>>(OrderDTOs);
        }
    }
}