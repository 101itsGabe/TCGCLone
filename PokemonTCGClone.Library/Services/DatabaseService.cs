using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonTCGClone.Library.Services
{
    public class DatabaseService
    {
        public static DatabaseService _cardService;
        public List<string> imageUrls;
        private static DatabaseService _instance;
        public DatabaseService() 
        {
            imageUrls = new List<string>();
        }

        public static DatabaseService Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance =  new DatabaseService();
                }
                return _instance;
            }
        }

        public void Add(string url)
        {
            imageUrls.Add(url);
        }

        public List<String> GetList()
        {
           return imageUrls;
        }
    }
        
}
