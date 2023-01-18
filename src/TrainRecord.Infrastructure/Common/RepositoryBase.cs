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

namespace TrainRecord.Core.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : BaseAuditableEntity
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<TEntity> DbSet;

        public RepositoryBase(AppDbContext context)
        {
            DbSet = context.Set<TEntity>();
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.AnyAsync(expression);
        }

        public async Task<bool> AnyByIdAsync(Guid id)
        {
            return await DbSet.AnyAsync(e => e.Id == id);
        }

        public async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.SingleOrDefaultAsync(expression);
        }

        public IQueryable<TEntity> GetAsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public Page<TEntity> GetPage(Pagination pagination)
        {
            return GetAsQueryable().GetPage(pagination);
        }

        public Page<TAdapt> GetPage<TAdapt>(Pagination pagination)
        {
            return GetAsQueryable().GetPage<TEntity, TAdapt>(pagination);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
