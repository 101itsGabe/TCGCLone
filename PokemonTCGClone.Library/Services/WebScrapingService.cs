using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PokemonTCGClone.Library.Services;

namespace PokemonTCGClone.Library.Services
{
    public class WebScrapingService
    {
        DatabaseService dbs;
        public WebScrapingService()
        {
            dbs = DatabaseService.Current;
        }
        public void WebScrape()
        {
            dbs = DatabaseService.Current;
            string url = "https://www.justinbasil.com/visual/sv2"; // Replace with the URL of the website you want to scrape images from

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);


            // Find all <img> tags in the HTML document
            var imgNodes = doc.DocumentNode.Descendants("img");

            foreach (var imgNode in imgNodes)
            {
                // Get the source attribute of the <img> tag
                string imgUrl = imgNode.GetAttributeValue("src", "");

                // Skip if the source attribute is empty or starts with "data:"
                if (!string.IsNullOrEmpty(imgUrl) && !imgUrl.StartsWith("data:"))
                {
                    // Append the base URL if the image URL is relative
                    if (!imgUrl.StartsWith("http"))
                    {
                        Uri baseUri = new Uri(url);
                        imgUrl = new Uri(baseUri, imgUrl).AbsoluteUri;
                    }

                    dbs.Add(imgUrl);
                }
            }

            // Download the images
            using (WebClient client = new WebClient())
            {
                foreach (string imageUrl in dbs.GetList())
                {
                    string fileName = GetFileNameFromUrl(imageUrl);

                    try
                    {
                        client.DownloadFile(imageUrl, fileName);
                        Console.WriteLine($"Downloaded: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error downloading {imageUrl}: {ex.Message}");
                    }
                }
            }

        }
        static string GetFileNameFromUrl(string url)
        {
            int lastSlashIndex = url.LastIndexOf('/');
            string fileName = url.Substring(lastSlashIndex + 1);
            return fileName;
        }
    }

    
}
