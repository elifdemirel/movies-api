using MongoDB.Driver;
using Movies.Domain;
using Movies.Infrastructure.Settings;

namespace Movies.Infrastructure
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(DatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<Movie> Movies
            => _database.GetCollection<Movie>("movies");

        public IMongoCollection<Director> Directors
            => _database.GetCollection<Director>("directors");
    }
}
