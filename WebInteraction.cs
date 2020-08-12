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
        // Utelly gives us the streaming providers
        // https://rapidapi.com/utelly/api/utelly
        private static string _utellyKey;
        
        // OMDB gives us lots of wonderful data about movies, shows, and games. We don't care about the games.
        // http://www.omdbapi.com/
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

        // Searches Utelly by title. Not currently used in favor of using the more accurate IMDB ID
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

        // This is the method we use to get our list of streaming providers using the IMDB ID
        // Note: country code must be lowercase!
        public static UtellyResultById SearchUtellyById(string searchTerms)
        {
            var client = new RestClient($"https://utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com/idlookup?country=us&source_id={searchTerms}&source=imdb");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", _utellyKey);
            IRestResponse response = client.Execute(request);

            UtellyResultById utellyResultConv = UtellyResultById.FromJson(response.Content);

            return utellyResultConv;
        }

        // This search returns one result, useful as a fallback when OMDB gives us a "too many results" error
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

        // Our main search, returns a very nice list.
        // Search supports pages ("&page=(1-100)" default is 1) not currently implemented.
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

        // Returns more data at the expense of only returning one result.
        // Gives us the critic ratings, plot, actors, and many other interesting details.
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