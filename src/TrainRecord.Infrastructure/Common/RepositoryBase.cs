using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Infrastructure;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Common
{
    public abstract class RepositoryBase<TEntity> : RepositoryContext, IRepositoryBase<TEntity>
        where TEntity : BaseAuditableEntity
    {
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(AppDbContext context) : base(context)
        {
            _dbSet = context.Set<TEntity>();
        }

        protected async Task<bool> Delete(Expression<Func<TEntity, bool>> expression)
        {
            var afectedRows = await Where(expression).ExecuteDeleteAsync();
            return afectedRows > 0;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<bool> DeleteById(Guid id)
        {
            var affectedRows = await Where(e => e.Id == id).ExecuteDeleteAsync();
            return affectedRows > 0;
        }

        public EntityEntry<TEntity> Update(TEntity entity)
        {
            return _dbSet.Update(entity);
        }

        protected async Task<bool> UpdateById(
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
            Guid id
        )
        {
            var affectedRows = await Where(e => e.Id == id).ExecuteUpdateAsync(setPropertyCalls);
            return affectedRows > 0;
        }

        protected async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        protected async Task<TEntity> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> expression
        )
        {
            return await _dbSet.SingleOrDefaultAsync(expression);
        }

        protected IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<bool> AnyByIdAsync(Guid id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }

        public async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public Page<TEntity> AsPage(Pagination pagination)
        {
            return AsQueryable().AsPage(pagination);
        }

        public Page<TAdapt> AsPage<TAdapt>(Pagination pagination)
        {
            return AsQueryable().AsPage<TEntity, TAdapt>(pagination);
        }
    }
}
