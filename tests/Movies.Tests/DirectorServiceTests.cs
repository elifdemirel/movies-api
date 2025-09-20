using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Movies.Application.Repositories;
using Movies.Domain;
using Movies.Infrastructure.Services;

namespace Movies.Tests
{
    public class DirectorServiceTests
    {
        [Fact]
        public async Task DeleteAsync_should_throw_when_movies_exist()
        {
            // Arrange
            var directorRepo = new Mock<IDirectorRepository>();
            var movieRepo = new Mock<IMovieRepository>();
            var logger = new Mock<ILogger<DirectorService>>();

            movieRepo.Setup(r => r.ExistsByDirectorIdAsync("dir-1")).ReturnsAsync(true);

            var service = new DirectorService(directorRepo.Object, movieRepo.Object, logger.Object);

            // Act
            Func<Task> act = async () => await service.DeleteAsync("dir-1");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task DeleteAsync_should_throw_when_director_not_found()
        {
            // Arrange
            var directorRepo = new Mock<IDirectorRepository>();
            var movieRepo = new Mock<IMovieRepository>();
            var logger = new Mock<ILogger<DirectorService>>();

            movieRepo.Setup(r => r.ExistsByDirectorIdAsync("dir-404")).ReturnsAsync(false);
            directorRepo.Setup(r => r.GetByIdAsync("dir-404")).ReturnsAsync((Director?)null);

            var service = new DirectorService(directorRepo.Object, movieRepo.Object, logger.Object);

            // Act
            Func<Task> act = async () => await service.DeleteAsync("dir-404");

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_should_succeed_when_director_exists_and_no_movies()
        {
            // Arrange
            var directorRepo = new Mock<IDirectorRepository>();
            var movieRepo = new Mock<IMovieRepository>();
            var logger = new Mock<ILogger<DirectorService>>();

            movieRepo.Setup(r => r.ExistsByDirectorIdAsync("dir-1")).ReturnsAsync(false);
            directorRepo.Setup(r => r.GetByIdAsync("dir-1"))
                        .ReturnsAsync(new Director { Id = "dir-1", FirstName = "Jane", SecondName = "Doe", BirthDate = DateTime.Today });

            directorRepo.Setup(r => r.DeleteAsync("dir-1")).Returns(Task.CompletedTask);

            var service = new DirectorService(directorRepo.Object, movieRepo.Object, logger.Object);

            // Act
            await service.DeleteAsync("dir-1");

            // Assert
            directorRepo.Verify(r => r.DeleteAsync("dir-1"), Times.Once);
        }
    }
}
