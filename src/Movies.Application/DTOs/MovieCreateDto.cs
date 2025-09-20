namespace Movies.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new movie.
    /// </summary>
    public class MovieCreateDto
    {
        /// <summary>The title of the movie.</summary>
        public required string Title { get; set; }

        /// <summary>A brief description of the movie.</summary>
        public string? Description { get; set; }

        /// <summary>The release date of the movie.</summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>The genre of the movie (e.g., Drama, Action).</summary>
        public required string Genre { get; set; }

        /// <summary>The IMDb rating of the movie (scale 1–10).</summary>
        public double Rating { get; set; }

        /// <summary>The IMDb identifier (e.g., tt1234567).</summary>
        public required string ImdbId { get; set; }

        /// <summary>The unique identifier of the director.</summary>
        public required string DirectorId { get; set; }
    }
}