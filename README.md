# Movies API - Case Study

A modern .NET 9 Web API for managing movies and directors with MongoDB and Redis caching.

## Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Run with Docker
```bash
git clone <repository-url>
cd movies-api
docker compose up --build
```

**Access Points:**
- **API**: http://localhost:8080/api
- **Swagger UI**: http://localhost:8080/swagger
- **MongoDB UI**: http://localhost:8081

## Tech Stack

| Component | Technology | Purpose |
|-----------|------------|---------|
| **API** | .NET 9 | Web API framework |
| **Database** | MongoDB 7 | Document storage |
| **Cache** | Redis 7 | Response caching |
| **Validation** | FluentValidation | Input validation |
| **Testing** | xUnit + Moq | Unit testing |
| **Docs** | Swagger/OpenAPI | API documentation |

## API Endpoints

### Movies
```bash
# Get movies with pagination and search
GET /api/movies?page=1&size=10&searchText=action

# Create movie
POST /api/movies
{
  "title": "The Matrix",
  "description": "A computer hacker learns the truth",
  "releaseDate": "1999-03-31T00:00:00Z",
  "genre": "Sci-Fi",
  "rating": 8.7,
  "imdbId": "tt0133093",
  "directorId": "director-id"
}

# Update movie
PUT /api/movies/{id}

# Delete movie
DELETE /api/movies/{id}
```

### Directors
```bash
# Create director
POST /api/directors
{
  "firstName": "Lana",
  "secondName": "Wachowski",
  "birthDate": "1965-06-21T00:00:00Z",
  "bio": "American film director"
}

# Delete director
DELETE /api/directors/{id}
```

## Features

### Search & Pagination
- **Search**: Case-insensitive search in movie titles and genres
- **Pagination**: Configurable page size (1-100 items)
- **Caching**: 5-minute Redis cache with smart invalidation

**Example Response:**
```json
{
  "items": [...],
  "page": 1,
  "size": 10,
  "total": 150,
  "totalPages": 15,
  "hasNext": true,
  "hasPrevious": false,
  "searchText": "action"
}
```

## Database

### Collections
**Movies**: `title`, `description`, `releaseDate`, `genre`, `rating`, `imdbId`, `directorId`  
**Directors**: `firstName`, `secondName`, `birthDate`, `bio`

### Indexes
- `movies.imdbId` (unique) - Prevents duplicates
- `movies.directorId` - Foreign key performance

## Testing

```bash
# Run all tests
dotnet test
```

## Configuration

### Environment Variables
| Variable | Default | Description |
|----------|---------|-------------|
| `MongoDB__ConnectionString` | mongodb://mongo:27017 | Database connection |
| `MongoDB__DatabaseName` | MoviesDb | Database name |
| `Redis__Connection` | redis:6379 | Cache connection |
| `ASPNETCORE_ENVIRONMENT` | Development | Environment |

## Docker Services

| Service | Port | Purpose |
|---------|------|---------|
| movies-api | 8080 | Main API |
| mongo | 27017 | Database |
| mongo-express | 8081 | DB Management |
| redis | 6379 | Caching |

## Troubleshooting

**Connection Issues:**
```bash
# Check services
docker compose ps

# View logs
docker compose logs movies-api

# Restart services
docker compose restart
```

## Project Structure

```
movies-api/
├── src/
│   ├── Movies.Api/             # Controllers, middleware
│   ├── Movies.Application/     # DTOs, interfaces
│   ├── Movies.Domain/          # Entities
│   └── Movies.Infrastructure/  # Data access, services
├── tests/
│   └── Movies.Tests/           # Unit tests
├── docker-compose.yml          # Service orchestration
└── README.md                   # This file
```

---

<div align="center">

**Built with ❤️ for HubX Backend Developer Case Study**

*Clean Architecture • MongoDB • Redis • .NET 9*

</div>