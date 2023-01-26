using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<TEntity> FindByIdAsync(Guid id);
    EntityEntry<TEntity> Delete(TEntity entity);
    void DeleteAll(IEnumerable<TEntity> entities);
    Task AddAsync(TEntity entity);
    Task<bool> DeleteIfExistsById(Guid id);
    void Update(TEntity entity);
    Task<bool> AnyByIdAsync(Guid id);
    IQueryable<TEntity> AsQueryable();
    Page<TEntity> AsPage(Pagination pagination);
    Page<TAdapt> AsPage<TAdapt>(Pagination pagination);
}
