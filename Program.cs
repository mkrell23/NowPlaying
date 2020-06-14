using System;
using System.Collections.Generic;
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

            //THIS IS WHAT WAS "WORKING"

            // UserInteraction.MainMenu();
            // var movieChoice = Console.ReadLine();

            // var result = WebInteraction.SearchUtelly(movieChoice);

            // var normal = result.Content.Normalize();
            // var content = JsonConvert.DeserializeObject<JObject>(result.Content);

            UserInteraction.MainMenu();
            var movieChoice = Console.ReadLine();

            var result = WebInteraction.SearchOmdb(movieChoice);

            var content = JsonConvert.DeserializeObject<JObject>(result);


            //THIS IS HACKY AND BAD

            // var frankenstein = JToken.Parse(content);

            // foreach (KeyValuePair<string, JToken> subContent in (JObject)content[0])
            // {
            //     Console.WriteLine(subContent.Key);
            // }

            

            WebInteraction.PrintKeys();
            Console.WriteLine("Hello World!");

         //I DON'T EVEN REMEMBER IF THIS WORKED AT ALL

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
