using System.Threading.Tasks;

namespace Jokes.Application.Interfaces
{
    public interface IJokesService
    {
        Task ProcessJokes(int jokesCount);
    }
}