using System.Threading.Tasks;
using Jokes.Infrastructure.Models;

namespace Jokes.Infrastructure.Interfaces
{
    public interface IJokesRepository
    {
        void SaveJoke(Joke joke);
        bool IsJokeExist(Joke joke);
        Task<int> SaveJokeAsync(Joke joke);
        Task<bool> IsJokeExistAsync(Joke joke);
    }
}