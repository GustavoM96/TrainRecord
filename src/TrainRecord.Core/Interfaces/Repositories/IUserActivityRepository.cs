using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IUserActivityRepository : IRepositoryBase<UserActivity>
{
    IQueryable<Activity> GetActivitiesByUserId(Guid userId);
    IQueryable<UserActivity> GetAllRecordByUserAndActivityId(Guid userId, Guid activityId);
    Task<UserActivity?> GetRecordByUserAndActivityId(Guid userId, Guid activityId);
    Task<bool> DeleteRecordByUserAndActivityId(Guid userId, Guid activityId);
}
