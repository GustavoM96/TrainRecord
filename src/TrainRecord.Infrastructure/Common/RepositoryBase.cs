using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        protected readonly AppDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public RepositoryBase(AppDbContext context)
        {
            DbSet = context.Set<TEntity>();
            Context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        protected async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.AnyAsync(expression);
        }

        protected async Task<TEntity> SingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> expression
        )
        {
            return await DbSet.SingleOrDefaultAsync(expression);
        }

        protected IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Where(expression);
        }

        public async Task<bool> AnyByIdAsync(Guid id)
        {
            return await DbSet.AnyAsync(e => e.Id == id);
        }

        public async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public Page<TEntity> AsPage(Pagination pagination)
        {
            return AsQueryable().AsPage(pagination);
        }

        public Page<TAdapt> AsPage<TAdapt>(Pagination pagination)
        {
            return AsQueryable().AsPage<TEntity, TAdapt>(pagination);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
