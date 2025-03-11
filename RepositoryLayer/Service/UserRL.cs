using System.Security.Cryptography;
using System.Text;
using ModelLayer.Model;
using RepositoryLayer.Content;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

public class UserRL : IUserRL
{
    private readonly GreetingDbContext _dbContext;

    public UserRL(GreetingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Register(UserEntity user)
    {
        if (IsUsernameExists(user.Username))
            throw new Exception("Username already exists.");

        if (IsEmailExists(user.Email))
            throw new Exception("Email already registered.");

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public UserDTO? Login(string username, string password)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (user == null || user.PasswordHash != HashPassword(password, user.Salt))
            return null;

        return new UserDTO
        {
            Username = user.Username,
            Email = user.Email
        };
    }

    public bool IsUsernameExists(string username) =>
        _dbContext.Users.Any(u => u.Username == username);

    public bool IsEmailExists(string email) =>
        _dbContext.Users.Any(u => u.Email == email);

    public bool IsUserExists(string username, string email)
    {
        return _dbContext.Users.Any(u => u.Username == username || u.Email == email);
    }

    public string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }

    public string HashPassword(string password, string salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
        return Convert.ToBase64String(pbkdf2.GetBytes(32));
    }
}
