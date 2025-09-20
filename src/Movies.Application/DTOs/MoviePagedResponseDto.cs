using Movies.Domain;

namespace Movies.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for paginated movie responses.
    /// </summary>
    public class MoviePagedResponseDto
    {
        /// <summary>List of movies for the current page.</summary>
        public IEnumerable<Movie> Items { get; set; } = Array.Empty<Movie>();

        /// <summary>Current page number (1-based).</summary>
        public int Page { get; set; }

        /// <summary>Number of items per page.</summary>
        public int Size { get; set; }

        /// <summary>Search text used for filtering (if any).</summary>
        public string? SearchText { get; set; }

        /// <summary>Total number of movies matching the criteria.</summary>
        public long Total { get; set; }

        /// <summary>Total number of pages.</summary>
        public int TotalPages { get; set; }

        /// <summary>Indicates if there is a next page.</summary>
        public bool HasNext { get; set; }

        /// <summary>Indicates if there is a previous page.</summary>
        public bool HasPrevious { get; set; }
    }
}
