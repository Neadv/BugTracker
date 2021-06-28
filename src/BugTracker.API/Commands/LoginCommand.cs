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
            private readonly IUserService _userService;
            private readonly IAuthorizationService _authorizationService;

            public LoginHandler(IAuthorizationService authorizationService, IUserService userService)
            {
                _authorizationService = authorizationService;
                _userService = userService;
            }


            public async Task<TokenResult> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetUserByNameAsync(request.Username);
                if (user != null && await _userService.UserManager.CheckPasswordAsync(user, request.Password) && user.IsActivated)
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
