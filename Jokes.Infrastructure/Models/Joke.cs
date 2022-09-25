﻿namespace Jokes.Infrastructure.Models
{
    public class Joke
    {
        public string Id { get; set; }
        public string HashedId { get; set; }
        public string IconUrl { get; set; }
        public string Url { get; set; }
        public string Value { get; set; }
    }
}