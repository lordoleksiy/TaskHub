using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Interfaces
{
    public interface ICategoryRepository: IGenericRepository<CategoryEntity, Guid>
    {
    }
}
