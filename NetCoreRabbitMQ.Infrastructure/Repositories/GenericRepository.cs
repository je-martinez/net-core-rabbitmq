using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NetCoreRabbitMQ.Data.Context;


namespace NetCoreRabbitMQ.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        protected ApiDbContext _context;
        protected DbSet<TEntity> _dbSet;

        public GenericRepository(ApiDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual Task<List<TEntity>> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
        )
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToListAsync();
            }
            else
            {
                return query.ToListAsync();
            }
        }

        public virtual Task<TEntity?> GetBy(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = ""
        )
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query.FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity?> GetByID(object id, string includeProperties = "")
        {
            return await _dbSet.FindAsync(id);
        }

        public async virtual Task<TEntity> Insert(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async virtual void Delete(object id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

    }
}