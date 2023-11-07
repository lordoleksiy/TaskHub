using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class UserRepository : GenericRepository<UserEntity, Guid>, IUserRepository
    {
        public UserRepository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
