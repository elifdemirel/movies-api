using MongoDB.Driver;
using Movies.Domain;
using Movies.Application.Repositories;

namespace Movies.Infrastructure.Repositories
{
    public class MongoDirectorRepository : IDirectorRepository
    {
        private readonly IMongoCollection<Director> _collection;

        public MongoDirectorRepository(MongoContext context)
        {
            _collection = context.Directors;
        }

        public async Task<Director?> GetByIdAsync(string id)
        {
            return await _collection.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Director>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task AddAsync(Director director)
        {
            await _collection.InsertOneAsync(director);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(d => d.Id == id);
        }
    }
}
