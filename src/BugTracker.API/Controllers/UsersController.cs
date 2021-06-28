using BugTracker.API.Commands;
using BugTracker.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BugTracker.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync([FromQuery]GetUsersQuery getUsers)
        {
            var result = await _mediator.Send(getUsers);
            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult> GetByNameAsync(string name)
        {
            var query = new GetUserByNameQuery { Username = name };
            var result = await _mediator.Send(query);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "ProjectManage")]
        public async Task<ActionResult> CreateAsync(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Succeeded)
                return Created("api/users/" + result.Model.Username, result.Model);
            return Conflict(new { Status = StatusCodes.Status409Conflict, Errors = result.Errors });
        }

        [HttpDelete("{name}")]
        [Authorize(Policy = "Admins")]
        public async Task<ActionResult> DeleteByNameAsync(string name)
        {
            var command = new DeleteUserCommand { Username = name };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
