using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TrainRecord.Core.Common;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Extentions;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Infrastructure;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Common
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : BaseAuditableEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(AppDbContext context)
        {
            _dbSet = context.Set<TEntity>();
            _context = context;
        }

        protected DbSet<TDbSet> GetDbSet<TDbSet>() where TDbSet : BaseAuditableEntity
        {
            return _context.Set<TDbSet>();
        }

        protected DbSet<TEntity> GetDbSet()
        {
            return _context.Set<TEntity>();
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

        public EntityEntry<TEntity> Delete(TEntity entity)
        {
            return _dbSet.Remove(entity);
        }

        public async Task<bool> DeleteById(Guid id)
        {
            var afectedRows = await Where(e => e.Id == id).ExecuteDeleteAsync();
            return afectedRows > 0;
        }

        public async Task<bool> DeleteIfExistsById(Guid id)
        {
            var entity = await FindByIdAsync(id);
            if (entity is null)
            {
                return false;
            }

            Delete(entity);
            return true;
        }

        public void DeleteAll(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
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
