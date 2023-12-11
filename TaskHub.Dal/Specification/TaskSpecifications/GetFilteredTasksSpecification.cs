using Ardalis.Specification;
using System.Linq;
using TaskHub.Common.QueryParams;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.TaskSpecifications
{
    public class GetFilteredTasksSpecification: Specification<TaskEntity>
    {
        public GetFilteredTasksSpecification(string userName, TaskQueryParams filter)
        {
            Query
                .Where(i => i.AssignedUsers.Any(u => u.UserName == userName))
                .Where(i => filter.Status.Contains(i.Status), condition: filter.Status != null)
                .Where(i => i.Categories.Any(c => filter.Category.Contains(c.Name)), condition: filter.Category != null)
                .Include(i => i.AssignedUsers)
                .Include(i => i.Categories);
        }

    }
}
