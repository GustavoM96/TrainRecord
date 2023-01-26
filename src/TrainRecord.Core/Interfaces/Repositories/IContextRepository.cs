using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IContextRepository
{
    Task<int> SaveChangesAsync();
}
