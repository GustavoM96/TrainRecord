using Microsoft.AspNetCore.Identity;
using TrainRecord.Core.Entities;

namespace Core.Interfaces;

public interface IGenaratorHash
{
    public string Generate(User user, string password);
    PasswordVerificationResult VerifyHashedPassword(
        User user,
        string password,
        string hashedPassword
    );
}
