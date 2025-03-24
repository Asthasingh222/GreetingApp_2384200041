using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System;
using RepositoryLayer.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly ILogger<UserRL> logger;
        private readonly GreetingDbContext _context;

        public UserRL(GreetingDbContext context,ILogger<UserRL> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public bool IsUserExists(string username, string email)
        {
            return _context.Users.Any(u => u.Username == username || u.Email == email);
        }

        public void Register(UserEntity user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public UserEntity Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return null;

            string hashedPassword = HashPassword(password, user.Salt);
            return user.PasswordHash == hashedPassword ? user : null;
        }

        public UserEntity? GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            logger.LogDebug("GetUserByEmail - Searching for Email: {Email}, Found: {User}", email, user);
            return user;
        }

        public void UpdateUser(UserEntity user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string password, string salt)
        {
            using (var sha512 = SHA512.Create())
            {
                byte[] saltedPassword = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashedBytes = sha512.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool UpdatePasswordRL(UserEntity user)
        {

            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }
    }
}
