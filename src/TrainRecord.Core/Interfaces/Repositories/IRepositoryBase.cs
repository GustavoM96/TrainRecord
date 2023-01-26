using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<TEntity> FindByIdAsync(Guid id);
    EntityEntry<TEntity> Delete(TEntity entity);
    Task<bool> DeleteById(Guid id);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    Task<bool> AnyByIdAsync(Guid id);
    IQueryable<TEntity> AsQueryable();
    Page<TEntity> AsPage(Pagination pagination);
    Page<TAdapt> AsPage<TAdapt>(Pagination pagination);
}
