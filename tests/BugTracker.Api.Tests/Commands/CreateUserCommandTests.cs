using AutoMapper;
using BugTracker.API.Commands;
using BugTracker.API.Models;
using BugTracker.API.Models.Responses;
using BugTracker.API.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BugTracker.Api.Tests.Commands
{
    public class CreateUserCommandTests
    {
        [Fact]
        public async Task HandleCreateUserSuccess()
        {
            // Arrange
            var command = new CreateUserCommand { Email = "validEmail", Password = "validPassword", Username = "validUsername" };

            var identityResult = IdentityResult.Success;
            var userService = new Mock<IUserService>();
            userService.Setup(u => u.CreateUserAsync(
                It.Is<ApplicationUser>(u => u.Email == command.Email && u.UserName == command.Username && u.IsActivated == true),
                command.Password, false)
            ).ReturnsAsync(identityResult);

            var userResponse = new UserResponse { Email = "validEmail", Username = "validUsername" };
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<ApplicationUser, UserResponse>(It.IsAny<ApplicationUser>())).Returns(userResponse);

            var handler = new CreateUserCommand.CreateUserHandler(userService.Object, mapper.Object);

            // Action
            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(userResponse, result.Model);
            userService.Verify(u => u.CreateUserAsync(It.IsAny<ApplicationUser>(), command.Password, false), Times.Once());
            mapper.Verify(m => m.Map<ApplicationUser, UserResponse>(It.IsAny<ApplicationUser>()), Times.Once());
        }

        [Fact]
        public async Task HandleCreateUserInvalid()
        {
            // Arrange
            var command = new CreateUserCommand { Email = "invalidEmail", Password = "invalidPassword", Username = "invalidUsername" };

            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Error" });
            var userService = new Mock<IUserService>();
            userService.Setup(u => u.CreateUserAsync(
                It.Is<ApplicationUser>(u => u.Email == command.Email && u.UserName == command.Username && u.IsActivated == true),
                command.Password, false)
            ).ReturnsAsync(identityResult);

            var mapper = new Mock<IMapper>();

            var handler = new CreateUserCommand.CreateUserHandler(userService.Object, mapper.Object);

            // Action
            var result = await handler.Handle(command, new System.Threading.CancellationToken());

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Null(result.Model);
            Assert.NotNull(result.Errors.FirstOrDefault(e => e == "Error"));
            userService.Verify(u => u.CreateUserAsync(It.IsAny<ApplicationUser>(), command.Password, false), Times.Once());
            mapper.Verify(m => m.Map<ApplicationUser, UserResponse>(It.IsAny<ApplicationUser>()), Times.Never());
        }
    }
}
