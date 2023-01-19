using System.Linq.Expressions;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IRepositoryBase<TEntity>
{
    Task<TEntity> FindByIdAsync(Guid id);

    // IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);

    // Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
    Task<bool> AnyByIdAsync(Guid id);

    // Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> AsQueryable();
    Page<TEntity> AsPage(Pagination pagination);
    Page<TAdapt> AsPage<TAdapt>(Pagination pagination);
    Task<int> SaveChangesAsync();
}
