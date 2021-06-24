using BugTracker.API.Commands;
using BugTracker.API.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Threading;
using BugTracker.API.Services;
using System.Collections.Generic;
using System.Security.Claims;
using BugTracker.API.Models.Responses;
using Microsoft.IdentityModel.Tokens;

namespace BugTracker.Api.Tests.Commands
{
    public class RefreshCommandTests
    {
        [Fact]
        public async Task HandleRetrieveTokenSuccess()
        {
            // Arrange
            var refreshCommand = new RefreshCommand { ExpiredToken = "ExpiredToken", RefreshToken = "RefreshToken" };
            var user = new ApplicationUser { UserName = "TestUser" };
            var refreshToken = new RefreshToken { Expires = DateTime.Now.AddDays(1) };
            var tokenResult = new TokenResult();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(u => u.FindByNameAsync(user.UserName)).ReturnsAsync(user);

            var claims = new Mock<ClaimsPrincipal>();
            claims.SetupGet(c => c.Identity.Name).Returns(user.UserName);

            var tokenService = new Mock<ITokenService>();
            tokenService.Setup(t => t.GetPrincipalFromToken(refreshCommand.ExpiredToken)).Returns(claims.Object);

            var authService = new Mock<IAuthorizationService>();
            authService.Setup(a => a.RemoveRefreshToken(user.UserName, refreshCommand.RefreshToken)).ReturnsAsync(refreshToken);
            authService.Setup(a => a.AuthorizeAsync(user)).ReturnsAsync(tokenResult);

            var refreshHander = new RefreshCommand.RefreshHandler(tokenService.Object, userManager.Object, authService.Object);

            // Action
            var result = await refreshHander.Handle(refreshCommand, new CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tokenResult, result);
            userManager.Verify(u => u.FindByNameAsync(user.UserName), Times.Once());
            tokenService.Verify(t => t.GetPrincipalFromToken(refreshCommand.ExpiredToken), Times.Once());
            authService.Verify(a => a.RemoveRefreshToken(user.UserName, refreshCommand.RefreshToken), Times.Once());
            authService.Verify(a => a.AuthorizeAsync(user), Times.Once());
        }

        [Fact]
        public async Task HandleRetrieveTokenExpiredRefreshToken()
        {
            // Arrange
            var refreshCommand = new RefreshCommand { ExpiredToken = "ExpiredToken", RefreshToken = "RefreshToken" };
            var user = new ApplicationUser { UserName = "TestUser" };
            var refreshToken = new RefreshToken { Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1)) };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var claims = new Mock<ClaimsPrincipal>();
            claims.SetupGet(c => c.Identity.Name).Returns(user.UserName);

            var tokenService = new Mock<ITokenService>();
            tokenService.Setup(t => t.GetPrincipalFromToken(refreshCommand.ExpiredToken)).Returns(claims.Object);

            var authService = new Mock<IAuthorizationService>();
            authService.Setup(a => a.RemoveRefreshToken(user.UserName, refreshCommand.RefreshToken)).ReturnsAsync(refreshToken);

            var refreshHander = new RefreshCommand.RefreshHandler(tokenService.Object, userManager.Object, authService.Object);

            // Action
            var result = await refreshHander.Handle(refreshCommand, new CancellationToken());

            // Assert
            Assert.Null(result);
            userManager.Verify(u => u.FindByNameAsync(user.UserName), Times.Never());
            tokenService.Verify(t => t.GetPrincipalFromToken(refreshCommand.ExpiredToken), Times.Once());
            authService.Verify(a => a.RemoveRefreshToken(user.UserName, refreshCommand.RefreshToken), Times.Once());
            authService.Verify(a => a.AuthorizeAsync(user), Times.Never());
        }

        [Fact]
        public async Task HandkeRetrieveTokenInvalidAccessToken()
        {
            // Arrange
            var refreshCommand = new RefreshCommand { ExpiredToken = "InvalidToken", RefreshToken = "RefreshToken" };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var tokenService = new Mock<ITokenService>();
            tokenService.Setup(t => t.GetPrincipalFromToken(refreshCommand.ExpiredToken)).Throws(new SecurityTokenException());

            var authService = new Mock<IAuthorizationService>();

            var refreshHander = new RefreshCommand.RefreshHandler(tokenService.Object, userManager.Object, authService.Object);

            // Action
            var result = await refreshHander.Handle(refreshCommand, new CancellationToken());

            // Assert
            Assert.Null(result);
            userManager.Verify(u => u.FindByNameAsync(It.IsAny<string>()), Times.Never());
            tokenService.Verify(t => t.GetPrincipalFromToken(refreshCommand.ExpiredToken), Times.Once());
            authService.Verify(a => a.RemoveRefreshToken(It.IsAny<string>(), refreshCommand.RefreshToken), Times.Never());
            authService.Verify(a => a.AuthorizeAsync(It.IsAny<ApplicationUser>()), Times.Never());
        }
    }
}
