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

        CreateMap<ProjectUserDto, ProjectUser>();

        CreateMap<ProjectUser, ProjectUserDto>();
        
        CreateMap<Section, SectionDto>()
            .ForMember(dest => dest.TasksIds,
                opt => opt.MapFrom(
                    src => src.TaskGroups.Select(x => x.Id)));
        
        CreateMap<SectionDto, Section>()
            .ForMember(src => src.TaskGroups, opt => opt.Ignore());
        
        CreateMap<TaskGroup, TaskGroupDto>()
            .ForMember(dest => dest.TaskIds,
                opt => opt.MapFrom(
                    src => src.Tasks.Select(x => x.Id)));
        
        CreateMap<TaskGroupDto, TaskGroup>()
            .ForMember(src => src.Tasks, opt => opt.Ignore());
    }
}