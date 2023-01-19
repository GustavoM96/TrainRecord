using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IUserActivityRepository : IRepositoryBase<UserActivity>
{
    IQueryable<UserActivity> GetAllByUserId(Guid userId);
}
