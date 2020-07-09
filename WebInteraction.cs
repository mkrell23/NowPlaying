using System;
using System.IO;
using RestSharp;
using System.Net;
using System.Web;

namespace NowPlaying
{
    // This class holds the methods for calling and working with APIs.
    class WebInteraction
    {
        private static string _utellyKey;
        private static string _omdbKey;

        //Initializes the API keys in the key files into the class
        static WebInteraction()
        {
            using (StreamReader utellykey = new StreamReader("UtellyKey.txt") )
            using (StreamReader omdbkey = new StreamReader("OmdbKey.txt") )
            {
               _utellyKey = utellykey.ReadToEnd();
               _omdbKey = omdbkey.ReadToEnd();
            }
        }
        
        // This is a test method to make sure I'm getting the proper value
        public static void PrintKeys()
        {
            Console.WriteLine( "Utelly Key: " + _utellyKey);
            Console.WriteLine( "OMDB Key: " + _omdbKey);
        }

        public static Result[] SearchUtelly(string searchTerms)
        {
            var client = new RestClient($"https://utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com/lookup?term={searchTerms}&country=us");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", _utellyKey);
            IRestResponse response = client.Execute(request);

            UtellyResult utellyResultConv = UtellyResult.FromJson(response.Content);

            return utellyResultConv.Results;
        }

        public static UtellyResultById SearchUtellyById(string searchTerms)
        {
            var client = new RestClient($"https://utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com/idlookup?country=US&source_id={searchTerms}&source=imdb");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", _utellyKey);
            IRestResponse response = client.Execute(request);

            UtellyResultById utellyResultConv = UtellyResultById.FromJson(response.Content);

            return utellyResultConv;
        }

        public static OmdbResult SearchOmdbForTitle(string searchTerms)
        {
            string response = "";
            string url = $"http://www.omdbapi.com/?apikey={_omdbKey}&t={HttpUtility.UrlEncode(searchTerms)}";

            using (WebClient wc = new WebClient())
            {
               response = wc.DownloadString(url);
            }
            
            var result = OmdbResult.FromJson(response);
            return result;
        }

        public static Search[] SearchOmdbByString(string searchTerms)
        {
            string response = "";
            string url = $"http://www.omdbapi.com/?apikey={_omdbKey}&s={HttpUtility.UrlEncode(searchTerms)}";

            using (WebClient wc = new WebClient())
            {
               response = wc.DownloadString(url);
            }
            
            var result = OmdbSearchByString.FromJson(response);
            return result.Search;
        }

        public static OmdbResult SearchOmdbForId(string searchTerms)
        {
            string response = "";
            string url = $"http://www.omdbapi.com/?apikey={_omdbKey}&i={searchTerms}";

            using (WebClient wc = new WebClient())
            {
               response = wc.DownloadString(url);
            }
            
            var result = OmdbResult.FromJson(response);
            return result;
        }

    }
}