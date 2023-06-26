using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonTCGClone.Library.Services;

namespace PokemonTCGClone.Library.Models
{
    public class Game
    {
        public Player P1 { get; set; }
        public Player P2 { get; set; }

        public DatabaseService dbs;

        public Game()
        {
            dbs = DatabaseService.Current;
            P1 = new Player();
            //P2 = new Player();
        }

        
    }
}
