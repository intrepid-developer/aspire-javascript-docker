using AspireJs.Api;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add Postgres database.
builder.AddNpgsqlDataSource("movies");

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Call Seed Data with retry logic for database readiness
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
    var database = new Database(db);
    const int maxRetries = 10;
    int retries = 0;
    while (true)
    {
        try
        {
            database.Seed();
            break;
        }
        catch (Npgsql.NpgsqlException) when (retries < maxRetries)
        {
            retries++;
            Console.WriteLine($"Database not ready, retrying in 2s... ({retries}/{maxRetries})");
            Thread.Sleep(2000);
        }
    }
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapMovies();

app.MapDefaultEndpoints();

app.Run();