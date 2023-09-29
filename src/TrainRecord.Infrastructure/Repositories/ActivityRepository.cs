using TrainRecord.Core.Entities;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Repositories;

public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
{
    public ActivityRepository(AppDbContext context) : base(context) { }

    public async Task<bool> AnyByNameAsync(string name)
    {
        return await AnyAsync(a => a.Name == name);
    }
}
