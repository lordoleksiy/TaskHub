using Ardalis.Specification;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.UserSpecifications
{
    public class GetUserByUserNameSpecification: Specification<UserEntity>
    {
        public GetUserByUserNameSpecification(string userName) 
        {
            Query.Where(i => i.UserName == userName);
        }
    }
}
