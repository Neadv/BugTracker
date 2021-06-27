using BugTracker.API.Models;
using System.Threading.Tasks;

namespace BugTracker.API.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
    }
}