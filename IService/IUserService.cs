using TheProjectTascamon.Models;
using TheProjectTascamon.ViewModel;

namespace TheProjectTascamon.IService
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterViewModel userRegister);
        Task<User> AuthenticateUserAsync(LoginViewModel userLogin);
        Task<User> GetUserByUsernameAsync(string username);
        
    }
}
