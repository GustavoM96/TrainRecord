namespace TrainRecord.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    int RollBack();
    void Detached(object obj);
}
