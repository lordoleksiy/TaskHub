namespace TaskHub.Dal.Interfaces
{
    public interface IGenericRepository<TEntity, TKey>
    {
        TEntity Get(TKey key);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> GetAll();

    }
}
