using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.TaskSpecifications
{
    public class GetTaskByNameSpecification: BaseSpecification<TaskEntity>
    {
        public GetTaskByNameSpecification(string name) 
        {
            Criteria = i => i.Name == name;
        }
    }
}
