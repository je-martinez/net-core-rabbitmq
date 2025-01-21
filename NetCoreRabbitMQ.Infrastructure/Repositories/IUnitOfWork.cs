using NetCoreRabbitMQ.Domain.Entities;

namespace NetCoreRabbitMQ.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        GenericRepository<Order> OrderRepository { get; }
        GenericRepository<OrderDetail> OrderDetailRepository { get; }
        GenericRepository<Product> ProductRepository { get; }
        GenericRepository<Session> SessionRepository { get; }

        void BeginTransaction(CancellationToken cancellationToken = default);
        void CommitTransaction(CancellationToken cancellationToken = default);
        void RollbackTransaction(CancellationToken cancellationToken = default);
        Task<int> Save(CancellationToken cancellationToken = default);
    }
}