using Microsoft.AspNetCore.Mvc;
using Movies.Application.DTOs;
using Movies.Application.Services;
using Movies.Domain;

namespace Movies.Api.Controllers
{
    /// <summary>
    /// Provides operations for managing directors.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _service;
        private readonly ILogger<DirectorsController> _logger;

        public DirectorsController(IDirectorService service, ILogger<DirectorsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new director.
        /// </summary>
        /// <param name="dto">The director details.</param>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Director), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] DirectorCreateDto dto)
        {
            _logger.LogInformation("Creating new director {FirstName} {SecondName}", dto.FirstName, dto.SecondName);
            var director = await _service.CreateAsync(dto);
            return Created("api/directors", director);
        }

        /// <summary>
        /// Deletes a director by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the director.</param>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting director {Id}", id);
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
