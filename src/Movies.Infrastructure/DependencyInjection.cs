using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Repositories;
using Movies.Application.Services;
using Movies.Infrastructure.Repositories;
using Movies.Infrastructure.Services;
using Movies.Infrastructure.Settings;

namespace Movies.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database settings
            services.Configure<DatabaseSettings>(
                configuration.GetSection("DatabaseSettings"));

            services.AddSingleton(sp =>
            {
                var settings = configuration
                    .GetSection("DatabaseSettings").Get<DatabaseSettings>()!;
                return new MongoContext(settings);
            });

            // Repositories
            services.AddScoped<IMovieRepository, MongoMovieRepository>();
            services.AddScoped<IDirectorRepository, MongoDirectorRepository>();

            // Services
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IDirectorService, DirectorService>();

            return services;
        }
    }
}