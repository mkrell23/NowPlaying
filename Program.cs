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

            UserInteraction.MainMenu();
            var movieChoice = Console.ReadLine();
            

            Result[] utellyMovie = WebInteraction.SearchUtelly(movieChoice);

            var pic = utellyMovie[0].Picture;
            var url = utellyMovie[0].Locations[0].Url;

            // foreach (var result in utellyMovie)
            // {
            //     result.
            // }


            OmdbResult omResult = WebInteraction.SearchOmdbForTitle(movieChoice); 
            Search[] otherOm = WebInteraction.SearchOmdbByString(movieChoice);

            var r = omResult.Type;

            var whazit = otherOm[0].Title;

            WebInteraction.PrintKeys();
            Console.WriteLine("Hello World!");

            return 0;
        }
    }
}
