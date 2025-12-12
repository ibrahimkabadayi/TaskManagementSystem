using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class ProjectUserConfiguration : IEntityTypeConfiguration<ProjectUser>
{
    public void Configure(EntityTypeBuilder<ProjectUser> builder)
    {
        builder.ToTable("ProjectUsers");
        
        builder.HasKey(x => x.Id);
        
        builder
            .Property(x => x.Role)
            .IsRequired();
        
        builder
            .Property(x => x.IsActive)
            .IsRequired();
        
        builder
            .Property(x => x.JoinedDate)
            .IsRequired()
            .HasColumnType("datetime");
        
        builder
            .HasOne(x => x.Project)
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}