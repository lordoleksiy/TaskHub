using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Context.ModelConfigurations;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<ReminderEntity> Reminders { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration<Guid>());
            modelBuilder.ApplyConfiguration(new ReminderConfiguration<Guid>());
            modelBuilder.ApplyConfiguration(new TaskConfiguration<Guid>());
            modelBuilder.ApplyConfiguration(new UserConfiguration<Guid>());
            base.OnModelCreating(modelBuilder);
        }
    }
}
