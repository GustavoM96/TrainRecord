namespace TrainRecord.Infrastructure.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    int RollBack();
}
