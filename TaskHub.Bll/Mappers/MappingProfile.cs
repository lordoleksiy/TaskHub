using AutoMapper;
using TaskHub.Common.DTO.User;
using TaskHub.Dal.Entities;

namespace TaskHub.Bll.Mappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserEntity, RegisterModel>().ReverseMap();
            CreateMap<UserEntity, LoginModel>().ReverseMap();
        }
    }
}
