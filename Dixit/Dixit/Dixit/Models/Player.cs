using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixit.Models
{
    public class Player
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public string Name { get; set; }

        public IEnumerable<int> Cards { get; set; }

        public Votes Votes { get; set; }

        public int SubmittedCard { get; set; }

        public bool StoryTeller { get; set; }

        public bool HasToldStory { get; set; }
    }
}
