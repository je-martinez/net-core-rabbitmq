using MediatR;
using NetCoreRabbitMQ.Application.DTOs.Orders;
using NetCoreRabbitMQ.Application.Mapping.Orders;
using NetCoreRabbitMQ.Application.Providers;
using NetCoreRabbitMQ.Application.Services;
using NetCoreRabbitMQ.Domain.ValueObjects;
using NetCoreRabbitMQ.Infrastructure.Repositories;

namespace NetCoreRabbitMQ.Application.UseCases.Orders
{
    public record CreateOrderCommand(CreateOrderDTO input) : IRequest<OrderDTO>;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderCalculationsService _orderCalculationsService;
        private readonly IBrokerProvider _brokerProvider;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IOrderCalculationsService orderCalculationsService, IBrokerProvider brokerProvider)
        {
            _unitOfWork = unitOfWork;
            _orderCalculationsService = orderCalculationsService;
            _brokerProvider = brokerProvider;
        }

        public async Task<OrderDTO> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {

            var calculatedOrder = await _orderCalculationsService.CalculateOrder(request.input);

            if (calculatedOrder == null)
            {
                throw new Exception("Something went wrong while calculating the order");
            }

            var newOrder = OrderMappers.ToOrder(calculatedOrder);
            newOrder.TrackingNumber = Guid.NewGuid().ToString();
            newOrder.OrderNumber = Guid.NewGuid().ToString();

            var orderCreated = await _unitOfWork.OrderRepository.Insert(newOrder);

            await _unitOfWork.Save();

            var mappedOrder = OrderMappers.ToOrderDTO(orderCreated);

            _brokerProvider.Publish<OrderDTO>(mappedOrder, BrokerExchanges.OrderExchange, BrokerTopics.OrderCreated);

            return mappedOrder;
        }
    }
}