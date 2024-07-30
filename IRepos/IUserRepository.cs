using TheProjectTascamon.Models;

namespace TheProjectTascamon.IRepos
{

    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
        

    }

}
