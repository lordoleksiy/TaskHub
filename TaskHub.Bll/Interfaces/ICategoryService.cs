using TaskHub.Dal.Entities;

namespace TaskHub.Bll.Interfaces
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryEntity>?> UpdateCategoriesAsync(ICollection<string>? categories);
    }
}
