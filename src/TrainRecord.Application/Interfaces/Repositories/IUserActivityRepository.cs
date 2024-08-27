using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;

namespace TrainRecord.Application.Interfaces.Repositories;

public interface IUserActivityRepository : IRepositoryBase<UserActivity>
{
    IQueryable<Activity> GetActivitiesByUserId(EntityId<User> userId);
    IQueryable<UserActivity> GetAllRecordByUserAndActivityId(
        EntityId<User> userId,
        EntityId<Activity> activityId
    );
    IQueryable<UserActivity> GetAllRecordByUser(EntityId<User> userId, EntityId<User>? teacherId);
    Task<UserActivity?> GetRecordByUserAndActivityId(
        EntityId<User> userId,
        EntityId<Activity> activityId
    );
    Task<bool> DeleteRecordByUserAndActivityId(
        EntityId<User> userId,
        EntityId<Activity> activityId
    );
    Task<bool> DeleteRecordByTeacherId(
        EntityId<UserActivity> userActivityId,
        EntityId<User>? teacherId
    );
}
