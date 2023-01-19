using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Repositories;
using TrainRecord.Core.Responses;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories
{
    public class UserActivityRepository : RepositoryBase<UserActivity>, IUserActivityRepository
    {
        public UserActivityRepository(AppDbContext context) : base(context) { }

        public IQueryable<Activity> GetActivitiesByUserId(Guid userId)
        {
            return Where(ua => ua.UserId == userId)
                .Join(_context.Activities, ua => ua.ActivityId, a => a.Id, (_, a) => a)
                .Distinct();
        }

        public IQueryable<UserActivity> GetRecordByUserAndActivityId(Guid userId, Guid activityId)
        {
            return Where(ua => ua.UserId == userId && ua.ActivityId == activityId);
        }
    }
}
