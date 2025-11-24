using Contacts.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(LoginModel model);
        Task<bool> Register(RegisterModel model);
        Task<bool> Logout(LoginModel model);
    }
}
