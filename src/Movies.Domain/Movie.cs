using MongoDB.Bson;

namespace Movies.Domain
{
    public class Movie
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Genre { get; set; }
        public double Rating { get; set; }
        public required string ImdbId { get; set; }
        public required string DirectorId { get; set; }
    }
}
