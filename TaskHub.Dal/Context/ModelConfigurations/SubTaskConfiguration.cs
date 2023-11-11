using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context.ModelConfigurations
{
    public class SubTaskConfiguration : IEntityTypeConfiguration<SubTaskEntity>
    {
        public void Configure(EntityTypeBuilder<SubTaskEntity> builder)
        {
            builder.ToTable("Subtasks");
            builder
                .HasKey(t => t.Id)
                .HasName("SubtaskId");

            builder
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(t => t.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .HasAlternateKey(t => t.Name);

            builder
                .Property(t => t.ParentTaskId)
                .IsRequired();

            builder
                .HasOne(t => t.Performer)
                .WithMany()
                .HasForeignKey(t => t.PerformerId);

            builder
                .Property(t => t.Status)
                .IsRequired();
        }
    }
}
