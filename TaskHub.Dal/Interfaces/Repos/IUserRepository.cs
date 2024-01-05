using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Interfaces
{
    public interface IUserRepository: IGenericRepository<UserEntity, Guid>
    {
    }
}
