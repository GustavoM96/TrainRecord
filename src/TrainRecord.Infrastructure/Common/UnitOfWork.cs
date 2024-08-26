using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TrainRecord.Application.Interfaces.Repositories;
using TrainRecord.Infrastructure.Extensions;
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

    public async Task<IDbContextTransaction> BeginTransaction(CancellationToken ct = default)
    {
        return await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(
        IDbContextTransaction transaction,
        CancellationToken ct = default
    )
    {
        await transaction.CommitAsync(ct);
    }

    public int RollBack()
    {
        var changedEntriesCopy = _context
            .ChangeTracker.Entries()
            .Where(e => e.AnyChange())
            .ToList();

        foreach (var entry in changedEntriesCopy)
        {
            entry.State = EntityState.Detached;
        }

        return changedEntriesCopy.Count;
    }
}
