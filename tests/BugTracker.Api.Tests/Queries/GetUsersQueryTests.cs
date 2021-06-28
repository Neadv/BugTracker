using AutoMapper;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Queries;
using BugTracker.API.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BugTracker.Api.Tests.Queries
{
    public class GetUsersQueryTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task HandleUsersGet(bool isActivated)
        {
            // Arrange
            var query = new GetUsersQuery { Activated = isActivated };
            var users = new ApplicationUser[]
            {
                new ApplicationUser { UserName = "Activated", IsActivated = true },
                new ApplicationUser { UserName = "Not activated", IsActivated = false }
            };

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetAllUsersAsync(isActivated)).ReturnsAsync(users);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<IEnumerable<ApplicationUser>, IEnumerable<UserResponse>>(users)).Returns(users.Select(u => new UserResponse { Username = u.UserName }));

            var handler = new GetUsersQuery.GetUserQueryHandler(userService.Object, mapper.Object);

            // Action
            var result = await handler.Handle(query, new System.Threading.CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            userService.Verify(u => u.GetAllUsersAsync(isActivated), Times.Once());
            mapper.Verify(m => m.Map<IEnumerable<ApplicationUser>, IEnumerable<UserResponse>>(users), Times.Once());

            var username = "Activated";
            if (!isActivated)
                username = "Not activated";
            Assert.All(result, r => r.Username = username);

        }        
    }
}
