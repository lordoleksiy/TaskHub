using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context.ModelConfigurations
{
    public class TaskConfiguration<TKey> : IEntityTypeConfiguration<TaskEntity<TKey>> where TKey : IEquatable<TKey>
    {
        public void Configure(EntityTypeBuilder<TaskEntity<TKey>> builder)
        {
            builder.ToTable("Tasks");
            builder
                .HasKey(t => t.Id)
                .HasName("TaskId");

            builder
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(t => t.Title)
                .HasMaxLength(255);

            builder
                .Property(t => t.Description)
                .HasMaxLength(1000);

            builder
                .Property(t => t.DueDate)
                .IsRequired();

            builder
                .Property(t => t.Status)
                .IsRequired();


            builder
                .HasMany(t => t.Subtasks)
                .WithOne(t => t.ParentTask)
                .HasForeignKey(t => t.ParentTaskId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasMany(t => t.Reminders)
                .WithOne(r => r.Task)
                .HasForeignKey(r => r.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
