using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace NowPlaying
{
    class Program
    {
        static void Main(string[] args)
        {
           var result = WebInteraction.SearchUtelly("bojack");
           var content = JsonConvert.DeserializeObject<String>(result.Content);
           var movie = Movie.FromJson(result.Content);

         
     
        //    Movie bojack = WebInteraction.ReadMovieInfo(movie2.);
        //    var bojack = Movie.DeserializeMovie(content);

        //    JToken token = JToken.Parse(result.Content);
        //    JObject json = JObject.Parse( token);
             
            
            WebInteraction.ReadMovieInfo(content);

          WebInteraction.SaveMovieToFile(content, "bojack.json") ; 
            WebInteraction.PrintUtellyKey();
            Console.WriteLine("Hello World!");
  
        }
    }
}
