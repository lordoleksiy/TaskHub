using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context.ModelConfigurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.ToTable("Notifications");

            builder
                .HasKey(r => r.Id)
                .HasName("ReminderId");

            builder
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(r => r.UserId)
                .IsRequired();
            
            builder.Property(r => r.TaskId)
                .IsRequired();

            builder
                .Property(r => r.DueDate)
                .IsRequired();
        }
    }
}
