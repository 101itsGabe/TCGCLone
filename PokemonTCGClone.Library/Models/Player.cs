using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTCGClone.Library.Models
{
    public class Player
    {
        public Deck curDeck { get; set; }
        public string PName { get; set; }

        public Player() 
        {
            curDeck = new Deck();
            PName = "Player1";
        }

    }
}
