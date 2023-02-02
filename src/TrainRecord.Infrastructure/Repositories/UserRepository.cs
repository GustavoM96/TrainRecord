using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> AnyByEmailAsync(string email)
        {
            return await AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UpdatePasswordById(string password, Guid userId)
        {
            return await UpdateById(u => u.SetProperty(u => u.Password, u => password), userId);
        }

        public IQueryable<User> GetAllByRole(Role? role)
        {
            if (role is null)
            {
                return AsQueryable();
            }

            return Where(u => u.Role == role.Value);
        }
    }
}
