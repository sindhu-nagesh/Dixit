using Dixit.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dixit.Caches
{

    public class PlayerCache
    {
        /// <summary>
        /// Map of player Id and player properties
        /// </summary>
        private IDictionary<int, Player> Players { get; set; }


        public static PlayerCache Instance = new PlayerCache
        {
            Players = new Dictionary<int, Player>(),
            
        };
       
        public void Add(int playerId, Player value)
        {
            Players.Add(playerId, value);
        }

        public Player Get(int playerId)
        {
            Players.TryGetValue(playerId, out var value);
            return value;
        }

        public IEnumerable<Player> List(int gameId)
        {
            return Players.Values.Where(player => player.GameId == gameId);
        }

        public IEnumerable<Player> List()
        {
            return Players.Values;
        }

        public void Remove(int playerId)
        {
            Players.Remove(playerId);
        }

        public void Update(int playerId, Player value)
        {
            Players[playerId] = value;
        }
    }
}
