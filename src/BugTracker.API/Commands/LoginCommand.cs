using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.API.Commands
{
    public class LoginCommand : IRequest<TokenResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class LoginHandler : IRequestHandler<LoginCommand, TokenResponse>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ITokenService _tokenService;

            public LoginHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService)
            {
                _userManager = userManager;
                _tokenService = tokenService;
            }


            public async Task<TokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var accessToken = _tokenService.GenerateJwt(user, roles);
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    
                    user.RefreshTokens.Add(refreshToken);
                    await _userManager.UpdateAsync(user);

                    return new TokenResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken.Token
                    };
                }
                return null;
            }
        }

        public class LoginCommandValidator : AbstractValidator<LoginCommand> 
        {
            public LoginCommandValidator()
            {
                RuleFor(l => l.Username).NotEmpty().Length(4, 50);
                RuleFor(p => p.Password).NotEmpty().Length(4, 50);
            }
        }
    }
}
