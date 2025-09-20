using Movies.Domain;

namespace Movies.Application.DTOs
{
    /// <summary>
    /// Represents a cached paginated response for movies.
    /// </summary>
    public class PagedMoviesResponseDto
    {
        /// <summary>
        /// The movies for the current page.
        /// </summary>
        public Movie[] Items { get; set; } = Array.Empty<Movie>();

        /// <summary>
        /// Total number of movies across all pages.
        /// </summary>
        public long Total { get; set; }
    }
}
