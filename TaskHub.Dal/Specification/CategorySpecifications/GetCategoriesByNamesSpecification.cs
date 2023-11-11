using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.CategorySpecifications
{
    public class GetCategoriesByNamesSpecification: BaseSpecification<CategoryEntity>
    {
        public GetCategoriesByNamesSpecification(IEnumerable<string> categoryNames) 
        {
            Criteria = i => categoryNames.Contains(i.Name);
        }
    }
}
