using Microsoft.Extensions.Logging;
using Movies.Application.DTOs;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Domain;

namespace Movies.Infrastructure.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _directorRepo;
        private readonly IMovieRepository _movieRepo;
        private readonly ILogger<DirectorService> _logger;

        public DirectorService(IDirectorRepository directorRepo, 
            IMovieRepository movieRepo,
            ILogger<DirectorService> logger)
        {
            _directorRepo = directorRepo;
            _movieRepo = movieRepo;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Director> CreateAsync(DirectorCreateDto dto)
        {
            var director = new Director
            {
                FirstName = dto.FirstName,
                SecondName = dto.SecondName,
                BirthDate = dto.BirthDate,
                Bio = dto.Bio
            };

            await _directorRepo.AddAsync(director);
            _logger.LogInformation("Created new director {DirectorId} ({FirstName} {SecondName})",
                                   director.Id, director.FirstName, director.SecondName);

            return director;
        }

        public async Task DeleteAsync(string id)
        {
            _logger.LogInformation("Attempting to delete director {DirectorId}", id);
            var hasMovies = await _movieRepo.ExistsByDirectorIdAsync(id);
            if (hasMovies)
            {
                _logger.LogWarning("Cannot delete director {DirectorId} because movies exist", id);
                throw new InvalidOperationException("Cannot delete director with existing movies");
            }

            var director = await _directorRepo.GetByIdAsync(id);
            if (director == null)
            {
                _logger.LogWarning("Director {DirectorId} not found", id);
                throw new KeyNotFoundException($"Director {id} not found");
            }

            await _directorRepo.DeleteAsync(id);
            _logger.LogInformation("Successfully deleted director {DirectorId}", id);
        }
    }
}
