using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class ReminderRepository : GenericRepository<ReminderEntity, Guid>, IReminderRepository
    {
        public ReminderRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
