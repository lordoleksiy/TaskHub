using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.TaskSpecifications
{
    public class GetTasksByUserNameSpecification: BaseSpecification<TaskEntity>
    {
        public GetTasksByUserNameSpecification(string userName) 
        {
            Criteria = i => i.AssignedUsers.Any(u => u.UserName == userName);
            AddInclude(i => i.AssignedUsers);
            AddInclude(i => i.Categories);
        }
    }
}
