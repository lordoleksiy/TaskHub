using AutoMapper;
using TaskHub.Common.DTO.Task;
using TaskHub.Dal.Entities;

namespace TaskHub.Bll.Mappers
{
    public class TaskMapperProfile: Profile
    {
        public TaskMapperProfile()
        {
            CreateMap<NewTaskDTO, TaskEntity>()
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => DateTime.Parse(src.DueDate)))
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedUsers, opt => opt.Ignore());

            CreateMap<UpdateTaskDTO, TaskEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedUsers, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                

            CreateMap<TaskEntity, TaskDTO>()
                .ForMember(dest => dest.AssignedUserNames, opt => opt.MapFrom(src => src.AssignedUsers.Select(u => u.UserName)))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)));
        }
    }
}
