using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context.ModelConfigurations
{
    public class ReminderConfiguration : IEntityTypeConfiguration<ReminderEntity>
    {
        public void Configure(EntityTypeBuilder<ReminderEntity> builder)
        {
            builder.ToTable("Reminders");

            builder
                .HasKey(r => r.Id)
                .HasName("ReminderId");

            builder
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(r => r.UserId)
                .IsRequired();

            builder.
                Property(r => r.TaskId)
                .IsRequired();

            builder
                .Property(r => r.ReminderTime)
                .IsRequired();

            builder
                .Property(r => r.DueTime)
                .IsRequired();
        }
    }
}
