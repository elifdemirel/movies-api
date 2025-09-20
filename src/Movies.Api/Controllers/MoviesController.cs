using Microsoft.AspNetCore.Mvc;
using Movies.Application.DTOs;
using Movies.Application.Services;
using Movies.Domain;

namespace Movies.Api.Controllers
{
    /// <summary>
    /// Provides operations for managing movies.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMovieService service, ILogger<MoviesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves movies with pagination and optional search functionality.
        /// </summary>
        /// <param name="request">Pagination request including optional search text</param>
        /// <returns>Paginated list of movies</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MoviePagedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDto request)
        {
            var logMessage = string.IsNullOrWhiteSpace(request.SearchText) 
                ? "Fetching movies - Page: {Page}, Size: {Size}"
                : "Fetching movies with search - Page: {Page}, Size: {Size}, SearchText: {SearchText}";
            
            _logger.LogInformation(logMessage, request.Page, request.Size, request.SearchText);

            var (movies, totalCount) = await _service.GetPagedAsync(request.Page, request.Size, request.SearchText);
            
            var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);
            var response = new MoviePagedResponseDto
            {
                Items = movies,
                Page = request.Page,
                Size = request.Size,
                SearchText = request.SearchText,
                Total = totalCount,
                TotalPages = totalPages,
                HasNext = request.Page < totalPages,
                HasPrevious = request.Page > 1
            };

            return Ok(response);
        }

        /// <summary>
        /// Creates a new movie.
        /// </summary>
        /// <param name="dto">The movie details.</param>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] MovieCreateDto dto)
        {
            _logger.LogInformation("Creating a new movie with IMDb ID {ImdbId}", dto.ImdbId);

            var movie = await _service.CreateAsync(dto);

            return Created("api/movies", movie);
        }

        /// <summary>
        /// Updates an existing movie.
        /// </summary>
        /// <param name="id">The unique identifier of the movie.</param>
        /// <param name="dto">The updated movie details.</param>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(string id, [FromBody] MovieUpdateDto dto)
        {
            _logger.LogInformation("Updating movie with Id {Id}", id);

            var updated = await _service.UpdateAsync(id, dto);

            return Ok(updated);
        }

        /// <summary>
        /// Deletes a movie by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the movie.</param>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting movie with Id {Id}", id);
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
