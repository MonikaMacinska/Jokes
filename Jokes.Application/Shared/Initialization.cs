using Jokes.Application.Configuration;
using Jokes.Application.Interfaces;
using Jokes.Application.Services;
using Jokes.Application.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jokes.Application.Shared
{
    public static class Initialization
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IJokesService, JokesService>();
            services.AddScoped<IJokesValidator, JokesValidator>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddOptions<JokesSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(nameof(JokesSettings)).Bind(settings);
                });

            return services;
        }

    }
}