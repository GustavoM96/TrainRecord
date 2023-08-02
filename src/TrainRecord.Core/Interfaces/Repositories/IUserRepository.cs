using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Enum;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> AnyByEmailAsync(string email);
    Task<bool> UpdatePasswordById(string password, EntityId<User> userId);
}
