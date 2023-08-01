using Microsoft.EntityFrameworkCore;
using TrainRecord.Core.Commum.Bases;
using TrainRecord.Infrastructure.Interfaces.Repositories;
using TrainRecord.Infrastructure.Extentions;
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

        public int RollBack()
        {
            var changedEntriesCopy = _context.ChangeTracker.Entries().Where(e => e.AnyChange());

            foreach (var entry in changedEntriesCopy)
            {
                entry.State = EntityState.Detached;
            }

            return changedEntriesCopy.Count();
        }
    }
}
