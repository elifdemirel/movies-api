using Movies.Application.DTOs;
using Movies.Domain;

namespace Movies.Application.Services
{
    public interface IMovieService
    {
        Task<Movie> CreateAsync(MovieCreateDto dto);
        Task<Movie> UpdateAsync(string id, MovieUpdateDto dto);
        Task DeleteAsync(string id);
        Task<(IEnumerable<Movie> Items, long Total)> GetPagedAsync(int page, int size, string? searchText = null);
    }
}
