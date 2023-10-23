using Microsoft.EntityFrameworkCore;

namespace TaskHub.Dal.Context
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
    }
}
