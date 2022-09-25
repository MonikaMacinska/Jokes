using System.Collections.Generic;
using Joke = Jokes.Application.Models.Joke;

namespace Jokes.Application.Interfaces
{
    public interface IJokesValidator
    {
        List<Joke> ValidateJokes(List<Joke> jokes);
    }
}