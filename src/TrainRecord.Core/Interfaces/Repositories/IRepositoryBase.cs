using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<TEntity> FindByIdAsync(Guid id);
    Task<bool> DeleteById(Guid id);
    Task AddAsync(TEntity entity);
    EntityEntry<TEntity> Update(TEntity entity);
    Task<bool> AnyByIdAsync(Guid id);
    IQueryable<TEntity> AsNoTracking();
    Page<TEntity> AsPage(Pagination pagination);
    Page<TAdapt> AsPage<TAdapt>(Pagination pagination);
}
