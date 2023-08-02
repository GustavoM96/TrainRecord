namespace TrainRecord.Infrastructure.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
    int RollBack();
}
