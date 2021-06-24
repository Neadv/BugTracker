using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.API.Commands
{
    public class RefreshCommand : IRequest<TokenResult>
    {
        public string ExpiredToken { get; set; }
        public string RefreshToken { get; set; }

        public class RefreshHandler : IRequestHandler<RefreshCommand, TokenResult>
        {
            private readonly ITokenService _tokenService;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IAuthorizationService _authorizationService;

            public RefreshHandler(ITokenService tokenService,
                                  UserManager<ApplicationUser> userManager,
                                  IAuthorizationService authorizationService)
            {
                _tokenService = tokenService;
                _userManager = userManager;
                _authorizationService = authorizationService;
            }

            public async Task<TokenResult> Handle(RefreshCommand request, CancellationToken cancellationToken)
            {
                ClaimsPrincipal claimsPrincipal;
                try
                {
                    claimsPrincipal = _tokenService.GetPrincipalFromToken(request.ExpiredToken);
                }
                catch (SecurityTokenException)
                {
                    return null;
                }

                RefreshToken refreshToken = await _authorizationService.RemoveRefreshToken(claimsPrincipal.Identity.Name, request.RefreshToken);

                if (refreshToken != null && refreshToken.Active)
                {
                    var user = await _userManager.FindByNameAsync(claimsPrincipal.Identity.Name);
                    return await _authorizationService.AuthorizeAsync(user);
                }

                return null;
            }
        }

        public class RefreshValidator : AbstractValidator<RefreshCommand>
        {
            public RefreshValidator()
            {
                RuleFor(r => r.ExpiredToken).NotEmpty();
                RuleFor(r => r.RefreshToken).NotEmpty();
            }
        }
    }
}
