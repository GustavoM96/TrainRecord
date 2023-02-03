namespace TrainRecord.Core.Interfaces.Repositories;

public interface IRepositoryContext
{
    Task<int> SaveChangesAsync();
    void Detached(object? obj);
}
