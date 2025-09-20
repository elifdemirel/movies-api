using Movies.Application.DTOs;
using Movies.Domain;

namespace Movies.Application.Services
{
    public interface IDirectorService
    {
        Task<Director> CreateAsync(DirectorCreateDto dto);
        Task DeleteAsync(string id);
    }
}
