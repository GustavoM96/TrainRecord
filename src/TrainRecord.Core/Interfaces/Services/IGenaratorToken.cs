using TrainRecord.Core.Entities;
using TrainRecord.Core.Services.Auth;

namespace TrainRecord.Core.Interfaces;

public interface IGenaratorToken
{
    ApiTokenResponse Generate(User user);
}
