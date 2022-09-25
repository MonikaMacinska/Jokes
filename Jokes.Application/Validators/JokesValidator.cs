using System.Collections.Generic;
using Jokes.Application.Configuration;
using Jokes.Application.Interfaces;
using Jokes.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jokes.Application.Validators
{
    public class JokesValidator : IJokesValidator
    {
        private readonly ILogger<JokesValidator> _logger;
        private readonly JokesSettings _settings;

        public JokesValidator(ILogger<JokesValidator> logger, IOptions<JokesSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public List<Joke> ValidateJokes(List<Joke> jokes)
        {
            var removedJokes= jokes.RemoveAll(x => x.Value.Length > _settings.MaxCharactersCount);
            _logger.LogInformation($"{removedJokes} jokes have been removed from the list");
            return jokes;
        }

    }
}