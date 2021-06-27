using BugTracker.API.Commands;
using BugTracker.API.Models;
using BugTracker.API.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace BugTracker.Api.Tests.Commands
{
    public class ChangePasswordCommandTests
    {
        [Fact]
        public async void HandleChangePasswordCorrectPassword()
        {
            // Arrange
            var command = new ChangePasswordCommand { Password = "correctPassword", NewPassword = "newPassword" };

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetCurrentUserAsync()).ReturnsAsync(new ApplicationUser());

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(u => u.ChangePasswordAsync(It.IsAny<ApplicationUser>(), command.Password, command.NewPassword)).ReturnsAsync(IdentityResult.Success);

            var handler = new ChangePasswordCommand.ChangePasswordHandler(userManager.Object, userService.Object);

            // Act
            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Empty(result.Errors);
            userService.Verify(u => u.GetCurrentUserAsync(), Times.Once());
            userManager.Verify(u => u.ChangePasswordAsync(It.IsAny<ApplicationUser>(), command.Password, command.NewPassword), Times.Once());
        }

        [Fact]
        public async void HandleChangePasswordIncorrectPassword()
        {
            // Arrange
            var command = new ChangePasswordCommand { Password = "incorrectPassword", NewPassword = "newPassword" };

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetCurrentUserAsync()).ReturnsAsync(new ApplicationUser());

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(u => u.ChangePasswordAsync(It.IsAny<ApplicationUser>(), command.Password, command.NewPassword)).ReturnsAsync(IdentityResult.Failed(new IdentityError()));

            var handler = new ChangePasswordCommand.ChangePasswordHandler(userManager.Object, userService.Object);

            // Act
            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.NotEmpty(result.Errors);
            userService.Verify(u => u.GetCurrentUserAsync(), Times.Once());
            userManager.Verify(u => u.ChangePasswordAsync(It.IsAny<ApplicationUser>(), command.Password, command.NewPassword), Times.Once());
        }
    }
}
