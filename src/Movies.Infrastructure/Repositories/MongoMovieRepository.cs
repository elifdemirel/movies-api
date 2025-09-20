using MongoDB.Driver;
using Movies.Domain;
using Movies.Application.Repositories;

namespace Movies.Infrastructure.Repositories
{
    public class MongoMovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<Movie> _collection;

        public MongoMovieRepository(MongoContext context)
        {
            _collection = context.Movies;
        }

        public async Task<Movie?> GetByIdAsync(string id)
        {
            return await _collection.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Movie> Items, long Total)> GetPagedAsync(int page, int size, string? searchText = null)
        {
            var skipCount = (page - 1) * size;
            var filter = Builders<Movie>.Filter.Empty;

            // Regex search for flexible partial matching
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var titleFilter = Builders<Movie>.Filter.Regex(m => m.Title, new MongoDB.Bson.BsonRegularExpression(searchText, "i"));
                var genreFilter = Builders<Movie>.Filter.Regex(m => m.Genre, new MongoDB.Bson.BsonRegularExpression(searchText, "i"));
                var searchFilter = Builders<Movie>.Filter.Or(titleFilter, genreFilter);
                
                filter = Builders<Movie>.Filter.And(filter, searchFilter);
            }
            
            var totalCount = await _collection.CountDocumentsAsync(filter);
            var movies = await _collection
                .Find(filter)
                .Skip(skipCount)
                .Limit(size)
                .ToListAsync();

            return (movies, totalCount);
        }

        public async Task<Movie?> GetByImdbIdAsync(string imdbId)
        {
            return await _collection.Find(m => m.ImdbId == imdbId).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Movie movie)
        {
            await _collection.InsertOneAsync(movie);
        }

        public async Task UpdateAsync(Movie movie)
        {
            await _collection.ReplaceOneAsync(m => m.Id == movie.Id, movie);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(m => m.Id == id);
        }

        public async Task<bool> ExistsByDirectorIdAsync(string directorId)
        {
            var filter = Builders<Movie>.Filter.Eq(m => m.DirectorId, directorId);
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}
