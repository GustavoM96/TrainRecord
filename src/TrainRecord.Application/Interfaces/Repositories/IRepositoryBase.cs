using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Interfaces;

namespace TrainRecord.Application.Interfaces.Repositories;

public interface IRepositoryBase<TEntity>
    where TEntity : class, IEntity
{
    Task<TEntity?> FindByIdAsync(EntityId<TEntity> id);
    Task<bool> DeleteById(EntityId<TEntity> id);
    Task AddAsync(TEntity entity);
    EntityEntry<TEntity> Update(TEntity entity);
    Task<bool> AnyByIdAsync(EntityId<TEntity> id);
    IQueryable<TEntity> AsNoTracking();
    Page<TEntity> AsPage(Pagination pagination);
    Page<TAdapt> AsPage<TAdapt>(Pagination pagination);
    EntityEntry<TEntity> Remove(TEntity entity);
}
