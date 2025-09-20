using Movies.Domain;

namespace Movies.Application.Repositories
{
    public interface IDirectorRepository
    {
        Task<Director?> GetByIdAsync(string id);
        Task<IEnumerable<Director>> GetAllAsync();
        Task AddAsync(Director director);
        Task DeleteAsync(string id);
    }
}
