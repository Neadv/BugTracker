using BugTracker.API.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.API.Services
{
    public interface IUserService
    {
        UserManager<ApplicationUser> UserManager { get; }

        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, bool validatePassword = true);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(bool activated = true);
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<ApplicationUser> GetUserByNameAsync(string name);
    }
}