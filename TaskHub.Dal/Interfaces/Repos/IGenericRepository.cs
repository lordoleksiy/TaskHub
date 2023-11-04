﻿namespace TaskHub.Dal.Interfaces
{
    public interface IGenericRepository<TEntity, TKey>
    {
        Task<TEntity> GetByIdAsync(TKey key);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        Task<IEnumerable<TEntity>> GetAsync(ISpecification<TEntity>? specification = null);
    }
}
