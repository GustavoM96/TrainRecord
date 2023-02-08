namespace TrainRecord.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    Task<int> RollBack();
    void Detached(object? obj);
}
