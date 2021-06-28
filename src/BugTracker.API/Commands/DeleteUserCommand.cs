using BugTracker.API.Services;
using FluentValidation;
using MediatR;
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
                await _userService.DeleteUserByNameAsync(request.Username);
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
