using AutoMapper;
using TaskHub.Common.DTO.User;
using TaskHub.Dal.Entities;

namespace TaskHub.Bll.Mappers
{
    public class UserMapperProfile: Profile
    {
        public UserMapperProfile() 
        {
            CreateMap<RegisterModel, UserEntity>().ForMember(u => u.UserName, opt => opt.MapFrom(r => r.Username));
        }
    }
}
