using System.Collections.Generic;
using Jokes.Application.Models;
using infra = Jokes.Infrastructure.Models;

namespace Jokes.Application.UnitTests.Helpers
{
    public static class JokesServiceTestsHelper
    {
        public static List<infra.JokeDto> GetJokesDto()
        {
            return new List<infra.JokeDto>()
            {
                new infra.JokeDto
                {
                    icon_url = "A",
                    id = "B",
                    url = "C",
                    value = "D"
                }
            };
        }

        public static List<Joke> GetJokes()
        {
            return new List<Joke>()
            {
                new Joke
                {
                    Id = "A",
                    HashedId = "B",
                    IconUrl = "C",
                    Url = "D",
                    Value = "E"
                },
                new Joke
                {
                    Id = "F",
                    HashedId = "G",
                    IconUrl = "H",
                    Url = "I",
                    Value = "J"
                }
            };
        }
    }
}