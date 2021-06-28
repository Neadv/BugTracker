using AutoMapper;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Queries;
using BugTracker.API.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BugTracker.Api.Tests.Queries
{
    public class GetUserByNameQueryTests
    {
        [Theory]
        [InlineData("Existing user")]
        [InlineData("Not existing user")]
        public async Task HandleGetUser(string username)
        {
            // Arrange
            var query = new GetUserByNameQuery { Username = username };

            var userService = new Mock<IUserService>();
            var user = username == "Existing user" ? new ApplicationUser { UserName = username } : null;
            userService.Setup(u => u.GetUserByNameAsync(username)).ReturnsAsync(user);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<ApplicationUser, UserResponse>(It.Is<ApplicationUser>(u => u.UserName == username))).Returns(new UserResponse { Username = username });

            var handler = new GetUserByNameQuery.GetUserByNameHandler(userService.Object, mapper.Object);

            // Act
            var result = await handler.Handle(query, new System.Threading.CancellationToken());

            // Assert
            userService.Verify(u => u.GetUserByNameAsync(username), Times.Once());
            if (username == "Existing user")
            {
                Assert.NotNull(result);
                Assert.Equal(username, result.Username);
                mapper.Verify(m => m.Map<ApplicationUser, UserResponse>(It.IsAny<ApplicationUser>()), Times.Once());
            }
            else
            {
                Assert.Null(result);
                mapper.Verify(m => m.Map<ApplicationUser, UserResponse>(It.IsAny<ApplicationUser>()), Times.Never());
            }
        }        
    }
}
