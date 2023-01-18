using System.Linq.Expressions;
using TrainRecord.Core.Common;

namespace TrainRecord.Infrastructure.Interfaces;

public interface IRepositoryBase<TEntity>
{
    Task<TEntity> FindByIdAsync(int id);
    Task AddAsync(TEntity entity);
    Task<bool> AnyByExpressionAsync(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> GetAsQueryable();
    Page<TEntity> GetPage(Pagination pagination);
    Page<TAdapt> GetPage<TAdapt>(Pagination pagination);
    Task<int> SaveChangesAsync();
}
