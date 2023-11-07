using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.UserSpecifications
{
    public class FindByNameOrEmailSpecification: BaseSpecification<UserEntity>
    {
        public FindByNameOrEmailSpecification(string? UserName, string? Email) 
        {
            Criteria = i => (UserName != null && Email != null) ?
                    (i.UserName == UserName && i.Email == Email) :
                    (UserName != null ? i.UserName == UserName : i.Email == Email);
        }
    }
}
