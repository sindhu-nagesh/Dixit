using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dixit.Services;
using Dixit.Models;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dixit.Controllers
{
    [EnableCors("dixitui")]
    [Route("api/game/{gameId}/[controller]")]
    public class PlayerController : ControllerBase
    {
        private PlayerService _service = new PlayerService();

        [HttpGet]
        public Player[] List(int gameId)
        {
            return _service.List(gameId).ToArray();
        }

        [HttpGet("{id}")]
        public Player Get(int gameId, int id)
        {
            return _service.Get(id);
        }

        [HttpPut]
        public Player Create(int gameId, [FromBody] PlayerInput playerInput)
        {
            return _service.Create(gameId, playerInput.Name);
        }

        [HttpPut("{id}")]
        public Player Update(int id, [FromBody] Player game)
        {
            _service.Update(id, game);
            return game;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Remove(id);
        }

        [HttpPost("{id}")]
        public void SubmitCard(int id, [FromBody] int cardId)
        {
        }

        [HttpPost("{id}")]
        public void SubmitVotes(int id, [FromBody] List<int> cardIds)
        {
        }
    }
}
