using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.API.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthorizationService(ITokenService tokenService, UserManager<ApplicationUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<TokenResult> AuthorizeAsync(ApplicationUser user)
        {
            if (user is null)
                throw new System.ArgumentNullException(nameof(user));

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _tokenService.GenerateJwt(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            return new TokenResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<RefreshToken> RemoveRefreshToken(string username, string refreshToken)
        {
            var user = await _userManager.Users.Where(u => u.UserName == username).Include(u => u.RefreshTokens).FirstOrDefaultAsync();
            if (user != null)
                return await RemoveRefreshToken(refreshToken, user);
            return null;
        }

        public async Task<RefreshToken> RemoveRefreshToken(string refreshToken, ApplicationUser user)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new System.ArgumentException($"\"{nameof(refreshToken)}\" can't be null or empty.", nameof(refreshToken));
            if (user is null)
                throw new System.ArgumentNullException(nameof(user));

            var refresh = user.RefreshTokens.FirstOrDefault(r => r.Token == refreshToken);
            if (refresh != null)
            {
                user.RefreshTokens.Remove(refresh);
                await _userManager.UpdateAsync(user);
                return refresh;
            }
            return null;
        }
    }
}
