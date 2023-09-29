using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces;
using TrainRecord.Application.Interfaces.Repositories;

using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Common;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet;
    protected readonly AppDbContext _context;

    public RepositoryBase(AppDbContext context)
    {
        _dbSet = context.Set<TEntity>();
        _context = context;
    }

    protected DbSet<TDbSet> GetOtherDbSet<TDbSet>() where TDbSet : class, IEntity
    {
        return _context.Set<TDbSet>();
    }

    protected async Task<bool> UpdateById(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        EntityId<TEntity> id
    )
    {
        var affectedRows = await Where(e => e.Id == id).ExecuteUpdateAsync(setPropertyCalls);
        return affectedRows > 0;
    }

    protected async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await AsNoTracking().AnyAsync(expression);
    }

    protected async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await AsNoTracking().SingleOrDefaultAsync(expression);
    }

    protected IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        return AsNoTracking().Where(expression);
    }

    protected async Task<bool> Delete(Expression<Func<TEntity, bool>> expression)
    {
        var afectedRows = await Where(expression).ExecuteDeleteAsync();
        return afectedRows > 0;
    }

    public async Task<bool> AnyByIdAsync(EntityId<TEntity> id)
    {
        return await AsNoTracking().AnyAsync(e => e.Id == id);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<bool> DeleteById(EntityId<TEntity> id)
    {
        var affectedRows = await Where(e => e.Id == id.Value).ExecuteDeleteAsync();
        return affectedRows > 0;
    }

    public EntityEntry<TEntity> Update(TEntity entity)
    {
        return _dbSet.Update(entity);
    }

    public async Task<TEntity?> FindByIdAsync(EntityId<TEntity> id)
    {
        return await SingleOrDefaultAsync(t => t.Id == id);
    }

    public IQueryable<TEntity> AsNoTracking()
    {
        return _dbSet.AsNoTracking();
    }

    public Page<TEntity> AsPage(Pagination pagination)
    {
        return AsNoTracking().AsPage(pagination);
    }

    public Page<TAdapt> AsPage<TAdapt>(Pagination pagination)
    {
        return AsNoTracking().AsPageAdapted<TEntity, TAdapt>(pagination);
    }
}
