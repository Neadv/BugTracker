using BugTracker.API.Models;
using BugTracker.API.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            private readonly IHttpContextAccessor _contextAccessor;

            public LogoutHandler(IHttpContextAccessor contextAccessor, IAuthorizationService authorizationService)
            {
                _contextAccessor = contextAccessor;
                _authorizationService = authorizationService;
            }

            public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
            {
                var userName = _contextAccessor.HttpContext.User?.Identity?.Name;
                if (userName == null)
                    return new Unit();

                await _authorizationService.RemoveRefreshToken(userName, request.RefreshToken);

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
