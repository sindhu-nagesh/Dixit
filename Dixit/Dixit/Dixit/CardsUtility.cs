using Dixit.Caches;
using Dixit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixit
{
    public static class CardsUtility
    {
        public const int PLAYER_HAND_CAPCACITY = 4;
        private static ServiceFactory _serivceFactory = ServiceFactory.Instance;
        private static GameCache _gameCache = GameCache.Instance;

        public static IEnumerable<int> Shuffle()
        {
            int shuffleCount = _serivceFactory.Random.Next(15);

            IEnumerable<int> cards = new List<int>(Enumerable.Range(1, 500));

            for (int i = 0; i < shuffleCount; i++)
            {
                cards = cards.OrderBy(x => _serivceFactory.Random.Next());
            }
            return cards;
        }

        public static IEnumerable<int> GetPlayerHand(int gameId)
        {
            Game game = _gameCache.Get(gameId);
            IEnumerable<int> cards = game.Cards;
            return null;
        }

        public static int GetCardFromDeck()
        {
            return 0;
        }
    }
}

