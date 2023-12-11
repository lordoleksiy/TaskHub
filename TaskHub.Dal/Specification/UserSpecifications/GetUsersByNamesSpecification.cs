using Ardalis.Specification;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.UserSpecifications
{
    public class GetUsersByNamesSpecification: Specification<UserEntity>
    {
        public GetUsersByNamesSpecification(IEnumerable<string> userNames) 
        {
            Query.Where(user => userNames.Contains(user.UserName));
        }
    }
}
