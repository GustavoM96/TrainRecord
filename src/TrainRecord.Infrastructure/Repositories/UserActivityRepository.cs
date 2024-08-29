using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories;

public class UserActivityRepository : RepositoryBase<UserActivity>, IUserActivityRepository
{
    public UserActivityRepository(AppDbContext context)
        : base(context) { }

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

    public IQueryable<UserActivity> GetAllRecordByUser(
        EntityId<User> userId,
        EntityId<User>? teacherId
    )
    {
        return Where(ua => ua.UserId == userId)
            .WhereIf(teacherId is not null, ua => ua.TeacherId == teacherId!.Value);
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

    public async Task<bool> DeleteRecordByTeacherId(
        EntityId<UserActivity> userActivityId,
        EntityId<User> teacherId
    )
    {
        return await Where(ua => ua.Id == userActivityId && ua.TeacherId == teacherId)
                .ExecuteDeleteAsync() > 0;
    }

    public async Task<bool> DeleteRecordByStudentId(
        EntityId<UserActivity> userActivityId,
        EntityId<User> studentId
    )
    {
        return await Where(ua => ua.Id == userActivityId && ua.UserId == studentId)
                .ExecuteDeleteAsync() > 0;
    }
}
