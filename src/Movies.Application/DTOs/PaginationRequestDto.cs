namespace Movies.Application.DTOs
{
    /// <summary>
    /// Represents a pagination request with page and size parameters.
    /// </summary>
    public class PaginationRequestDto
    {
        /// <summary>
        /// Page number (1-based). Default is 1.
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items per page. Default is 20, maximum is 100.
        /// </summary>
        public int Size { get; set; } = 20;

        /// <summary>
        /// Optional search text to filter movies by title or genre.
        /// </summary>
        public string? SearchText { get; set; }
    }
}
