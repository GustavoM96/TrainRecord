using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Repositories;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
    }
}
