using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Interfaces;

public interface IGenaratorToken
{
    string Generate(User user);
}
