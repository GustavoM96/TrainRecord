namespace TrainRecord.Core.Interfaces;

public interface IHashGenerator
{
    public string Generate(string password);
    bool VerifyHashedPassword(string password, string hashedPassword);
}
