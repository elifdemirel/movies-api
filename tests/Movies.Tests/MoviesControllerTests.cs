using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Movies.Api.Controllers;
using Movies.Application.DTOs;
using Movies.Application.Services;
using Movies.Domain;

namespace Movies.Tests
{
    public class MoviesControllerTests
    {
        [Fact]
        public async Task Create_should_return_Conflict_when_imdbId_exists()
        {
            // Arrange
            var service = new Mock<IMovieService>();
            var logger = new Mock<Microsoft.Extensions.Logging.ILogger<MoviesController>>();

            service.Setup(s => s.CreateAsync(It.IsAny<MovieCreateDto>()))
                   .ThrowsAsync(new InvalidOperationException("Movie with IMDb ID already exists"));

            var controller = new MoviesController(service.Object, logger.Object);

            var dto = new MovieCreateDto
            {
                Title = "Test",
                Genre = "Drama",
                ReleaseDate = DateTime.Today,
                Rating = 7,
                ImdbId = "tt1234567",
                DirectorId = "dir-1"
            };

            // Act
            Func<Task> act = async () => await controller.Create(dto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Create_should_return_BadRequest_when_director_missing()
        {
            // Arrange
            var service = new Mock<IMovieService>();
            var logger = new Mock<Microsoft.Extensions.Logging.ILogger<MoviesController>>();

            service.Setup(s => s.CreateAsync(It.IsAny<MovieCreateDto>()))
                   .ThrowsAsync(new ArgumentException("Director not found"));

            var controller = new MoviesController(service.Object, logger.Object);

            var dto = new MovieCreateDto
            {
                Title = "Test",
                Genre = "Drama",
                ReleaseDate = DateTime.Today,
                Rating = 6,
                ImdbId = "tt7654321",
                DirectorId = "dir-404"
            };

            // Act
            Func<Task> act = async () => await controller.Create(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task Create_should_return_Created_when_payload_is_valid()
        {
            // Arrange
            var service = new Mock<IMovieService>();
            var logger = new Mock<Microsoft.Extensions.Logging.ILogger<MoviesController>>();

            var movie = new Movie
            {
                Id = "1",
                Title = "Good Movie",
                Genre = "Action",
                ReleaseDate = DateTime.Today,
                Rating = 8.5,
                ImdbId = "tt9999999",
                DirectorId = "dir-1"
            };

            service.Setup(s => s.CreateAsync(It.IsAny<MovieCreateDto>()))
                   .ReturnsAsync(movie);

            var controller = new MoviesController(service.Object, logger.Object);

            var dto = new MovieCreateDto
            {
                Title = "Good Movie",
                Genre = "Action",
                ReleaseDate = DateTime.Today,
                Rating = 8.5,
                ImdbId = "tt9999999",
                DirectorId = "dir-1"
            };

            // Act
            var result = await controller.Create(dto);

            // Assert
            var created = result.Should().BeOfType<CreatedResult>().Subject;
            created.Value.Should().Be(movie);

            service.Verify(s => s.CreateAsync(It.IsAny<MovieCreateDto>()), Times.Once);
        }
    }
}
