using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> AnyByEmailAsync(string email);
}
