using NetCoreRabbitMQ.Domain.Entities;

namespace NetCoreRabbitMQ.Infrastructure.Repositories.Sessions
{
    public class SessionRepository : ISessionRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Session?> GetSession(Guid Id) => await _unitOfWork.SessionRepository.GetByID(Id);

        public async Task<Session> CreateSession(Session session)
        {
            await _unitOfWork.SessionRepository.Insert(session);
            return session;
        }

    }

}