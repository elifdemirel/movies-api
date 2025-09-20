using Movies.Domain;

namespace Movies.Application.Repositories
{
    public interface IMovieRepository
    {
        Task<Movie?> GetByIdAsync(string id);
        Task<(IEnumerable<Movie> Items, long Total)> GetPagedAsync(int page, int size, string? searchText = null);
        Task AddAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task DeleteAsync(string id);
        Task<Movie?> GetByImdbIdAsync(string imdbId);
        Task<bool> ExistsByDirectorIdAsync(string directorId);
    }
}
