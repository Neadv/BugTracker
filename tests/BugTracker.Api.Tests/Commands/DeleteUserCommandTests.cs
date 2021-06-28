using BugTracker.API.Commands;
using BugTracker.API.Models;
using BugTracker.API.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BugTracker.Api.Tests.Commands
{
    public class DeleteUserCommandTests
    {
        [Fact]
        public async Task HandleDeleteUser()
        {
            // Arrange
            var username = "Username";
            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetUserByNameAsync(username)).ReturnsAsync(new ApplicationUser());
            userService.Setup(u => u.DeleteUser(It.IsAny<ApplicationUser>())).Returns(Task.CompletedTask);

            var handler = new DeleteUserCommand.DeleteUserHandler(userService.Object);

            // Act
            await handler.Handle(new DeleteUserCommand { Username = username }, new System.Threading.CancellationToken());

            // Assert
            userService.Verify(u => u.GetUserByNameAsync(username), Times.Once());
            userService.Verify(u => u.DeleteUser(It.IsAny<ApplicationUser>()), Times.Once());
        }
    }
}
