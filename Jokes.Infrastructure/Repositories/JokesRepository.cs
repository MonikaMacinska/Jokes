using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;
using Jokes.Infrastructure.Configuration;
using Jokes.Infrastructure.Interfaces;
using Jokes.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace Jokes.Infrastructure.Repositories
{
    public class JokesRepository : IJokesRepository
    {
        private readonly DatabaseSettings _settings;

        public JokesRepository(IOptions<DatabaseSettings> settings)
        {
            _settings = settings == null
                ? throw new ArgumentException(nameof(settings))
                : settings.Value;
        }

        //private string _connection = "Data Source=C:\\Users\\macinskm\\source\\repos\\Jokes\\Jokes.Infrastructure\\Database\\JokesDb.db;Version=3;";

        public void SaveJoke(Joke jokeDto)
        {
            using IDbConnection connection= new SQLiteConnection(_settings.ConnectionString);
            connection.Execute("INSERT INTO Joke (Id, Hash, IconUrl, Url, Value) VALUES (@Id, @HashedId, @IconUrl, @Url, @Value)", jokeDto);
        }

        public async Task<int> SaveJokeAsync (Joke jokeDto)
        {
            using IDbConnection connection = new SQLiteConnection(_settings.ConnectionString);
            return await connection.ExecuteAsync("INSERT INTO Joke (Id, Hash, IconUrl, Url, Value) VALUES (@Id, @HashedId, @IconUrl, @Url, @Value)", jokeDto);
        }

        public bool IsJokeExist(Joke jokeDto)
        {
            using IDbConnection connection = new SQLiteConnection(_settings.ConnectionString);
            return connection.ExecuteScalar<bool>("SELECT COUNT(1) FROM Joke WHERE Hash=@HashedId", jokeDto);
        }

        public async Task<bool> IsJokeExistAsync (Joke jokeDto)
        {
            using IDbConnection connection = new SQLiteConnection(_settings.ConnectionString);
            return await connection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Joke WHERE Hash=@HashedId", jokeDto);
        }
    }
}