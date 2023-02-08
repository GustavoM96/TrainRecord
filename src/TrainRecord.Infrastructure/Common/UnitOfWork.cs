using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Core.Interfaces.Repositories;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RollBack()
        {
            var changedEntriesCopy = _context.ChangeTracker
                .Entries()
                .Where(
                    e =>
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted
                );

            foreach (var entry in changedEntriesCopy)
            {
                entry.State = EntityState.Detached;
            }

            return changedEntriesCopy.Count();
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
