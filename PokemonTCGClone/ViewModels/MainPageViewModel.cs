using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonTCGClone.Library;
using PokemonTCGClone.Library.Models;

namespace PokemonTCGClone.ViewModels
{
    public class MainPageViewModel
    {
        Card c { get; set; }
        Game game { get; set; }

        public ObservableCollection<Card> P1Deck 
        { get { return new ObservableCollection<Card>(game.P1.curDeck.GetCards()); } }
        public string cardName
        {
            get { return c.CName; }
        }

        public string cardHP
        { get { return c.HP.ToString(); } }

        public string cardBox1
        {
            get { return c.Box1Text; }
        }

        public string cardBox1Name
        {
            get { return c.Box1Name; }
        }

        public string cardBox2
        {
            get { return c.Box2Text; }
        }

        public string cardBox2Name
        {
            get { return c.Box2Name; }
        }
        public string hasAbility
        { get 
            {
                if (c.hasAbilityBox1)
                    return "true";
                else
                    return "false"; 
            } }
        public string cardFilepath
        {
            get
            {
                return c.filePath;
            }
        }

        public MainPageViewModel()
        {
            c = new Card();
            game = new Game();
        }
    }
}
