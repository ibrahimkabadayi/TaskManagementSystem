using DataAccessLayer.Configurations;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Task = DomainLayer.Entities.Task;

namespace DataAccessLayer.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<TaskGroup> TaskGroups { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ProjectInvitation> ProjectInvitations { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectUserConfiguration());
        modelBuilder.ApplyConfiguration(new SectionConfiguration());
        modelBuilder.ApplyConfiguration(new TaskGroupConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectInvitationConfiguration());
    }
}