using AutoMapper;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.API.Queries
{
    public class GetUsersQuery : IRequest<IEnumerable<UserResponse>>
    {
        public bool Activated { get; set; }

        public class GetUserQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserResponse>>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public GetUserQueryHandler(IUserService userService, IMapper mapper)
            {
                _userService = userService;
                _mapper = mapper;
            }

            public async Task<IEnumerable<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _userService.GetAllUsersAsync(request.Activated);
                var userResponse = _mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<UserResponse>>(users);
                return userResponse;
            }
        }
    }
}
