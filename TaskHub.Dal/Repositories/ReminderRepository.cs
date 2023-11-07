using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class ReminderRepository : GenericRepository<ReminderEntity, Guid>, IReminderRepository
    {
        public ReminderRepository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
