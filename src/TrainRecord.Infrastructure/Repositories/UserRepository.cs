using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Repositories;
using TrainRecord.Infrastructure.Interfaces;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<bool> AnyByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }
    }
}
