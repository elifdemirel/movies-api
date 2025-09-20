using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Movies.Application.DTOs;
using Movies.Application.Repositories;
using Movies.Domain;
using Movies.Infrastructure.Services;

namespace Movies.Tests
{
    public class MovieServiceTests
    {
        [Fact]
        public async Task CreateAsync_should_throw_when_director_missing()
        {
            // Arrange
            var movieRepo = new Mock<IMovieRepository>();
            var directorRepo = new Mock<IDirectorRepository>();

            directorRepo.Setup(r => r.GetByIdAsync("dir-404"))
                        .ReturnsAsync((Director?)null);

            var cache = new Mock<IDistributedCache>();
            var logger = new Mock<ILogger<MovieService>>();
            var service = new MovieService(movieRepo.Object, directorRepo.Object, cache.Object, logger.Object);

            var dto = new MovieCreateDto
            {
                Title = "Test",
                Genre = "Drama",
                ReleaseDate = DateTime.Today,
                Rating = 7,
                ImdbId = "tt1111111",
                DirectorId = "dir-404"
            };

            // Act
            Func<Task> act = async () => await service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("*not found*");
        }

        [Fact]
        public async Task CreateAsync_should_throw_when_duplicate_imdbId()
        {
            // Arrange
            var movieRepo = new Mock<IMovieRepository>();
            var directorRepo = new Mock<IDirectorRepository>();

            directorRepo.Setup(r => r.GetByIdAsync("dir-1"))
                        .ReturnsAsync(new Director { Id = "dir-1", FirstName = "Jane", SecondName = "Doe", BirthDate = DateTime.Now });

            movieRepo.Setup(r => r.GetByImdbIdAsync("tt1234567"))
                     .ReturnsAsync(new Movie { Id = "m-1", ImdbId = "tt1234567", Title = "Existing", Genre = "Drama", DirectorId = "dir-1" });

            var cache = new Mock<IDistributedCache>();
            var logger = new Mock<ILogger<MovieService>>();
            var service = new MovieService(movieRepo.Object, directorRepo.Object, cache.Object, logger.Object);

            var dto = new MovieCreateDto
            {
                Title = "Test",
                Genre = "Drama",
                ReleaseDate = DateTime.Today,
                Rating = 8,
                ImdbId = "tt1234567",
                DirectorId = "dir-1"
            };

            // Act
            Func<Task> act = async () => await service.CreateAsync(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*already exists*");
        }

        [Fact]
        public async Task CreateAsync_should_return_movie_when_valid()
        {
            // Arrange
            var movieRepo = new Mock<IMovieRepository>();
            var directorRepo = new Mock<IDirectorRepository>();

            directorRepo.Setup(r => r.GetByIdAsync("dir-1"))
                        .ReturnsAsync(new Director { Id = "dir-1", FirstName = "Jane", SecondName = "Doe", BirthDate = DateTime.Now });

            movieRepo.Setup(r => r.GetByImdbIdAsync("tt9999999"))
                     .ReturnsAsync((Movie?)null);

            movieRepo.Setup(r => r.AddAsync(It.IsAny<Movie>())).Returns(Task.CompletedTask);

            var cache = new Mock<IDistributedCache>();
            var logger = new Mock<ILogger<MovieService>>();
            var service = new MovieService(movieRepo.Object, directorRepo.Object, cache.Object, logger.Object);

            var dto = new MovieCreateDto
            {
                Title = "Good Movie",
                Genre = "Action",
                ReleaseDate = DateTime.Today,
                Rating = 9,
                ImdbId = "tt9999999",
                DirectorId = "dir-1"
            };

            // Act
            var movie = await service.CreateAsync(dto);

            // Assert
            movie.Should().NotBeNull();
            movie.ImdbId.Should().Be("tt9999999");
            movie.Title.Should().Be("Good Movie");

            movieRepo.Verify(r => r.AddAsync(It.IsAny<Movie>()), Times.Once);
        }
    }
}
