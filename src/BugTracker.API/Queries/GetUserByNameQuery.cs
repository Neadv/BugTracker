using AutoMapper;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.API.Queries
{
    public class GetUserByNameQuery : IRequest<UserResponse>
    {
        public string Username { get; set; }

        public class GetUserByNameHandler : IRequestHandler<GetUserByNameQuery, UserResponse>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public GetUserByNameHandler(IUserService userService, IMapper mapper)
            {
                _userService = userService;
                _mapper = mapper;
            }

            public async Task<UserResponse> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetUserByNameAsync(request.Username);
                if (user != null)
                {
                    var result = _mapper.Map<ApplicationUser, UserResponse>(user);
                    return result;
                }
                return null;
            }
        }

        public class GetUserByNameQueryValidator : AbstractValidator<GetUserByNameQuery>
        {
            public GetUserByNameQueryValidator()
            {
                RuleFor(q => q.Username).NotEmpty();
            }
        }
    }
}
