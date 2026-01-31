using DataAccessLayer.Context;
using DataAccessLayer.Implementations;
using DomainLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IProjectUserRepository, ProjectUserRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<ITaskGroupRepository, TaskGroupRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
            
        return services;
    }
}