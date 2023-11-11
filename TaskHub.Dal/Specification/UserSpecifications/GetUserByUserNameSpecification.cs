using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.UserSpecifications
{
    public class GetUserByUserNameSpecification: BaseSpecification<UserEntity>
    {
        public GetUserByUserNameSpecification(string userName) 
        {
            Criteria = i => i.UserName == userName;
        }
    }
}
