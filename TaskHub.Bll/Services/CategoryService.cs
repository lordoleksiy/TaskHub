using AutoMapper;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services.Abstract;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.CategorySpecifications;

namespace TaskHub.Bll.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<ICollection<CategoryEntity>?> UpdateCategoriesAsync(ICollection<string>? categories)
        {
            if (categories == null || !categories.Any())
            {
                return null;
            }
            var categoriesInDB = (await _unitOfWork.CategoryRepository.GetAsync(new GetCategoriesByNamesSpecification(categories))).ToList();
            var newCategories = categories.Except(categoriesInDB.Select(u => u.Name));
            categoriesInDB.AddRange(newCategories.Select(c => new CategoryEntity { Name = c }));

            return categoriesInDB;
        }
    }
}
