using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.API.Commands
{
    public class UpdateUserCommand : IRequest<RequestResult>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActivated { get; set; }

        public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, RequestResult>
        {
            private readonly IUserService _userService;

            public UpdateUserHandler(IUserService userService)
            {
                _userService = userService;
            }

            public async Task<RequestResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetUserByNameAsync(request.Username);
                if (user != null)
                {
                    if (user.Roles.Any(r => r.Name == "admin"))
                    {
                        return RequestResult.CreateError(new[] { "Forbidden" });
                    }

                    user.Email = request.Email;
                    user.IsActivated = request.IsActivated;
                    var result = await _userService.UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RequestResult.CreateSucceeded();
                    return RequestResult.CreateError(result.Errors.Select(e => e.Description));
                }
                return RequestResult.CreateError(new[] { "Invalid user" });
            }
        }

        public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
        {
            public UpdateUserCommandValidator()
            {
                RuleFor(c => c.Username).NotEmpty();
                RuleFor(c => c.Email).NotEmpty().EmailAddress();
            }
        }
    }
}
