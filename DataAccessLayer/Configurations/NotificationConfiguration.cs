using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.CreatedDate).HasColumnType("datetime").IsRequired();
        
        builder.Property(x => x.Title).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(1000).IsRequired();

        builder.Property(x => x.IsRead).HasColumnType("bit").HasDefaultValue(false);
        
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();
        
        builder.Property(x => x.RelatedTaskId)
            .IsRequired(false);
    }
}