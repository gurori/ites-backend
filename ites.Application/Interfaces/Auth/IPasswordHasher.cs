namespace ites.Application.Interfaces.Auth
{
    public interface IPasswordHasher
    {
        string Generate(string password);
        bool IsVerify(string password, string hashedPassword);
    }
}
