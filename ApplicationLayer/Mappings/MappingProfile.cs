using Application.DTOs;
using AutoMapper;
using DataAccessLayer.Entities;
using Task = DataAccessLayer.Entities.Task;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.ProjectUserIds,
                opt => opt.MapFrom(
                    src => src.ProjectUsers.Select(x => x.UserId)));
        
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.ProjectUsers,
                opt => opt.Ignore());
        
        CreateMap<Task, TaskDto>();
        
        CreateMap<TaskDto, Task>();
        
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.ProjectUserIds,
                opt => opt.MapFrom(
                    src => src.ProjectUsers.Select(x => x.UserId)));
        
        CreateMap<ProjectDto, Project>()
            .ForMember(src => src.ProjectUsers, opt => opt.Ignore());
    }
}