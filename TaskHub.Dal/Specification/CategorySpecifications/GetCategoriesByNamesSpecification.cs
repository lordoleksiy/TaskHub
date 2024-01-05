using Ardalis.Specification;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.CategorySpecifications
{
    public class GetCategoriesByNamesSpecification: Specification<CategoryEntity>
    {
        public GetCategoriesByNamesSpecification(IEnumerable<string> categoryNames) 
        {
            Query.Where(i => categoryNames.Contains(i.Name));
        }
    }
}
