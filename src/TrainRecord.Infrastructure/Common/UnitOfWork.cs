using Microsoft.EntityFrameworkCore;
using TrainRecord.Application.Interfaces.Repositories;

using TrainRecord.Infrastructure.Extentions;
using TrainRecord.Infrastructure.Persistence;

namespace TrainRecord.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork
{
    protected readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }

    public int RollBack()
    {
        var changedEntriesCopy = _context.ChangeTracker
            .Entries()
            .Where(e => e.AnyChange())
            .ToList();

        foreach (var entry in changedEntriesCopy)
        {
            entry.State = EntityState.Detached;
        }

        return changedEntriesCopy.Count;
    }
}
