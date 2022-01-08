using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IAuthService
    { 
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(LoginModel model);
        Task<AuthModel> RegisterAdminAsync(RegisterModel model);
    }

}
