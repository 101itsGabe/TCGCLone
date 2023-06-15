using OpenCvSharp;
using OpenCvSharp.XImgProc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace PokemonTCGClone.Library.Models
{
    public class Deck
    {
        public List<Card> cards { get; set; }
        public string name;
        public string cardPath = "C:\\Users\\manng\\Desktop\\TCGClone\\PokemonTCGClone\\Resources\\Images\\cardpngs\\";
        public Deck() 
        {
            cards = new List<Card>();
            string[] files = Directory.GetFiles(cardPath, "*.png", SearchOption.AllDirectories);
            foreach (string file in files) 
            {
                cards.Add(new Card(file));
            }
        }

        public List<Card> GetCards()
        {
            return cards;
        }
    }
}
