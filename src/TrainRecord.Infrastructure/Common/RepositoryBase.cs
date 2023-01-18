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
using TrainRecord.Infrastructure;
using TrainRecord.Infrastructure.Interfaces;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Core.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : BaseAuditableEntity
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(AppDbContext context)
        {
            _dbSet = context.Set<TEntity>();
            var a = context.Set<User>().AsQueryable().GetPage<User, Activity>(new Pagination());
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<bool> AnyByExpressionAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }

        public async Task<bool> AnyByIdAsync(Guid id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<TEntity> GetAsQueryable()
        {
            return _dbSet.AsQueryable();
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
