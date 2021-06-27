using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BugTracker.API.Infrastructure.Validators;

namespace BugTracker.API.Commands
{
    public class RegisterCommand : IRequest<RequestResult> 
    { 
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public class RegisterHandler : IRequestHandler<RegisterCommand, RequestResult>
        {
            private readonly UserManager<ApplicationUser> _userManager;

            public RegisterHandler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<RequestResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var user = new ApplicationUser
                {
                    UserName = request.Username,
                    Email = request.Email
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                    return RequestResult.CreateSucceeded();
                return RequestResult.CreateError(result.Errors.Select(e => e.Description));
            }
        }

        public class RegisterValidator : Infrastructure.Validators.PasswordValidator<RegisterCommand>
        {
            public RegisterValidator()
            {
                RuleFor(r => r.Username).NotEmpty().Length(6, 50);
                RuleFor(r => r.Email).NotEmpty().EmailAddress();
                RuleFor(r => r.Password).NotEmpty().Length(6, 50).Must(HasValidPassword).WithMessage(InvalidPassword);
            }
        }
    }
}
