using TrainRecord.Core.Entities;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories
{
    public class UserActivityRepository : RepositoryBase<UserActivity>, IUserActivityRepository
    {
        public UserActivityRepository(AppDbContext context) : base(context) { }

        public IQueryable<Activity> GetActivitiesByUserId(Guid userId)
        {
            var dbSetActivity = GetOtherDbSet<Activity>();

            return Where(ua => ua.UserId == userId)
                .Join(dbSetActivity, ua => ua.ActivityId, a => a.Id, (_, a) => a)
                .Distinct();
        }

        public IQueryable<UserActivity> GetAllRecordByUserAndActivityId(
            Guid userId,
            Guid activityId
        )
        {
            return Where(ua => ua.UserId == userId && ua.ActivityId == activityId);
        }

        public async Task<UserActivity?> GetRecordByUserAndActivityId(Guid userId, Guid activityId)
        {
            return await SingleOrDefaultAsync(
                ua => ua.UserId == userId && ua.ActivityId == activityId
            );
        }

        public async Task<bool> DeleteRecordByUserAndActivityId(Guid userId, Guid activityId)
        {
            return await Delete(ua => ua.UserId == userId && ua.ActivityId == activityId);
        }
    }
}
