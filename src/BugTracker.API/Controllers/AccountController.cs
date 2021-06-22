using BugTracker.API.Commands;
using MediatR;
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

    }
}
