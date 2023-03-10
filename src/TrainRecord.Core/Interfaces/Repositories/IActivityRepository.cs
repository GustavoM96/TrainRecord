using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IActivityRepository : IRepositoryBase<Activity>
{
    Task<bool> AnyByNameAsync(string name);
}
