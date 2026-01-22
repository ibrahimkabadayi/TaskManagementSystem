using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class TaskGroupConfiguration : IEntityTypeConfiguration<TaskGroup>
{
    public void Configure(EntityTypeBuilder<TaskGroup> builder)
    {
        builder.ToTable("TaskGroups");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        
        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Tasks)
            .WithOne(x  => x.TaskGroup)
            .HasForeignKey(x => x.TaskGroupId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Section)
            .WithMany(x => x.TaskGroups)
            .HasForeignKey(x => x.SectionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}