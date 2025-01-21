using MediatR;
using NetCoreRabbitMQ.Application.DTOs.Sessions;
using NetCoreRabbitMQ.Application.Mapping;
using NetCoreRabbitMQ.Infrastructure.Repositories;

namespace NetCoreRabbitMQ.Application.UseCases.Session.Commands
{

    public record CreateSessionCommand(CreateSessionDTO session) : IRequest<SessionDTO>;

    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, SessionDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateSessionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<SessionDTO> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            //Check if already has an existing Session
            var existingSession = await _unitOfWork.SessionRepository.GetBy(x => x.Email == request.session.Email);
            if (existingSession != null)
            {
                return SessionMappers.ToSessionDTO(existingSession);
            }
            //Create new session
            var newSession = await _unitOfWork.SessionRepository.Insert(SessionMappers.ToSession(request.session));
            await _unitOfWork.Save();
            return SessionMappers.ToSessionDTO(newSession);
        }
    }
}