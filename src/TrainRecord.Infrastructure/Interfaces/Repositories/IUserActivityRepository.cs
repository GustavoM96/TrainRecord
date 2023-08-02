using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;

namespace TrainRecord.Infrastructure.Interfaces.Repositories;

public interface IUserActivityRepository : IRepositoryBase<UserActivity>
{
    IQueryable<Activity> GetActivitiesByUserId(EntityId<User> userId);
    IQueryable<UserActivity> GetAllRecordByUserAndActivityId(
        EntityId<User> userId,
        EntityId<Activity> activityId
    );
    Task<UserActivity?> GetRecordByUserAndActivityId(
        EntityId<User> userId,
        EntityId<Activity> activityId
    );
    Task<bool> DeleteRecordByUserAndActivityId(
        EntityId<User> userId,
        EntityId<Activity> activityId
    );
}
