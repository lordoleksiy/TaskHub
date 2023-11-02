using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _entities;
        public GenericRepository(DbContext dbContext) 
        {
            _dbContext = dbContext;
            _entities = dbContext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public TEntity Get(TKey key)
        {
            return _entities.Find(key);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities;
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _entities.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
