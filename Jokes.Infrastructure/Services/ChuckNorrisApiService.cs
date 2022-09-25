using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Jokes.Infrastructure.Configuration;
using Jokes.Infrastructure.Interfaces;
using Jokes.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace Jokes.Infrastructure.Services
{
    public  class ChuckNorrisApiService : IApiService
    {
        private readonly HttpClient _client;
        private readonly ApiSettings _settings;

        public ChuckNorrisApiService(IHttpClientFactory client, IOptions<ApiSettings> settings)
        {
            _client = client.CreateClient();
            _settings = settings == null
                ? throw new ArgumentException(nameof(settings))
                : settings.Value;
        }

        public async Task<List<JokeDto>> GetJokesAsync(int jokesCount)
        {
            var jokes = new List<Task<JokeDto>>();
            for (var i = 0; i < jokesCount; i++)
            {
                var requestMessage = GetRequestMessage();
                using var response = await _client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var joke =  response.Content.ReadFromJsonAsync<JokeDto>();
                    jokes.Add(joke);
                }
                else
                {
                    throw new Exception($"Something went wrong while getting jokes: {response.StatusCode} : {response.ReasonPhrase}");
                }
            }
            return (await Task.WhenAll(jokes)).ToList();
        }

        private  HttpRequestMessage GetRequestMessage()
        {
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_settings.Uri),
                Headers =
                {
                    { _settings.KeyName, _settings.KeyValue },
                    { _settings.HostName, _settings.HostValue },
                }
            };
        }
    }
}