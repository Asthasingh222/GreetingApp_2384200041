using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;
namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {

        //UC10
        bool IsUserExists(string username, string email);
        void Register(UserEntity user);
        UserEntity Login(string username, string password);
        UserEntity GetUserByEmail(string email);
        void UpdateUser(UserEntity user);
        string GenerateSalt();
        string HashPassword(string password, string salt);

        bool UpdatePasswordRL(UserEntity user);
    }
}
