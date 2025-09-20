using FluentValidation;
using FluentValidation.AspNetCore;
using Movies.Api.Middleware;
using Movies.Infrastructure;
using Movies.Infrastructure.Indexes;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddSwaggerGen(options =>
{
    // Include XML comments from API project
    var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
    if (File.Exists(apiXmlPath))
        options.IncludeXmlComments(apiXmlPath, includeControllerXmlComments: true);

    // Include XML comments from Application project
    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, "Movies.Application.xml");
    if (File.Exists(applicationXmlPath))
        options.IncludeXmlComments(applicationXmlPath);
});

builder.Services.AddInfrastructure(builder.Configuration);

// Redis cache registration
var redisConnection = builder.Configuration.GetValue<string>("Redis:Connection")
    ?? Environment.GetEnvironmentVariable("REDIS_CONNECTION")
    ?? "redis:6379";
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});
builder.Services.AddLogging();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MongoContext>();
    var initializer = new MongoIndexInitializer(context);
    initializer.CreateIndexes();
}

app.UseGlobalExceptionHandler();

app.MapControllers();
app.Run();
