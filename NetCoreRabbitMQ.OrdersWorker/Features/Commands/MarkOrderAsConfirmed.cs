using MediatR;
using NetCoreRabbitMQ.Infrastructure.Repositories;
using NetCoreRabbitMQ.OrdersWorker.DTOs;

namespace NetCoreRabbitMQ.OrdersWorker.Features.Commands
{
    public record MarkOrderAsConfirmedCommand(OrderDTO order) : IRequest;

    public class MarkOrderAsConfirmedCommandHandler : IRequestHandler<MarkOrderAsConfirmedCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public MarkOrderAsConfirmedCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarkOrderAsConfirmedCommand request, CancellationToken cancellationToken)
        {

            var order = await _unitOfWork.OrderRepository.GetByID(request.order.Id);
            if (order == null)
            {
                return;
            }
            order.Status = Domain.Entities.OrderStatus.Confirmed;
            await _unitOfWork.Save();
        }
    }
}