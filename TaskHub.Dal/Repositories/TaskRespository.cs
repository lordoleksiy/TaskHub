using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class TaskRespository : GenericRepository<TaskEntity, Guid>, ITaskRepository
    {
        public TaskRespository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
