using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using System.Threading.Tasks;

namespace BugTracker.API.Services
{
    public interface IAuthorizationService
    {
        Task<TokenResult> AuthorizeAsync(ApplicationUser user);
        Task<RefreshToken> RemoveRefreshToken(string username, string refreshToken);
        Task<RefreshToken> RemoveRefreshToken(string refreshToken, ApplicationUser user);
    }
}