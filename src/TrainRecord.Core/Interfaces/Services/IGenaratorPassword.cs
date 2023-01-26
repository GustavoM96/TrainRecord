using Microsoft.AspNetCore.Identity;
using TrainRecord.Core.Entities;

namespace TrainRecord.Core.Interfaces;

public interface IGenaratorHash
{
    public string Generate(User user);
    PasswordVerificationResult VerifyHashedPassword(
        User user,
        string password,
        string hashedPassword
    );
}
