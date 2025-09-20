using MongoDB.Driver;
using Movies.Domain;

namespace Movies.Infrastructure.Indexes
{
    public class MongoIndexInitializer
    {
        private readonly MongoContext _context;

        public MongoIndexInitializer(MongoContext context)
        {
            _context = context;
        }

        public void CreateIndexes()
        {
            // IMDb ID unique index for Movies
            var movieImdbIndexKeys = Builders<Movie>.IndexKeys.Ascending(m => m.ImdbId);
            var movieImdbIndexOptions = new CreateIndexOptions { Unique = true };
            var movieImdbIndexModel = new CreateIndexModel<Movie>(movieImdbIndexKeys, movieImdbIndexOptions);
            _context.Movies.Indexes.CreateOne(movieImdbIndexModel);

            // DirectorId index for Movies (for ExistsByDirectorIdAsync performance)
            var movieDirectorIndexKeys = Builders<Movie>.IndexKeys.Ascending(m => m.DirectorId);
            var movieDirectorIndexModel = new CreateIndexModel<Movie>(movieDirectorIndexKeys);
            _context.Movies.Indexes.CreateOne(movieDirectorIndexModel);
        }
    }
}
