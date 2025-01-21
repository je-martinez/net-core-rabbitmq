using NetCoreRabbitMQ.Data.Context;
using NetCoreRabbitMQ.Domain.Entities;

namespace NetCoreRabbitMQ.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApiDbContext _context;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<OrderDetail> _orderDetailRepository;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<Session> _sessionRepository;

        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
        }

        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new GenericRepository<Order>(_context);
                }
                return _orderRepository;
            }
        }

        public GenericRepository<OrderDetail> OrderDetailRepository
        {
            get
            {
                if (_orderDetailRepository == null)
                {
                    _orderDetailRepository = new GenericRepository<OrderDetail>(_context);
                }
                return _orderDetailRepository;
            }
        }

        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new GenericRepository<Product>(_context);
                }
                return _productRepository;
            }
        }

        public GenericRepository<Session> SessionRepository
        {
            get
            {
                if (_sessionRepository == null)
                {
                    _sessionRepository = new GenericRepository<Session>(_context);
                }
                return _sessionRepository;
            }
        }

        public void BeginTransaction(CancellationToken cancellationToken = default)
        {
            _context.Database.BeginTransactionAsync(cancellationToken);
        }


        public void CommitTransaction(CancellationToken cancellationToken = default)
        {
            _context.Database.CommitTransactionAsync(cancellationToken);
        }

        public void RollbackTransaction(CancellationToken cancellationToken = default)
        {
            _context.Database.RollbackTransactionAsync(cancellationToken);
        }

        public async Task<int> Save(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}