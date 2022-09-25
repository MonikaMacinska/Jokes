using System.Collections.Generic;
using System.Text;
using Jokes.Application.Models;

namespace Jokes.Application.Helpers
{
    public static class JokesHelper
    {
        public static void HashJokesId(IEnumerable<Joke> jokes)
        {
            foreach (var joke in jokes)
            {
                using var md5 = System.Security.Cryptography.MD5.Create();
                var inputBytes = Encoding.ASCII.GetBytes(joke.Id);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var t in hashBytes)
                {
                    sb.Append(t.ToString("X2"));
                }
                joke.HashedId = sb.ToString();
            }
        }
    }
}