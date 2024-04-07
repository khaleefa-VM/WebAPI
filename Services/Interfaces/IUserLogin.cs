using Infrastructure.Models;
using Services.Models;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserLogin
    {
        string GetToken(LoginModel loginModel);
        Task<User> GetLoggedUserDetails(Guid userId);
    }
}
