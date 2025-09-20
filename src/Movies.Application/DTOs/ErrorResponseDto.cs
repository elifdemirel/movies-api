namespace Movies.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for error responses.
    /// </summary>
    public class ErrorResponseDto
    {
        /// <summary>The error message.</summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>The type of exception that occurred.</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>The HTTP status code.</summary>
        public int StatusCode { get; set; }

        /// <summary>The trace identifier for debugging.</summary>
        public string TraceId { get; set; } = string.Empty;
    }
}
