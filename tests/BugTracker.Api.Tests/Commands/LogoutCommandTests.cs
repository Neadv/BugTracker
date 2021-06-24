using BugTracker.API.Commands;
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
            var username = "ValidUser";

            var context = new Mock<IHttpContextAccessor>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns(username);

            var authService = new Mock<IAuthorizationService>();
            authService.Setup(a => a.RemoveRefreshToken(username, refreshToken));

            var logoutHandler = new LogoutCommand.LogoutHandler(context.Object, authService.Object);

            // Action
            var result = await logoutHandler.Handle(logoutCommand, new System.Threading.CancellationToken());

            // Assert
            authService.Verify(a => a.RemoveRefreshToken(username, refreshToken), Times.Once());
        }

        [Theory]
        [InlineData("ValidUser")]
        [InlineData(null)]
        public async Task HandleLogoutCorrectToken(string username)
        {
            // Arrange
            var logoutCommand = new LogoutCommand { RefreshToken = "RefreshToken" };

            var context = new Mock<IHttpContextAccessor>();
            context.SetupGet(c => c.HttpContext.User.Identity.Name).Returns(username);

            var authService = new Mock<IAuthorizationService>();
            authService.Setup(a => a.RemoveRefreshToken(username, logoutCommand.RefreshToken));

            var logoutHandler = new LogoutCommand.LogoutHandler(context.Object, authService.Object);

            // Action
            var result = await logoutHandler.Handle(logoutCommand, new System.Threading.CancellationToken());

            // Assert
            if (username != null)
                authService.Verify(a => a.RemoveRefreshToken(username, logoutCommand.RefreshToken), Times.Once());
            if (username == null)
                authService.Verify(a => a.RemoveRefreshToken(username, logoutCommand.RefreshToken), Times.Never());
        }
    }
}
