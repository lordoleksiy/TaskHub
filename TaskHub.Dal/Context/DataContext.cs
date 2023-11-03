using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Context.ModelConfigurations;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<UserEntity<Guid>> Users { get; set; }
        public DbSet<TaskEntity<Guid>> Tasks { get; set; }
        public DbSet<ReminderEntity<Guid>> Reminders { get; set; }
        public DbSet<CategoryEntity<Guid>> Categories { get; set; }

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
