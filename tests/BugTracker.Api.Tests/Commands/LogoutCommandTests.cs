using BugTracker.API.Commands;
using BugTracker.API.Models;
using BugTracker.API.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BugTracker.Api.Tests.Commands
{
    public class LogoutCommandTests
    {
        [Theory]
        [InlineData("RefreshToken")]
        [InlineData(null)]
        public async Task HandleLogoutCorrectUser(string refreshToken)
        {
            // Arrange
            var logoutCommand = new LogoutCommand { RefreshToken = refreshToken };
            var user = new ApplicationUser();

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetCurrentUserAsync()).ReturnsAsync(user);

            var authService = new Mock<IAuthorizationService>();
            authService.Setup(a => a.RemoveRefreshToken(refreshToken, user));

            var logoutHandler = new LogoutCommand.LogoutHandler(authService.Object, userService.Object);

            // Action
            var result = await logoutHandler.Handle(logoutCommand, new System.Threading.CancellationToken());

            // Assert
            userService.Verify(u => u.GetCurrentUserAsync(), Times.Once());
            authService.Verify(a => a.RemoveRefreshToken(refreshToken, user), Times.Once());
        }

        [Theory]
        [InlineData("ValidUser")]
        [InlineData(null)]
        public async Task HandleLogoutCorrectToken(string username)
        {
            // Arrange
            var logoutCommand = new LogoutCommand { RefreshToken = "RefreshToken" };
            var user = username != null ? new ApplicationUser { UserName = username } : null;

            var userService = new Mock<IUserService>();
            userService.Setup(u => u.GetCurrentUserAsync()).ReturnsAsync(user);

            var authService = new Mock<IAuthorizationService>();
            authService.Setup(a => a.RemoveRefreshToken(logoutCommand.RefreshToken, It.IsAny<ApplicationUser>()));

            var logoutHandler = new LogoutCommand.LogoutHandler(authService.Object, userService.Object);

            // Action
            var result = await logoutHandler.Handle(logoutCommand, new System.Threading.CancellationToken());

            // Assert
            if (username != null)
                authService.Verify(a => a.RemoveRefreshToken(logoutCommand.RefreshToken, user), Times.Once());
            if (username == null)
                authService.Verify(a => a.RemoveRefreshToken(logoutCommand.RefreshToken, It.IsAny<ApplicationUser>()), Times.Never());
        }
    }
}
