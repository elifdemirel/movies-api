namespace Movies.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating a new director.
    /// </summary>
    public class DirectorCreateDto
    {
        /// <summary>The first name of the director.</summary>
        public required string FirstName { get; set; }

        /// <summary>The last name of the director.</summary>
        public required string SecondName { get; set; }

        /// <summary>The biography of the director.</summary>
        public string? Bio { get; set; }

        /// <summary>The birth date of the director.</summary>
        public DateTime BirthDate { get; set; }
    }
}