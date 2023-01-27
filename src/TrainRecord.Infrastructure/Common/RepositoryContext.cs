using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Entities;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Infrastructure.Common;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Common
{
    public class RepositoryContext : IRepositoryContext
    {
        protected readonly AppDbContext _context;

        public RepositoryContext(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Detached(object? obj)
        {
            _context.Entry(obj).State = EntityState.Detached;
        }

        protected DbSet<TDbSet> GetOtherDbSet<TDbSet>() where TDbSet : BaseAuditableEntity
        {
            return _context.Set<TDbSet>();
        }
    }
}
