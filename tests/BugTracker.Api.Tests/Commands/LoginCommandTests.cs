using BugTracker.API.Commands;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BugTracker.Api.Tests.Commands
{
    public class LoginCommandTests
    {
        [Fact]
        public async Task HandleSuccessResult()
        {
            // Arrange
            var loginCommand = new LoginCommand { Username = "Test", Password = "TestPassword" };
            var user = new ApplicationUser { UserName = loginCommand.Username, IsActivated = true };
            var tokenResult = new TokenResult();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(u => u.CheckPasswordAsync(user, loginCommand.Password)).ReturnsAsync(true);

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetUserByNameAsync(loginCommand.Username)).ReturnsAsync(user);
            userService.SetupGet(u => u.UserManager).Returns(userManager.Object);

            var authService = new Mock<IAuthorizationService>();
            authService.Setup(a => a.AuthorizeAsync(user)).ReturnsAsync(tokenResult);

            var loginHadler = new LoginCommand.LoginHandler(authService.Object, userService.Object);

            // Action
            var result = await loginHadler.Handle(loginCommand, new System.Threading.CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tokenResult, result);
            userService.Verify(u => u.GetUserByNameAsync(loginCommand.Username), Times.Once());
            userManager.Verify(u => u.CheckPasswordAsync(user, loginCommand.Password), Times.Once());
            authService.Verify(a => a.AuthorizeAsync(user), Times.Once());
        }

        [Fact]
        public async Task HandleNotExistUser()
        {
            // Arrange
            var loginCommand = new LoginCommand { Username = "Test", Password = "TestPassword" };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetUserByNameAsync(loginCommand.Username)).ReturnsAsync((ApplicationUser)null);
            userService.SetupGet(u => u.UserManager).Returns(userManager.Object);

            var authService = new Mock<IAuthorizationService>();

            var loginHadler = new LoginCommand.LoginHandler(authService.Object, userService.Object);

            // Action
            var result = await loginHadler.Handle(loginCommand, new System.Threading.CancellationToken());

            // Assert
            Assert.Null(result);
            userService.Verify(u => u.GetUserByNameAsync(loginCommand.Username), Times.Once());
            userManager.Verify(u => u.CheckPasswordAsync(It.IsAny<ApplicationUser>(), loginCommand.Password), Times.Never());
            authService.Verify(a => a.AuthorizeAsync(It.IsAny<ApplicationUser>()), Times.Never());
        }

        [Fact]
        public async Task HandleWrongPassword()
        {
            // Arrange
            var loginCommand = new LoginCommand { Username = "Test", Password = "TestPassword" };
            var user = new ApplicationUser { UserName = loginCommand.Username, IsActivated = true };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(u => u.FindByNameAsync(loginCommand.Username)).ReturnsAsync(user);

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetUserByNameAsync(loginCommand.Username)).ReturnsAsync(user);
            userService.SetupGet(u => u.UserManager).Returns(userManager.Object);

            var authService = new Mock<IAuthorizationService>();

            var loginHadler = new LoginCommand.LoginHandler(authService.Object, userService.Object);

            // Action
            var result = await loginHadler.Handle(loginCommand, new System.Threading.CancellationToken());

            // Assert
            Assert.Null(result);
            userService.Verify(u => u.GetUserByNameAsync(loginCommand.Username), Times.Once());
            userManager.Verify(u => u.CheckPasswordAsync(user, loginCommand.Password), Times.Once());
            authService.Verify(a => a.AuthorizeAsync(It.IsAny<ApplicationUser>()), Times.Never());
        }
    }
}
