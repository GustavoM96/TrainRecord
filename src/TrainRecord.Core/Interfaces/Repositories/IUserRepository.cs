using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User> GetByEmail(string email);
}
