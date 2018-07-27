using Dixit.Caches;
using Dixit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixit.Services
{
    public class PlayerService
    {
        private PlayerCache _playerCache = PlayerCache.Instance;
        private ServiceFactory _serivceFactory = ServiceFactory.Instance;

        public Player Get(int playerId)
        {
            return _playerCache.Get(playerId);
        }

        public IEnumerable<Player> List(int gameId)
        {
            return _playerCache.List(gameId);
        }

        public Player Create(int gameId, string name)
        {
            int playerId = _serivceFactory.Random.Next(1000);
            while (_playerCache.Get(playerId) != null)
            {
                playerId = _serivceFactory.Random.Next(1000);
            }

            var player = new Player
            {
                Id = playerId,
                GameId = gameId,
                Name = name,
            };

            _playerCache.Add(playerId, player);
            return player;
        }

        public void Update(int playerId, Player data)
        {
            _playerCache.Update(playerId, data);
        }

        public void Remove(int playerId)
        {
            _playerCache.Remove(playerId);
        }        
    }
}
