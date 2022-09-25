using System.Threading.Tasks;
using Jokes.Infrastructure.Models;

namespace Jokes.Infrastructure.Interfaces
{
    public interface IJokesRepository
    {
        Task<int> SaveJokeAsync(Joke joke);
        Task<bool> IsJokeExistAsync(Joke joke);
    }
}