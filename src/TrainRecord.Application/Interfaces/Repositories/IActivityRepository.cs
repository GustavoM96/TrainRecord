using TrainRecord.Core.Entities;

namespace TrainRecord.Application.Interfaces.Repositories;

public interface IActivityRepository : IRepositoryBase<Activity>
{
    Task<bool> AnyByNameAsync(string name);
}
