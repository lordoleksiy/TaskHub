using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class TaskRespository : GenericRepository<TaskEntity, Guid>, ITaskRepository
    {
        public TaskRespository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
