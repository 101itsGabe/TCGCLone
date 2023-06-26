using OpenCvSharp;
using OpenCvSharp.XImgProc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using PokemonTCGClone.Library.Services;
using System.Data.Common;

namespace PokemonTCGClone.Library.Models
{
    public class Deck
    {
        public List<Card> cards { get; set; }
        public string name;
        public string cardPath = "C:\\Users\\manng\\Desktop\\TCGClone\\PokemonTCGClone\\Resources\\Images\\cardpngs\\";
        public DatabaseService dbs;
        public WebScrapingService wss;
        public Deck() 
        {
            wss = new WebScrapingService();
            wss.WebScrape();
            dbs = DatabaseService.Current;
            cards = new List<Card>();
            string[] files = Directory.GetFiles(cardPath, "*.png", SearchOption.AllDirectories);
            foreach (string file in dbs.GetList()) 
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
