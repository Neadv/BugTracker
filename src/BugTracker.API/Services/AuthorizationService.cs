using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using Microsoft.AspNetCore.Identity;
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
    }
}
