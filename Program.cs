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


            OmdbResult CorrectOmdbResult = WebInteraction.SearchOmdb(movieChoice); 

            var r = CorrectOmdbResult.Title;


            WebInteraction.PrintKeys();
            Console.WriteLine("Hello World!");

            return 0;
        }
    }
}
