using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Jokes.Application.Interfaces;
using Jokes.Application.Models;
using Jokes.Application.Services;
using Jokes.Application.Shared;
using Jokes.Application.UnitTests.Helpers;
using Jokes.Infrastructure.Interfaces;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using infra = Jokes.Infrastructure.Models;

namespace Jokes.Application.UnitTests
{
    public class JokesServiceUnitTests
    {
        private Mock<ILogger<JokesService>> _logger;
        private Mock<IApiService> _apiService;
        private Mock<IJokesValidator> _validator;
        private Mock<IJokesRepository> _repository;
        private List<infra.JokeDto> _validJokesDto;
        private List<Joke> _validJokes;
        private IMapper _mapper;
        private JokesService _jokesService;


        [SetUp]
        public void SetUp()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }).CreateMapper();
            _logger = new Mock<ILogger<JokesService>>();
            _apiService = new Mock<IApiService>();
            _validator = new Mock<IJokesValidator>();
            _repository = new Mock<IJokesRepository>();
            _validJokesDto= JokesServiceTestsHelper.GetJokesDto();
            _validJokes = JokesServiceTestsHelper.GetJokes();

            _jokesService = new JokesService(_logger.Object, _apiService.Object, _validator.Object, _repository.Object,
                _mapper);
        }

        [Test]
        public async Task GivenValidListOfJokes_JokesAreProcessedSuccessfully()
        {
            _apiService.Setup(x => x.GetJokesAsync(It.IsAny<int>())).ReturnsAsync(_validJokesDto);
            _validator.Setup(x => x.ValidateJokes(It.IsAny<List<Joke>>())).Returns(_validJokes);
            _repository.Setup(x => x.IsJokeExistAsync(It.IsAny<infra.Joke>())).ReturnsAsync(false);

            await _jokesService.ProcessJokes(It.IsAny<int>());

            _repository.Verify(x=>x.SaveJokeAsync(It.IsAny<infra.Joke>()), Times.Exactly(2));
        }

        [Test]
        public async Task GivenEmptyListOfJokes_JokesProcessingEnds()
        {
            _apiService.Setup(x => x.GetJokesAsync(It.IsAny<int>())).ReturnsAsync(new List<infra.JokeDto>());

            await _jokesService.ProcessJokes(It.IsAny<int>());

            _validator.Verify(x=>x.ValidateJokes(It.IsAny<List<Joke>>()), Times.Never);
            _repository.Verify(x=>x.IsJokeExistAsync(It.IsAny<infra.Joke>()), Times.Never);
            _repository.Verify(x=>x.SaveJokeAsync(It.IsAny<infra.Joke>()), Times.Never);
        }

        [Test]
        public void GivenFailedApiService_ExceptionIsThrown()
        {
            _apiService.Setup(x => x.GetJokesAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            Assert.ThrowsAsync<Exception>(() => _jokesService.ProcessJokes(It.IsAny<int>()));
        }


        [Test]
        public async Task GivenExistingJokesInDb_ExistingJokeIsNotSaved()
        {
            _apiService.Setup(x => x.GetJokesAsync(It.IsAny<int>())).ReturnsAsync(_validJokesDto);
            _validator.Setup(x => x.ValidateJokes(It.IsAny<List<Joke>>())).Returns(_validJokes);
            _repository.Setup(x => x.IsJokeExistAsync(It.IsAny<infra.Joke>())).ReturnsAsync(true);

            await _jokesService.ProcessJokes(It.IsAny<int>());

            _repository.Verify(x => x.SaveJokeAsync(It.IsAny<infra.Joke>()), Times.Never);
        }

        [Test]
        public async Task GivenEmptyListOfValidJokes_JokesProcessingEnds()
        {
            _apiService.Setup(x => x.GetJokesAsync(It.IsAny<int>())).ReturnsAsync(_validJokesDto);
            _validator.Setup(x => x.ValidateJokes(It.IsAny<List<Joke>>())).Returns(new List<Joke>());

            await _jokesService.ProcessJokes(It.IsAny<int>());

            _repository.Verify(x => x.IsJokeExistAsync(It.IsAny<infra.Joke>()), Times.Never);
            _repository.Verify(x => x.SaveJokeAsync(It.IsAny<infra.Joke>()), Times.Never);
        }
    }
}