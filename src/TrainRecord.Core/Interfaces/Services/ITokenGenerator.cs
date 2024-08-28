using TrainRecord.Core.Entities;
using TrainRecord.Core.Services.Auth;

namespace TrainRecord.Core.Interfaces;

public interface ITokenGenerator
{
    ApiTokenResponse Generate(User user);
}
