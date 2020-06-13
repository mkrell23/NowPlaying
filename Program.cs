using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace NowPlaying
{
    class Program
    {
        static int Main(string[] args)
        {

            // UserInteraction.DisplayWelcome();
            
            // var selection = Console.ReadKey();
            // if (selection.Key == ConsoleKey.Escape)
            // {
            //     return 0;
            // }
            // else
            // {      
            //     Console.Clear();    
            //     movieChoice = UserInteraction.MainMenu();
            // }

            UserInteraction.MainMenu();
            var movieChoice = Console.ReadLine();

            
            // UserInteraction.MainMenu();
            // string selection = Console.ReadLine();
            // Console.WriteLine($"You picked {selection}");
            
            var result = WebInteraction.SearchUtelly(movieChoice);
            var content = JsonConvert.DeserializeObject(result.Content);

            WebInteraction.PrintUtellyKey();
            Console.WriteLine("Hello World!");

            //    var movie = Movie.FromJson(result.Content);

         
     
        // //    Movie bojack = WebInteraction.ReadMovieInfo(movie.);
        // //    var bojack = Movie.DeserializeMovie(content);

        // //    JToken token = JToken.Parse(result.Content);
        // //    JObject json = JObject.Parse( token);
             
            
        //     WebInteraction.ReadMovieInfo(content);

        //   WebInteraction.SaveMovieToFile(content, "bojack.json") ; 

            return 0;
        }
    }
}
