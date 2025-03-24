using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        string Register(UserDTO user);
        string Login(string username, string password);
        bool ForgetPasswordBL(ForgetPasswordDTO forgetPasswordDTO);
        bool ResetPasswordBL(ResetPasswordDTO resetPasswordDTO);

    }
}
