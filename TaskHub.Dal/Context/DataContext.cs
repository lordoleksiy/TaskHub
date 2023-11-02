using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Context
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        DbSet<UserEntity<Guid>> Users { get; set; }
        DbSet<TaskEntity<Guid>> Tasks { get; set; }
        DbSet<ReminderEntity<Guid>> Reminders { get; set; }
        DbSet<CategoryEntity<Guid>> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
