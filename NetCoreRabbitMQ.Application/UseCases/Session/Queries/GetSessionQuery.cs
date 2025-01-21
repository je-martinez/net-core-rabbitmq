using MediatR;
using NetCoreRabbitMQ.Application.DTOs.Sessions;
using NetCoreRabbitMQ.Application.Mapping;
using NetCoreRabbitMQ.Infrastructure.Repositories;

namespace NetCoreRabbitMQ.Application.UseCases.Session.Queries
{

    public record GetSessionQuery(Guid Id) : IRequest<SessionDTO>;

    public class GetSessionQueryHandler : IRequestHandler<GetSessionQuery, SessionDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetSessionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<SessionDTO?> Handle(GetSessionQuery request, CancellationToken cancellationToken)
        {
            //Check if already has an existing Session
            var existingSession = await _unitOfWork.SessionRepository.GetByID(request.Id);
            if (existingSession != null)
            {
                return SessionMappers.ToSessionDTO(existingSession);
            }
            return null;
        }
    }
}