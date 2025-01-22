using MediatR;
using NetCoreRabbitMQ.Infrastructure.Repositories;
using NetCoreRabbitMQ.OrdersWorker.DTOs;

namespace NetCoreRabbitMQ.OrdersWorker.Features.Commands
{
    public record MarkOrderAsConfirmedCommand(OrderDTO order) : IRequest;

    public class MarkOrderAsConfirmedCommandHandler : IRequestHandler<MarkOrderAsConfirmedCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MarkOrderAsConfirmedCommandHandler> _logger;
        public MarkOrderAsConfirmedCommandHandler(IUnitOfWork unitOfWork, ILogger<MarkOrderAsConfirmedCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(MarkOrderAsConfirmedCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Marking order {request.order.Id} as confirmed...Started at {DateTime.Now.ToShortTimeString()}...");

            var order = await _unitOfWork.OrderRepository.GetByID(request.order.Id);

            if (order == null)
            {
                return;
            }

            order.Status = Domain.Entities.OrderStatus.Confirmed;

            await _unitOfWork.Save();
            _logger.LogInformation($"Marking order {request.order.Id} as confirmed...Finished at {DateTime.Now.ToShortTimeString()}...");
        }
    }
}