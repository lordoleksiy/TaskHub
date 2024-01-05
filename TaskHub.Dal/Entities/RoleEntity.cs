using Microsoft.AspNetCore.Identity;

namespace TaskHub.Dal.Entities
{
    public class RoleEntity: IdentityRole<Guid>, IBaseEntity<Guid>
    {
    }
}
