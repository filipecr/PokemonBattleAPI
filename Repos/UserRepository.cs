using Microsoft.EntityFrameworkCore;
using TheProjectTascamon.DBContext;
using TheProjectTascamon.IRepos;
using TheProjectTascamon.Models;

namespace TheProjectTascamon.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly TascamonContext _context;

        public UserRepository(TascamonContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                throw new Exception($"User with username {username} not found.");
            }
            return user;
        }

        public async Task CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        

    }
}
