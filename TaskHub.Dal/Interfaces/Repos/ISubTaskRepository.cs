using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Interfaces.Repos
{
    public interface ISubTaskRepository: IGenericRepository<SubTaskEntity, Guid>
    {
    }
}
