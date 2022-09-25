using System.Collections.Generic;
using System.Threading.Tasks;
using Jokes.Infrastructure.Models;

namespace Jokes.Infrastructure.Interfaces
{
    public interface IApiService
    {
        Task<List<JokeDto>> GetJokesAsync(int jokesCount);
    }
}