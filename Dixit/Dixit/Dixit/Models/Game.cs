using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixit.Models
{
    public class Game
    {
        public int Id { get; set; }

        public Enum GameAction { get; set; }

        public IDictionary<string, int> Scores { get; set; }
        
        public IEnumerable<int> SubmittedCards { get; set; }

        public IEnumerable<int> Cards { get; set; }

        public int StoryTellerId { get; set; }
    }
}
