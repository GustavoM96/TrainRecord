using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Core.Repositories;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories
{
    public class UserActivityRepository : RepositoryBase<UserActivity>, IUserActivityRepository
    {
        public UserActivityRepository(AppDbContext context) : base(context) { }

        public IQueryable<UserActivity> GetAllByUserId(Guid userId)
        {
            return Where(ua => ua.UserId == userId);
        }
    }
}
