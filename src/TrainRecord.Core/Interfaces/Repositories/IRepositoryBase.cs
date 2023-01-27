using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> : IRepositoryContext where TEntity : class
{
    Task<TEntity> FindByIdAsync(Guid id);
    Task<bool> DeleteById(Guid id);
    Task AddAsync(TEntity entity);
    EntityEntry<TEntity> Update(TEntity entity);
    Task<bool> AnyByIdAsync(Guid id);
    IQueryable<TEntity> AsQueryable();
    Page<TEntity> AsPage(Pagination pagination);
    Page<TAdapt> AsPage<TAdapt>(Pagination pagination);
}
