using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Jokes.Application.Helpers;
using Jokes.Application.Interfaces;
using Jokes.Application.Models;
using Jokes.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Jokes.Application.Services
{
    public class JokesService : IJokesService
    {
        private readonly ILogger<JokesService> _logger;
        private readonly IApiService _apiService;
        private readonly IJokesValidator _validator;
        private readonly IJokesRepository _repository;
        private readonly IMapper _mapper;

        public JokesService(
            ILogger<JokesService> logger,
            IApiService apiService,
            IJokesValidator validator,
            IJokesRepository repository,
            IMapper mapper)
        {
            _logger = logger;
            _apiService = apiService;
            _validator = validator;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task ProcessJokes(int jokesCount)
        {
            try
            {
                var jokesDto= await _apiService.GetJokesAsync(jokesCount);
                if (!jokesDto.Any())
                {
                    _logger.LogInformation("No jokes to be processed");
                    return;
                }
                _logger.LogInformation($"{jokesDto.Count} jokes have been added to the list");
                var jokes = _mapper.Map<List<Joke>>(jokesDto);

                var validJokes= _validator.ValidateJokes(jokes);
                if (!validJokes.Any())
                {
                    _logger.LogInformation("No jokes to be saved");
                    return;
                }

                JokesHelper.HashJokesId(validJokes);

                var insertedRows = 0;
                foreach (var joke in _mapper.Map<IList<Infrastructure.Models.Joke>>(validJokes))
                {
                    if (!await _repository.IsJokeExistAsync(joke))
                    {
                        var row= await _repository.SaveJokeAsync(joke);
                        insertedRows += row;
                    }
                }
                _logger.LogInformation($"{insertedRows} jokes have been inserted to DB");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Jokes processing failed with an exception: {e}");
                throw;
            }
        }

    }
}