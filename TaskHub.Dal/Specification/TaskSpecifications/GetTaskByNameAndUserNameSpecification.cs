using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.TaskSpecifications
{
    public class GetTaskByNameAndUserNameSpecification: BaseSpecification<TaskEntity>
    {
        public GetTaskByNameAndUserNameSpecification(string name, string userName) 
        {
            Criteria = i => i.Name == name && i.AssignedUsers.Any(u => u.UserName == userName);
            AddInclude(i => i.AssignedUsers);
            AddInclude(i => i.Categories);
        }
    }
}
