using BugTracker.API.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace BugTracker.API.Services
{
    public interface ITokenService
    {
        string GenerateJwt(ApplicationUser user);
        RefreshToken GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}