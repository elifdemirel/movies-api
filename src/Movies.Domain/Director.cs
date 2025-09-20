using MongoDB.Bson;

namespace Movies.Domain
{
    public class Director
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public required string FirstName { get; set; }
        public required string SecondName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Bio { get; set; }
    }
}