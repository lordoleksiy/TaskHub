using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.UserSpecifications
{
    public class GetUserByNameOrEmailSpecification: BaseSpecification<UserEntity>
    {
        public GetUserByNameOrEmailSpecification(string UserName, string Email) 
        {
            Criteria = i => i.UserName == UserName || i.Email == Email;
        }
    }
}
