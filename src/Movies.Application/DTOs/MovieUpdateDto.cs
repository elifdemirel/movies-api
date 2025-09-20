namespace Movies.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for updating an existing movie.
    /// </summary>
    public class MovieUpdateDto
    {
        /// <summary>The title of the movie.</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>Description of the movie.</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>Release date of the movie.</summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>Genre of the movie.</summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>IMDb rating between 0 and 10.</summary>
        public double Rating { get; set; }

        /// <summary>IMDb identifier in format tt1234567.</summary>
        public string ImdbId { get; set; } = string.Empty;

        /// <summary>MongoDB ObjectId of the director.</summary>
        public string DirectorId { get; set; } = string.Empty;
    }
}