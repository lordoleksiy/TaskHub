using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Interfaces
{
    public interface IReminderRepository: IGenericRepository<ReminderEntity, Guid>
    {
    }
}
