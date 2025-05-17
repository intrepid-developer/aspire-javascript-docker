using Dapper;
using Npgsql;

namespace AspireJs.Api;

public class Database(NpgsqlConnection db)
{
    public void Seed()
    {
        const string sql = """
                           CREATE TABLE IF NOT EXISTS Movies (
                               Id SERIAL PRIMARY KEY,
                               Title TEXT NOT NULL,
                               Year INT NOT NULL,
                               Genre TEXT NOT NULL
                           );
                           """;
        db.Execute(sql);

        const string insertSql = """
                                 INSERT INTO Movies (Title, Year, Genre)
                                 SELECT * FROM (VALUES
                                     ('The Shawshank Redemption', 1994, 'Drama'),
                                     ('The Godfather', 1972, 'Crime'),
                                     ('The Dark Knight', 2008, 'Action')
                                 ) AS v(Title, Year, Genre)
                                 WHERE NOT EXISTS (SELECT 1 FROM Movies);
                                 """;
        db.Execute(insertSql);
    }
}