using System.Linq.Expressions;
using TrainRecord.Core.Common;

namespace TrainRecord.Core.Interfaces.Repositories;

public interface IRepositoryBase<TEntity>
{
    Task<TEntity> FindByIdAsync(Guid id);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> GetAsQueryable();
    Page<TEntity> GetPage(Pagination pagination);
    Page<TAdapt> GetPage<TAdapt>(Pagination pagination);
    Task<int> SaveChangesAsync();
}
