using Ardalis.Specification;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.TaskSpecifications
{
    public class GetTasksByUserNameSpecification: Specification<TaskEntity>
    {
        public GetTasksByUserNameSpecification(string userName) 
        {
            Query
                .Where(i => i.AssignedUsers.Any(u => u.UserName == userName))
                .Include(i => i.AssignedUsers)
                .Include(i => i.Categories);
        }
    }
}
