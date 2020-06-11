using System;
using System.IO;
using Newtonsoft.Json;
using RestSharp;

namespace NowPlaying
{
    // This class holds the methods for calling and working with APIs.
    class WebInteraction
    {
        private static string _utellyKey;

        static WebInteraction()
        {
            using (StreamReader utellykey = new StreamReader("UtellyKey.txt") )
            {
               _utellyKey = utellykey.ReadToEnd();
            }
        }
        
        public static void PrintUtellyKey()
        {
            Console.WriteLine(WebInteraction._utellyKey);
        }

        private static string GetUtellyKey()
        {
            return _utellyKey;
        }

        public static IRestResponse SearchUtelly(string searchTerms)
        {
            var client = new RestClient($"https://utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com/lookup?term={searchTerms}&country=us");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", WebInteraction.GetUtellyKey());
            IRestResponse response = client.Execute(request);

            return response;
        }


        //TODO: Move this to proper class
        public static void SaveMovieToFile(string movie, string fileName)
        {
            var serializer = new JsonSerializer();
            using (StreamWriter writer = new StreamWriter(fileName))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(writer, movie);
               
            }
        }
    }
}