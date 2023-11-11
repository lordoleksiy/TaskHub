using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.UserSpecifications
{
    public class GetUsersByNamesSpecification: BaseSpecification<UserEntity>
    {
        public GetUsersByNamesSpecification(IEnumerable<string> userNames) 
        {
            Criteria = user => userNames.Contains(user.UserName);
        }
    }
}
