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
    public class LoginCommand : IRequest<TokenResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class LoginHandler : IRequestHandler<LoginCommand, TokenResult>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IAuthorizationService _authorizationService;

            public LoginHandler(UserManager<ApplicationUser> userManager, IAuthorizationService authorizationService)
            {
                _userManager = userManager;
                _authorizationService = authorizationService;
            }


            public async Task<TokenResult> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    return await _authorizationService.AuthorizeAsync(user);
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
