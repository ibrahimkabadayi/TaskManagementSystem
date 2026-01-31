using Application.DTOs;
using AutoMapper;
using DomainLayer.Entities;
using Task = DomainLayer.Entities.Task;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.ProjectUsers,
                opt => opt.MapFrom(
                    src => src.ProjectUsers));
        
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.ProjectUsers,
                opt => opt.Ignore());
        

        CreateMap<Task, TaskDto>()
            .ForMember(dest => dest.CreatedBy, 
                opt => opt.MapFrom(src => src.CreatedBy.User))
            .ForMember(dest => dest.AssignedTo, 
                opt => opt.MapFrom(src => src.AssignedTo != null ? src.AssignedTo.User : null));
        
        CreateMap<TaskDto, Task>();
        
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.ProjectUsers,
                opt => opt.MapFrom(
                    src => src.ProjectUsers));
        
        CreateMap<ProjectDto, Project>()
            .ForMember(src => src.ProjectUsers, opt => opt.Ignore());

        CreateMap<ProjectUserDto, ProjectUser>();

        CreateMap<ProjectUser, ProjectUserDto>();
        
        CreateMap<Section, SectionDto>()
            .ForMember(dest => dest.TasksGroupDtos, opt => opt.MapFrom(src => src.TaskGroups));
        
        CreateMap<SectionDto, Section>()
            .ForMember(src => src.TaskGroups, opt => opt.Ignore());
        
        CreateMap<TaskGroup, TaskGroupDto>()
            .ForMember(dest => dest.TaskDtos,
                opt => opt.MapFrom(
                    src => src.Tasks));
        
        CreateMap<TaskGroupDto, TaskGroup>()
            .ForMember(src => src.Tasks, opt => opt.Ignore());
        
        CreateMap<Notification, NotificationDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        CreateMap<NotificationDto, Notification>();
    }
}