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
        void Register(UserEntity user);
        UserDTO? Login(string username, string password);
        bool IsUsernameExists(string username);
        bool IsEmailExists(string email);
        bool IsUserExists(string username, string email);
        string GenerateSalt();
        string HashPassword(string password, string salt);

    }
}
