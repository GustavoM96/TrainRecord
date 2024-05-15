using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TrainRecord.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> AnyByEmailAsync(string email)
    {
        return await AnyAsync(u => u.Email == email);
    }

    public async Task<bool> UpdatePasswordById(string password, EntityId<User> userId)
    {
        return await UpdateById(u => u.SetProperty(u => u.Password, u => password), userId);
    }

    public async Task<bool> UpdatePasswordByEmail(string email, string hashedPassword)
    {
        return await Where(user => user.Email == email)
                .ExecuteUpdateAsync(u => u.SetProperty(u => u.Password, u => hashedPassword)) > 0;
    }
}
