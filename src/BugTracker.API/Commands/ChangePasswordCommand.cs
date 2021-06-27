using BugTracker.API.Infrastructure.Validators;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.API.Commands
{
    public class ChangePasswordCommand : IRequest<RequestResult>
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }

        public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, RequestResult>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IUserService _userService;

            public ChangePasswordHandler(UserManager<ApplicationUser> userManager, IUserService userService)
            {
                _userManager = userManager;
                _userService = userService;
            }

            public async Task<RequestResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
                    if (result.Succeeded)
                        return RequestResult.CreateSucceeded();
                    return RequestResult.CreateError(result.Errors.Select(e => e.Description));                    
                }
                return RequestResult.CreateError(new[] { "Invalid user" });
            }
        }

        public class ChangePasswordValidator : Infrastructure.Validators.PasswordValidator<ChangePasswordCommand>
        {
            public ChangePasswordValidator()
            {
                RuleFor(c => c.Password).NotEmpty();
                RuleFor(r => r.NewPassword).NotEmpty().Length(6, 50).Must(HasValidPassword).WithMessage(InvalidPassword);
            }
        }
    }
}
