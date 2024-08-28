using TrainRecord.Core.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace TrainRecord.Core.Services.Auth;

public class HashGenerator : IHashGenerator
{
    public string Generate(string password)
    {
        return BC.HashPassword(password);
    }

    public bool VerifyHashedPassword(string password, string hashedPassword)
    {
        return BC.Verify(password, hashedPassword);
    }
}
