using Dixit.Caches;
using Dixit.Models;
using Dixit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixit.Controllers.Services
{
    public class GameService
    {
        private GameCache _gameCache = GameCache.Instance;
        private PlayerCache _playerCache = PlayerCache.Instance;
        private ServiceFactory _serivceFactory = ServiceFactory.Instance;

        public Game Get(int id)
        {
            return _gameCache.Get(id);
        }

        public Game Create()
        {
            int id = _serivceFactory.Random.Next(1000);
            while(_gameCache.Get(id) != null)
            {
                id = _serivceFactory.Random.Next(1000);
            }           

            var game = new Game
            {
                Id = id,
                Scores = new Dictionary<string, int>()
            };

            _gameCache.Add(id, game);
            return game;
        }

        public Game Setup(int id)
        {
            Game game = _gameCache.Get(id);

            SetupGame(game);
            SetupPlayerScores(game);
            DealPlayerCards(game);
            ChooseStoryTeller(game);
            _gameCache.Update(game.Id, game);

            return game;
        }

        //public Game Play(int id)
        //{
        //    Game game = _gameCache.Get(id);
        
        //    CollectCards(game);
        //    CalculateScores(game);
        //    bool isGameEnd = game.Scores.Where(x => x.Value >= 30).Any();
        //    if (!isGameEnd)
        //    {
        //        ChooseStoryTeller(game);
        //    }
        //    else
        //    {
        //        game.StoryTellerId = -1;
        //    }

        //    _gameCache.Update(game.Id, game);

        //    return game;

        //}

        public void Play(int id)
        {
            Game game = _gameCache.Get(id);

            switch (game.GameAction)
            {
                case GameActions.Setup:
                    SetupGame(game);
                    SetupPlayerScores(game);
                    DealPlayerCards(game);
                    ChooseStoryTeller(game);
                    game.GameAction = GameActions.CalculateScores;
                    _gameCache.Update(game.Id, game);
                    break;

                case GameActions.CalculateScores:
                    CollectCards(game);
                    CalculateScores(game);
                    bool isGameEnd = game.Scores.Where(x => x.Value >= 30).Any();
                    if(!isGameEnd)
                    {
                        ChooseStoryTeller(game);
                    }
                    game.GameAction = isGameEnd ? GameActions.End : GameActions.CalculateScores;
                    _gameCache.Update(game.Id, game);
                    break;

                default:
                    break;
            }
        }

        private void ChooseStoryTeller(Game game)
        {
            IEnumerable<Player> players = GetPlayers(game.Id);
            IEnumerable<Player> playersWhoHaveNotToldAStory = players.Where(x => x.HasToldStory != true);

            if(!playersWhoHaveNotToldAStory.Any())
            {
                // All have told a story, reset
                foreach(Player player in players)
                {
                    player.HasToldStory = false;
                }
                playersWhoHaveNotToldAStory = players;
            }

            Player chosenPlayer = playersWhoHaveNotToldAStory.First();
            chosenPlayer.StoryTeller = true;
            chosenPlayer.HasToldStory = true;
            game.StoryTellerId = chosenPlayer.Id;
        }

        private void CalculateScores(Game game)
        {
            List<Player> players = GetPlayers(game.Id).ToList();

            Player storyTeller = players.Where(x => x.StoryTeller == true).First();

            players.Remove(storyTeller);

            IDictionary<string, int> scores = game.Scores;

            var playersWhoGuessedStoryTellerCard =
                players.Where(x => x.SubmittedCard == storyTeller.SubmittedCard);

            // If all players guessed the story teller's card, all but the story teller scores 2
            // If no players guessed the story teller's card, all but the story teller scores 2
            if (playersWhoGuessedStoryTellerCard.Count() == players.Count
                || playersWhoGuessedStoryTellerCard.Count() == 0)
            {
                foreach(Player player in players)
                {
                    scores[player.Name] += 2;
                }
            } else
            {
                scores[storyTeller.Name] += 3;
                foreach (Player player in playersWhoGuessedStoryTellerCard)
                {
                    scores[player.Name] += 3;
                }
            }

            // Each player scores 1 point for every vote for their own card upto a max of 3
            foreach (Player player in players)
            {
                var points = 0;
                foreach (Player otherPlayer in players)
                {
                    if(otherPlayer.Name != player.Name
                        && otherPlayer.SubmittedCard == player.SubmittedCard
                        && points < 3)
                    {
                        points++;
                    }
                    
                }
                scores[player.Name] += points;
            }          
        }

        private void CollectCards(Game game)
        {
            IEnumerable<Player> players = GetPlayers(game.Id);

            var submittedCards = new List<int>();

            foreach(Player player in players)
            {
                submittedCards.Add(player.SubmittedCard);
            }

            game.SubmittedCards = submittedCards;
        }
        
        public void Update(int id, Game data)
        {
            _gameCache.Update(id, data);
        }

        public void Remove(int id)
        {
            _gameCache.Remove(id);
        }  
        
        private void SetupGame(Game game)
        {
            // Shuffle game deck
            game.Cards = CardsUtility.Shuffle().ToList();            
        }

        private void DealPlayerCards(Game game)
        {
            // Get Players of the game
            IEnumerable<Player> players = GetPlayers(game.Id);

            List<int> cards = game.Cards.ToList();

            foreach (Player player in players)
            {
                player.Cards = cards.Take(CardsUtility.PLAYER_HAND_CAPCACITY);
                cards.RemoveRange(0, CardsUtility.PLAYER_HAND_CAPCACITY);

                // update player
                _playerCache.Update(player.Id, player);
            }

            game.Cards = cards;
        }

        private void SetupPlayerScores(Game game)
        {
            // Get Players of the game
            IEnumerable<Player> players = GetPlayers(game.Id);

            // Initialize player scores
            var scores = new Dictionary<string, int>();
            foreach (Player player in players)
            {
                scores.Add(player.Name, 0);
            }
            game.Scores = scores;    
        }

        private IEnumerable<Player> GetPlayers(int gameId)
        {
            // Get Players of the game
            return _playerCache.List().Where(x => x.GameId == gameId);
        }
    }
}
