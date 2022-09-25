using System;
using System.Threading.Tasks;
using Jokes.Application.Interfaces;
using Jokes.Function.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jokes.Function
{
    public class JokesFunction
    {
        private readonly ILogger<JokesFunction> _logger;
        private readonly IJokesService _jokesService;
        private readonly FunctionSettings _settings;

        public JokesFunction(
            ILogger<JokesFunction> logger,
            IJokesService jokesService,
            IOptions<FunctionSettings> settings)
        {
            _logger = logger;
            _jokesService = jokesService;
            _settings = settings == null
                ? throw new ArgumentException(nameof(settings))
                : settings.Value;
        }

        [FunctionName("JokesFunction")]
        public async Task Run([TimerTrigger("%FunctionSettings:CronSchedule%")]TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            await _jokesService.ProcessJokes(_settings.JokesCount);
        }
    }
}
