using BugTracker.API.Services;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.API.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public string Username { get; set; }

        public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
        {
            private readonly IUserService _userService;

            public DeleteUserHandler(IUserService userService)
            {
                _userService = userService;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetUserByNameAsync(request.Username);
                if (user != null && !user.Roles.Any(r => r.Name == "admin"))
                {
                    await _userService.DeleteUser(user);
                }
                return new Unit();
            }
        }

        public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
        {
            public DeleteUserCommandValidator()
            {
                RuleFor(c => c.Username).NotEmpty();
            }
        }
    }
}
