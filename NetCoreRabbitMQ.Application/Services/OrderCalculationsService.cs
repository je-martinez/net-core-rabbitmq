using NetCoreRabbitMQ.Application.DTOs.Orders;
using NetCoreRabbitMQ.Infrastructure.Repositories;

namespace NetCoreRabbitMQ.Application.Services
{
    public class OrderCalculationsService : IOrderCalculationsService
    {
        IUnitOfWork _unitOfWork;
        public OrderCalculationsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDTO> CalculateOrder(CreateOrderDTO createOrderDTO)
        {

            var productIds = createOrderDTO.OrderDetails.Select(x => x.ProductId).ToList();

            var products = await _unitOfWork.ProductRepository.Get(x => productIds.Contains(x.Id));

            if (products.Count != productIds.Count)
            {
                throw new Exception("Some products are not found. Please check the products and try again.");
            }

            var order = new OrderDTO
            {
                Id = Guid.NewGuid(),
                OrderNumber = Guid.NewGuid().ToString(),
                OrderDate = DateTimeOffset.Now.ToUniversalTime(),
                SessionId = createOrderDTO.SessionId,
                OrderDetails = new List<OrderDetailsDTO>()
            };

            foreach (var orderDetail in createOrderDTO.OrderDetails)
            {
                decimal price = products.FirstOrDefault(x => x.Id == orderDetail.ProductId)!.Price;
                int quantity = orderDetail.Quantity;
                var orderDetailDTO = new OrderDetailsDTO
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = orderDetail.ProductId,
                    Quantity = orderDetail.Quantity,
                    Price = price,
                    Total = orderDetail.Quantity * price
                };
                order.OrderDetails.Add(orderDetailDTO);
            }

            order.SubTotal = order.OrderDetails.Sum(x => x.Total);
            order.Tax = order.SubTotal * 0.1m;
            order.Total = order.SubTotal + order.Tax;

            return order;
        }
    }
}