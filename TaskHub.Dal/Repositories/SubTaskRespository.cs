using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces.Repos;

namespace TaskHub.Dal.Repositories
{
    public class SubTaskRespository : GenericRepository<SubTaskEntity, Guid>, ISubTaskRepository
    {
        public SubTaskRespository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
