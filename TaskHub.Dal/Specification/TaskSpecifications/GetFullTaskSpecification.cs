using Ardalis.Specification;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.TaskSpecifications
{
    public class GetFullTaskSpecification: Specification<TaskEntity>
    {
        public GetFullTaskSpecification(Guid Id) 
        {
            Query
                .Where(x => x.Id == Id)
                .Include(i => i.Subtasks)
                .Include(i => i.AssignedUsers)
                .Include(i => i.Categories)
                .Include(i => i.ParentTask);
        }
    }
}
