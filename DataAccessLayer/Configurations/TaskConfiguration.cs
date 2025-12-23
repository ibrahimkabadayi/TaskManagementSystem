using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = DataAccessLayer.Entities.Task;

namespace DataAccessLayer.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.ToTable("Tasks");
        
        builder.HasKey(x => x.Id);
        
        builder
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);
        
        builder
            .Property(x => x.Description)
            .HasMaxLength(1000);
        
        builder
            .Property(x => x.CompletedDate)
            .HasColumnType("datetime");
        
        builder
            .Property(x=> x.StartDate)
            .IsRequired()
            .HasColumnType("datetime");

        builder
            .Property(x => x.State)
            .IsRequired();
        
        builder
            .Property(x => x.Priority)
            .IsRequired();
        
        builder
            .Property(x => x.DueDate)
            .HasColumnType("datetime");
        
        builder
            .HasOne(x => x.TaskGroup)
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.TaskGroupId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.AssignedTo)
            .WithMany()
            .HasForeignKey(x => x.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(x => x.FinishedBy)
            .WithMany()
            .HasForeignKey(x => x.FinishedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}