using Microsoft.AspNetCore.Identity;

namespace iXpenseBackend.Layers.Repositories
{
    public class UserRepo
    {
        private readonly UserManager<IdentityUser> _userManager;
        
        public UserRepo(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUser> FindUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityUser> FindUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}
