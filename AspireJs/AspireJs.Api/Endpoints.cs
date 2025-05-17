using Dapper;
using Npgsql;

namespace AspireJs.Api;

public static class Endpoints
{
    public static WebApplication MapMovies(this WebApplication app)
    {
        app.MapGet("/movies", async (NpgsqlConnection db) =>
        {
            const string sql = """
                               SELECT Id, Title, Year, Genre
                               FROM Movies
                               """;

            return await db.QueryAsync<Movie>(sql);
        });

        app.MapGet("/movies/{id}", async (int id, NpgsqlConnection db) =>
        {
            const string sql = """
                               SELECT Id, Title, Year, Genre
                               FROM Movies
                               WHERE Id = @id
                               """;

            return await db.QueryFirstOrDefaultAsync<Movie>(sql, new { id }) is { } todo
                ? Results.Ok(todo)
                : Results.NotFound();
        });

        app.MapPost("/movies", async (Movie movie, NpgsqlConnection db) =>
        {
            const string sql = """
                               INSERT INTO Movies (Title, Year, Genre)
                               VALUES (@Title, @Year, @Genre)
                               RETURNING Id
                               """;

            var id = await db.ExecuteScalarAsync<int>(sql, movie);
            return Results.Created($"/movies/{id}", id);
        });

        app.MapPut("/movies/{id}", async (int id, Movie movie, NpgsqlConnection db) =>
        {
            const string sql = """
                               UPDATE Movies
                               SET  Title = @Title,
                                    Year = @Year,
                                    Genre = @Genre
                               WHERE Id = @id
                               """;

            var rowsAffected = await db.ExecuteAsync(sql, new { movie.Title, movie.Year, movie.Genre, id });
            return rowsAffected == 1 ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}