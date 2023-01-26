using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Responses;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories
{
    public class UserActivityRepository : RepositoryBase<UserActivity>, IUserActivityRepository
    {
        public UserActivityRepository(AppDbContext context) : base(context) { }

        public IQueryable<Activity> GetActivitiesByUserId(Guid userId)
        {
            var dbSetActivity = GetDbSet<Activity>();

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

        public Task<UserActivity> GetRecordByUserAndActivityId(Guid userId, Guid activityId)
        {
            return SingleOrDefaultAsync(ua => ua.UserId == userId && ua.ActivityId == activityId);
        }
    }
}
