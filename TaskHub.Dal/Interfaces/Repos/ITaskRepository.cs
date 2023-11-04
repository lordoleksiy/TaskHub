using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Interfaces
{
    public interface ITaskRepository: IGenericRepository<TaskEntity, Guid>
    {
        
    }
}
