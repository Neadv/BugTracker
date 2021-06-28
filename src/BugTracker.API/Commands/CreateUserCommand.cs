using AutoMapper;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.API.Commands
{
    public class CreateUserCommand : IRequest<RequestResult<UserResponse>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public class CreateUserHandler : IRequestHandler<CreateUserCommand, RequestResult<UserResponse>>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public CreateUserHandler(IUserService userService, IMapper mapper)
            {
                _userService = userService;
                _mapper = mapper;
            }


            public async Task<RequestResult<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = new ApplicationUser { UserName = request.Username, Email = request.Email, IsActivated = true };
                var result = await _userService.CreateUserAsync(user, request.Password, false);
                if (result.Succeeded)
                {
                    var userResponse = _mapper.Map<ApplicationUser, UserResponse>(user);
                    userResponse.Roles = null;
                    return RequestResult<UserResponse>.CreateSucceeded(userResponse);
                }
                return RequestResult<UserResponse>.CreateError(result.Errors.Select(e => e.Description));
            }
        }

        public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
        {
            public CreateUserCommandValidator()
            {
                RuleFor(c => c.Username).NotEmpty().Length(4, 20);
                RuleFor(c => c.Email).NotEmpty().EmailAddress();
                RuleFor(c => c.Password).NotEmpty();
            }
        }
    }
}
