using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class SectionConfiguration : IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.ToTable("Sections");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        
        builder.Property(x => x.ImageUrl).HasMaxLength(100).IsRequired();
        
        builder
            .HasOne(x => x.Project)
            .WithMany(x => x.Sections)
            .HasForeignKey(x => x.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasMany(x => x.TaskGroups)
            .WithOne(x => x.Section)
            .HasForeignKey(x => x.SectionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}