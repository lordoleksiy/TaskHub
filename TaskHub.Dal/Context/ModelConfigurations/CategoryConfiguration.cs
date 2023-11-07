using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context.ModelConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("Categories");

            builder
                .HasKey(c => c.Id)
                .HasName("CategoryId");

            builder
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder
                .Property(c => c.Description)
                .HasMaxLength(1000);

            builder
                .HasMany(c => c.Tasks)
                .WithMany(t => t.Categories)
                .UsingEntity("TaskCategoriesJoinTable");
        }
    }
}
