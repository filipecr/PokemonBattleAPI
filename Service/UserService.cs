using Microsoft.AspNetCore.Identity;
using TheProjectTascamon.IRepos;
using TheProjectTascamon.IService;
using TheProjectTascamon.Models;
using TheProjectTascamon.ViewModel;

namespace TheProjectTascamon.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; 
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> RegisterUserAsync(RegisterViewModel userRegister)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(userRegister.Username);
            if (existingUser != null)
            {
                return null; // User already exists
            }

            var user = new User
            {
                UserName = userRegister.Username,
                Email = userRegister.Email
            };

            // Hash the password before storing it
            user.PasswordHash = _passwordHasher.HashPassword(user, userRegister.Password);

            await _userRepository.CreateUserAsync(user);

            return user;
        }

        public async Task<User> AuthenticateUserAsync(LoginViewModel userLogin)
        {
            var user = await _userRepository.GetUserByUsernameAsync(userLogin.Username);
            if (user == null || !_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLogin.Password).Equals(PasswordVerificationResult.Success))
            {
                return null; // User not found or password is incorrect
            }

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }
    }

}
