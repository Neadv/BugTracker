using BugTracker.API.Commands;
using BugTracker.API.Models;
using BugTracker.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.API.Controllers
{
    [ApiController]
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
        public async Task<ActionResult> CreateAsync(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Succeeded)
                return Created("api/users/" + result.Model.Username, result.Model);
            return Conflict(new { Status = StatusCodes.Status409Conflict, Errors = result.Errors });
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync()
        {
            return Ok();
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteByNameAsync(string name)
        {
            return NoContent();
        }
    }
}
