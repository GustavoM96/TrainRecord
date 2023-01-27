using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IRepositoryContext
{
    Task<int> SaveChangesAsync();
    void Detached(object? obj);
}
