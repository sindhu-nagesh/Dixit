using Dixit.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;

namespace Dixit.Caches
{

    public class GameCache
    {
        private IDictionary<int, Game> Values { get; set; }

        public static GameCache Instance = new GameCache
        {
            Values = new Dictionary<int, Game>()
        };
       
        public void Add(int gameId, Game value)
        {
            Values.Add(gameId, value);
        }

        public Game Get(int gameId)
        {
            Values.TryGetValue(gameId, out var value);
            return value;
        }

        public void Remove(int gameId)
        {
            Values.Remove(gameId);
        }

        public void Update(int gameId, Game value)
        {
            Values[gameId] = value;
        }
    }
}
