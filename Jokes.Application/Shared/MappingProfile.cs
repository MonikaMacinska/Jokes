using AutoMapper;
using Jokes.Infrastructure.Models;

namespace Jokes.Application.Shared
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<JokeDto, Models.Joke>()
                .ForMember(x=>x.IconUrl, x=>x.MapFrom(j=>j.icon_url));
            CreateMap<Models.Joke, Joke>();
        }
    }
}