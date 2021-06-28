using BugTracker.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserManager<ApplicationUser> UserManager { get; }

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
        {
            UserManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var username = _contextAccessor.HttpContext.User?.Identity?.Name;
            if (username == null)
            {
                return null;
            }

            return await UserManager.FindByNameAsync(username);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, bool validatePassword = true)
        {
            IdentityResult result;
            if (validatePassword)
            {
                result = await UserManager.CreateAsync(user, password);
            }
            else
            {
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, password);
                result = await UserManager.CreateAsync(user);
            }
            return result;
        }

        public async Task<ApplicationUser> GetUserByNameAsync(string name)
        {
            return await UserManager.Users.Where(u => u.UserName == name).Include(u => u.RefreshTokens).Include(u => u.Roles).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(bool activated = true)
        {
            return await UserManager.Users.Where(u => u.IsActivated == activated).Include(u => u.Roles).ToListAsync();
        }
    }
}
