using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dixit.Models;
using Dixit.Controllers.Services;
using Dixit.Services;
using Microsoft.AspNetCore.Cors;

namespace Dixit.Controllers
{
    [EnableCors("dixitui")]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private GameService _service = new GameService();

        [HttpGet("{id}")]
        public Game Get(int id)
        {
            return _service.Get(id);
        }

        [HttpPut]
        public Game Create()
        {
            return _service.Create();
        }

        [HttpPut("{id}")]
        public Game Update(int id, [FromBody] Game game)
        {
            _service.Update(id, game);
            return game;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Remove(id);
        }

        [HttpPost("{id}/setup")]
        public Game Setup(int id)
        {
            return _service.Setup(id);           
        }
    }
}
