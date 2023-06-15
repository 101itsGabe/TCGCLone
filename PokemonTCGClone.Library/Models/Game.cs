using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTCGClone.Library.Models
{
    public class Game
    {
        public Player P1 { get; set; }
        public Player P2 { get; set; }

        public Game()
        {
            P1 = new Player();
            //P2 = new Player();
        }

        
    }
}
