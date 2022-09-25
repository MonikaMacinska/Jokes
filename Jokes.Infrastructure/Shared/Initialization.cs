using Jokes.Infrastructure.Configuration;
using Jokes.Infrastructure.Interfaces;
using Jokes.Infrastructure.Repositories;
using Jokes.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jokes.Infrastructure.Shared
{
    public static class Initialization
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IApiService, ChuckNorrisApiService>();
            services.AddScoped<IJokesRepository, JokesRepository>();
            services.AddHttpClient();
            services.AddOptions<ApiSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(nameof(ApiSettings)).Bind(settings);
                });
            services.AddOptions<DatabaseSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(nameof(DatabaseSettings)).Bind(settings);
                });
            return services;
        }
    }
}