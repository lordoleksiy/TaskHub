using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification;

namespace TaskHub.Dal.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>
    {
        private readonly DataContext _dbContext;
        private readonly DbSet<TEntity> _entities;
        public GenericRepository(DataContext dbContext) 
        {
            _dbContext = dbContext;
            _entities = dbContext.Set<TEntity>();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(ISpecification<TEntity>? specification = null)
        {
            var res = specification != null ? _entities.Specify(specification) : _entities;
            return await res.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TKey key)
        {
            return await _entities.FindAsync(key);
        }


        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _entities.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
