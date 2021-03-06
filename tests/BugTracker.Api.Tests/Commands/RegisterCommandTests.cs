using BugTracker.API.Commands;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BugTracker.Api.Tests.Commands
{
    public class RegisterCommandTests
    {
        [Fact]
        public async Task HandleCreateUserSuccess()
        {
            // Arange
            var registerCommand = new RegisterCommand { Username = "Test", Email = "test@test.com", Password = "TestPassword" };
            var token = new TokenResult();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(u => u.CreateAsync(It.IsNotNull<ApplicationUser>(), registerCommand.Password)).ReturnsAsync(IdentityResult.Success);

            var registerHandler = new RegisterCommand.RegisterHandler(userManager.Object);

            // Action
            var result = await registerHandler.Handle(registerCommand, new System.Threading.CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            userManager.Verify(u => u.CreateAsync(It.IsNotNull<ApplicationUser>(), registerCommand.Password), Times.Once());
        }

        [Fact]
        public async Task HandleCreateUserError()
        {
            // Arange
            var registerCommand = new RegisterCommand { Username = "Test", Email = "test@test.com", Password = "TestPassword" };
            var error = new IdentityError { Description = "Error" };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(u => u.CreateAsync(It.IsNotNull<ApplicationUser>(), registerCommand.Password)).ReturnsAsync(IdentityResult.Failed(error));

            var registerHandler = new RegisterCommand.RegisterHandler(userManager.Object);

            // Action
            var result = await registerHandler.Handle(registerCommand, new System.Threading.CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Contains(error.Description, result.Errors);
            userManager.Verify(u => u.CreateAsync(It.IsNotNull<ApplicationUser>(), registerCommand.Password), Times.Once());
        }
    }
}
