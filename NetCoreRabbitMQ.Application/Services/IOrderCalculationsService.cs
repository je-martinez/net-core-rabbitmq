using NetCoreRabbitMQ.Application.DTOs.Orders;

namespace NetCoreRabbitMQ.Application.Services
{
    public interface IOrderCalculationsService
    {
        public Task<OrderDTO> CalculateOrder(CreateOrderDTO createOrderDTO);
    }
}