using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class CategoryRepository : GenericRepository<CategoryEntity, Guid>, ICategoryRepository
    {
        public CategoryRepository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
