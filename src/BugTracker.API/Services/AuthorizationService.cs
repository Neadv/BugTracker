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
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthorizationService(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<TokenResult> AuthorizeAsync(ApplicationUser user)
        {
            if (user is null)
                throw new System.ArgumentNullException(nameof(user));

            var roles = await _userService.UserManager.GetRolesAsync(user);
            var accessToken = _tokenService.GenerateJwt(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userService.UserManager.UpdateAsync(user);

            return new TokenResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<RefreshToken> RemoveRefreshToken(string username, string refreshToken)
        {
            var user = await _userService.GetUserByNameAsync(username);
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
                await _userService.UserManager.UpdateAsync(user);
                return refresh;
            }
            return null;
        }
    }
}
