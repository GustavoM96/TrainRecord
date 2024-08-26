using Microsoft.EntityFrameworkCore.Storage;

namespace TrainRecord.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransaction(CancellationToken ct = default);
    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    int RollBack();
}
