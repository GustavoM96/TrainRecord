using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Interfaces.Repositories;

public interface IActivityRepository : IRepositoryBase<Activity>
{
    Task<bool> AnyByNameAsync(string name);
}
