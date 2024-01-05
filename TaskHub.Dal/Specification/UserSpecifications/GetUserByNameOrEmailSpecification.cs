using Ardalis.Specification;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.UserSpecifications
{
    public class GetUserByNameOrEmailSpecification: Specification<UserEntity>
    {
        public GetUserByNameOrEmailSpecification(string UserName, string Email) 
        {
            Query.Where(i => i.UserName == UserName || i.Email == Email);
        }
    }
}
