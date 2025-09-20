namespace Movies.Infrastructure.Settings
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string MoviesCollectionName { get; set; } = "movies";
        public string DirectorsCollectionName { get; set; } = "directors";
    }
}
