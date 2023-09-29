using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories;

public class UserActivityRepository : RepositoryBase<UserActivity>, IUserActivityRepository
{
    public UserActivityRepository(AppDbContext context) : base(context) { }

    public IQueryable<Activity> GetActivitiesByUserId(EntityId<User> userId)
    {
        var dbSetActivity = GetOtherDbSet<Activity>();

        return Where(ua => ua.UserId == userId)
            .Join(dbSetActivity, ua => ua.ActivityId, a => a.Id, (_, a) => a)
            .Distinct();
    }

    public IQueryable<UserActivity> GetAllRecordByUserAndActivityId(
        EntityId<User> userId,
        EntityId<Activity> activityId
    )
    {
        return Where(ua => ua.UserId == userId && ua.ActivityId == activityId);
    }

    public async Task<UserActivity?> GetRecordByUserAndActivityId(
        EntityId<User> userId,
        EntityId<Activity> activityId
    )
    {
        return await SingleOrDefaultAsync(ua => ua.UserId == userId && ua.ActivityId == activityId);
    }

    public async Task<bool> DeleteRecordByUserAndActivityId(
        EntityId<User> userId,
        EntityId<Activity> activityId
    )
    {
        return await Delete(ua => ua.UserId == userId && ua.ActivityId == activityId);
    }
}
