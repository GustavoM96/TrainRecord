using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Commum;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Interfaces.Repositories;
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

        protected DbSet<TDbSet> GetOtherDbSet<TDbSet>() where TDbSet : AuditableEntityBase
        {
            return _context.Set<TDbSet>();
        }
    }
}
