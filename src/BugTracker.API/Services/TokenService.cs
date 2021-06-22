using BugTracker.API.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BugTracker.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenOptions _tokenOptions;

        public TokenService(IOptions<TokenOptions> options)
        {
            _tokenOptions = options.Value;
        }

        public string GenerateJwt(ApplicationUser user, IEnumerable<string> roles = null)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };
            if (roles != null)
            {
                foreach (var r in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, r));
                }
            }

            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                notBefore: DateTime.UtcNow,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_tokenOptions.Lifetime)),
                signingCredentials: new SigningCredentials(_tokenOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            string token;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
            }
            return new RefreshToken
            {
                Token = token,
                Expires = DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenLifetime)
            };
        }
    }
}
