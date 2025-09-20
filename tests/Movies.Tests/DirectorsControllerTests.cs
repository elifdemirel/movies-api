using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Movies.Api.Controllers;
using Movies.Application.Services;

namespace Movies.Tests
{
    public class DirectorsControllerTests
    {
        [Fact]
        public async Task Delete_should_throw_InvalidOperationException_when_movies_exist()
        {
            // Arrange
            var service = new Mock<IDirectorService>();
            var logger = new Mock<Microsoft.Extensions.Logging.ILogger<DirectorsController>>();

            service.Setup(s => s.DeleteAsync("dir-1"))
                   .ThrowsAsync(new InvalidOperationException("Cannot delete director with existing movies"));

            var controller = new DirectorsController(service.Object, logger.Object);

            // Act
            Func<Task> act = async () => await controller.Delete("dir-1");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Delete_should_throw_KeyNotFoundException_when_director_missing()
        {
            // Arrange
            var service = new Mock<IDirectorService>();
            var logger = new Mock<Microsoft.Extensions.Logging.ILogger<DirectorsController>>();

            service.Setup(s => s.DeleteAsync("dir-404"))
                   .ThrowsAsync(new KeyNotFoundException("Director not found"));

            var controller = new DirectorsController(service.Object, logger.Object);

            // Act
            Func<Task> act = async () => await controller.Delete("dir-404");

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task Delete_should_return_NoContent_when_director_deleted_successfully()
        {
            // Arrange
            var service = new Mock<IDirectorService>();
            var logger = new Mock<Microsoft.Extensions.Logging.ILogger<DirectorsController>>();

            service.Setup(s => s.DeleteAsync("dir-1")).Returns(Task.CompletedTask);

            var controller = new DirectorsController(service.Object, logger.Object);

            // Act
            var result = await controller.Delete("dir-1");

            // Assert
            result.Should().BeOfType<NoContentResult>();
            service.Verify(s => s.DeleteAsync("dir-1"), Times.Once);
        }
    }
}
