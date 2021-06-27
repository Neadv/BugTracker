using BugTracker.API.Services;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.API.Commands
{
    public class LogoutCommand : IRequest
    {
        public string RefreshToken { get; set; }

        public class LogoutHandler : IRequestHandler<LogoutCommand>
        {
            private readonly IAuthorizationService _authorizationService;
            private readonly IUserService _userService;

            public LogoutHandler(IAuthorizationService authorizationService, IUserService userService)
            {
                _authorizationService = authorizationService;
                _userService = userService;
            }

            public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user != null)
                    await _authorizationService.RemoveRefreshToken(request.RefreshToken, user);

                return new Unit();
            }
        }

        public class LogoutValidator : AbstractValidator<LogoutCommand>
        {
            public LogoutValidator()
            {
                RuleFor(l => l.RefreshToken).NotEmpty();
            }
        }
    }
}
