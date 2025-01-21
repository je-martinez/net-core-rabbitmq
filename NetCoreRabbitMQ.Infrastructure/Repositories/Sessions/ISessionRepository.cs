using NetCoreRabbitMQ.Domain.Entities;

namespace NetCoreRabbitMQ.Infrastructure.Repositories.Sessions
{
    public interface ISessionRepository
    {
        Task<Session?> GetSession(Guid Id);
        Task<Session> CreateSession(Session session);
    }

}