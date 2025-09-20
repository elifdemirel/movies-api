using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Movies.Application.DTOs;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Domain;
using System.Text.Json;

namespace Movies.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IDirectorRepository _directorRepo;
        private readonly IDistributedCache _cache;
        private readonly ILogger<MovieService> _logger;
        private const string CacheVersionKey = "movies:cache:version";

        public MovieService(
            IMovieRepository movieRepo, 
            IDirectorRepository directorRepo,
            IDistributedCache cache,
            ILogger<MovieService> logger)
        {
            _movieRepo = movieRepo ?? throw new ArgumentNullException(nameof(movieRepo));
            _directorRepo = directorRepo ?? throw new ArgumentNullException(nameof(directorRepo));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Movie> CreateAsync(MovieCreateDto dto)
        {
            var director = await _directorRepo.GetByIdAsync(dto.DirectorId);
            if (director == null)
                throw new ArgumentException($"Director {dto.DirectorId} not found");

            var existing = await _movieRepo.GetByImdbIdAsync(dto.ImdbId);
            if (existing != null)
                throw new InvalidOperationException($"Movie with IMDb ID {dto.ImdbId} already exists");

            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                ReleaseDate = dto.ReleaseDate,
                Genre = dto.Genre,
                Rating = dto.Rating,
                ImdbId = dto.ImdbId,
                DirectorId = dto.DirectorId
            };

            await _movieRepo.AddAsync(movie);
            await InvalidateMoviesCache("create");
            return movie;
        }

        public async Task<Movie> UpdateAsync(string id, MovieUpdateDto dto)
        {
            var movie = await _movieRepo.GetByIdAsync(id);
            if (movie == null)
                throw new KeyNotFoundException($"Movie {id} not found");

            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.ReleaseDate = dto.ReleaseDate;
            movie.Genre = dto.Genre;
            movie.Rating = dto.Rating;
            movie.ImdbId = dto.ImdbId;
            movie.DirectorId = dto.DirectorId;

            await _movieRepo.UpdateAsync(movie);
            await InvalidateMoviesCache("update");
            return movie;
        }

        public async Task DeleteAsync(string id)
        {
            var movie = await _movieRepo.GetByIdAsync(id);
            if (movie == null)
                throw new KeyNotFoundException($"Movie {id} not found");

            await _movieRepo.DeleteAsync(id);
            await InvalidateMoviesCache("delete");
        }

        public async Task<(IEnumerable<Movie> Items, long Total)> GetPagedAsync(int page, int size, string? searchText = null)
        {
            var cacheVersion = await GetCacheVersionAsync();
            var searchKey = string.IsNullOrWhiteSpace(searchText) ? "all" : searchText.ToLowerInvariant();
            var cacheKey = $"movies:v{cacheVersion}:page:{page}:size:{size}:search:{searchKey}";
            
            _logger.LogInformation("Cache key: {CacheKey}", cacheKey);

            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cached))
                {
                    var cachedResult = JsonSerializer.Deserialize<PagedMoviesResponseDto>(cached);
                    if (cachedResult != null)
                        return (cachedResult.Items ?? Array.Empty<Movie>(), cachedResult.Total);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to read paged movies from cache; falling back to repository");
            }

            var result = await _movieRepo.GetPagedAsync(page, size, searchText);
            
            try
            {
                var cacheData = new PagedMoviesResponseDto 
                { 
                    Items = result.Items.ToArray(), 
                    Total = result.Total 
                };
                var serialized = JsonSerializer.Serialize(cacheData);
                await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to write paged movies to cache");
            }

            return result;
        }

        private async Task InvalidateMoviesCache(string operation)
        {
            try 
            { 
                var newVersion = Guid.NewGuid().GetHashCode();
                await _cache.SetStringAsync(CacheVersionKey, newVersion.ToString());
                
                _logger.LogInformation("Movies cache invalidated after {Operation}", operation);
            }
            catch (Exception ex) 
            { 
                _logger.LogWarning(ex, "Failed to invalidate movies cache after {Operation}", operation); 
            }
        }

        private async Task<int> GetCacheVersionAsync()
        {
            try
            {
                var versionString = await _cache.GetStringAsync(CacheVersionKey);
                if (int.TryParse(versionString, out var version))
                    return version;
                
                await _cache.SetStringAsync(CacheVersionKey, "1");
                return 1;
            }
            catch (Exception ex)
            { 
                _logger.LogWarning(ex, "Failed to get cache version, using default version 1");
                return 1; 
            }
        }

    }
}
