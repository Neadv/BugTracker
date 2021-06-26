using BugTracker.API.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("token")]
        public async Task<ActionResult> Login(LoginCommand loginCommand)
        {
            var tokenResponse = await _mediator.Send(loginCommand);
            if (tokenResponse == null)
                return Unauthorized(new { Status = StatusCodes.Status401Unauthorized, Error = "Invalid login or password" } );
            return Ok(tokenResponse);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterCommand registerCommand)
        {
            var result = await _mediator.Send(registerCommand);
            if (!result.Succeeded)
            {
                return Conflict(new
                {
                    Status = StatusCodes.Status409Conflict,
                    Errors = result.Errors
                });
            }
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh(RefreshCommand refreshCommand)
        {
            var token = await _mediator.Send(refreshCommand);
            if (token == null)
                return Unauthorized(new { Status = StatusCodes.Status401Unauthorized, Error = "Invalid access or refresh token" });
            return Ok(token);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout(LogoutCommand logoutCommand)
        {
            await _mediator.Send(logoutCommand);
            return Ok();
        }
    }
}
