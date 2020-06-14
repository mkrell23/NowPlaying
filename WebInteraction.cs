using System;
using System.IO;
using Newtonsoft.Json;
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

        //Initializes the API keys in the Key files into the class

        static WebInteraction()
        {
            using (StreamReader utellykey = new StreamReader("UtellyKey.txt") )
            using (StreamReader omdbkey = new StreamReader("OmdbKey.txt") )
            {
               _utellyKey = utellykey.ReadToEnd();
               _omdbKey = omdbkey.ReadToEnd();
            }
        }
        
        public static void PrintKeys()
        {
            Console.WriteLine( "Utelly Key: " + WebInteraction._utellyKey);
            Console.WriteLine( "OMDB Key: " + WebInteraction._omdbKey);
        }

        public static Result[] SearchUtelly(string searchTerms)
        {
            var client = new RestClient($"https://utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com/lookup?term={searchTerms}&country=us");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", WebInteraction._utellyKey);
            IRestResponse response = client.Execute(request);

            UtellyResult utellyResultConv = UtellyResult.FromJson(response.Content);

            return utellyResultConv.Results;
        }

        public static OmdbResult SearchOmdb(string searchTerms)
        {
            string response = "";
            string url = $"http://www.omdbapi.com/?apikey={_omdbKey}&t={HttpUtility.UrlEncode(searchTerms)}";

            using (WebClient wc = new WebClient())
            {
               response = wc.DownloadString(url);
            }
           
            return OmdbResult.FromJson(response);          
        }


        //TODO: Move this to proper class?
        public static void SaveMovieToFile(String movie, string fileName)
        {
            var serializer = new JsonSerializer();
            using (StreamWriter writer = new StreamWriter(fileName))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(writer, movie);               
            }          
        }

//This got borked in an auto refactor when the original Movie class got moved to the UtellyResult class.
//What did this do? I'll need to recreate it's purported functionality anyway.

        // public static UtellyResult ReadMovieInfo(String result)
        // {
        //     UtellyResult movie = new UtellyResult();

        //     using (var reader = new StreamReader(result))
        //     {
        //         string line = "";
        //         while ((line = reader.ReadLine()) != null)
        //         {
        //             string[] values = line.Split(',');
        //         }
        //     }
        //     return movie;
        // }
    }
}