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

namespace BugTracker.API.Commands
{
    public class RegisterCommand : IRequest<RegisterResult> 
    { 
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResult>
        {
            private readonly UserManager<ApplicationUser> _userManager;

            public RegisterHandler(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {
                var user = new ApplicationUser
                {
                    UserName = request.Username,
                    Email = request.Email
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                    return RegisterResult.CreateSucceeded();
                return RegisterResult.CreateError(result.Errors.Select(e => e.Description));
            }
        }

        public class RegisterValidator : AbstractValidator<RegisterCommand>
        {
            public RegisterValidator()
            {
                var invalidPassword = "Password must contain at least 6, uppercase, lowercase, numbers and a special character";

                RuleFor(r => r.Username).NotEmpty().Length(6, 50);
                RuleFor(r => r.Email).NotEmpty().EmailAddress();
                RuleFor(r => r.Password).NotEmpty().Length(6, 50).Must(HasValidPassword).WithMessage(invalidPassword);
            }

            private bool HasValidPassword(string pw)
            {
                var lowercase = new Regex(@"[a-z]+");
                var uppercase = new Regex(@"[A-Z]+");
                var digit = new Regex(@"(\d)+");
                var symbol = new Regex(@"(\W)+");

                return (lowercase.IsMatch(pw) && uppercase.IsMatch(pw) && digit.IsMatch(pw) && symbol.IsMatch(pw));
            }
        }
    }
}
