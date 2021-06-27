using BugTracker.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BugTracker.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var username = _contextAccessor.HttpContext.User?.Identity?.Name;
            if (username == null)
            {
                return null;
            }

            return await _userManager.FindByNameAsync(username);
        }
    }
}
