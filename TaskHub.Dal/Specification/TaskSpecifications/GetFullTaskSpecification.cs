using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.TaskSpecifications
{
    public class GetFullTaskSpecification: BaseSpecification<TaskEntity>
    {
        public GetFullTaskSpecification(Guid Id) 
        {
            Criteria = i => i.Id == Id;
            AddInclude(i => i.Subtasks);
            AddInclude(i => i.AssignedUsers);
            AddInclude(i => i.Categories);
            AddInclude(i => i.ParentTask);
        }
    }
}
