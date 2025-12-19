using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        
        builder.HasKey(x => x.Id);
        
        builder
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(1000);
        
        builder
            .Property(x => x.StartDate)
            .IsRequired()
            .HasColumnType("datetime");
        
        builder
            .Property(x => x.EndDate)
            .HasColumnType("datetime");

        builder
            .HasMany(x => x.ProjectUsers)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId)
            .HasPrincipalKey(x => x.Id);
        
        builder
            .HasMany(x => x.Sections)
            .WithOne(x => x.Project)
            .HasForeignKey(x => x.ProjectId)
            .HasPrincipalKey(x => x.Id);
    }
}