using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            List<Movie> userSearch = new List<Movie>();

            UserInteraction.MainMenu();
            var movieChoice = Console.ReadLine();

            Search[] OmdbSearchResults = WebInteraction.SearchOmdbByString(movieChoice);

            userSearch = ParseThroughResults(OmdbSearchResults);

            var hu = userSearch[0];

            OmdbResult omResult = WebInteraction.SearchOmdbForTitle(movieChoice); 

            // var r = omResult.Type;

            // var whazit = OmdbSearchResults[0].Title;

            // Result[] utellyMovie = WebInteraction.SearchUtelly(movieChoice);

            // var pic = utellyMovie[0].Picture;
            // var url = utellyMovie[0].Locations[0].Url;


            WebInteraction.PrintKeys();
            Console.WriteLine("Hello World!");

            return 0;
        }

        static List<Movie> ParseThroughResults(Search[] results)
        {
            List<Movie> userSearch = new List<Movie>();
            foreach (var result in results)
            {
                var newResults = WebInteraction.SearchOmdbForId(result.ImdbId);
                var utellyResults = WebInteraction.SearchUtellyById(result.ImdbId);
                Movie movie = new Movie
                {
                    Title = result.Title, 
                    ImdbId = result.ImdbId, 
                    Year = (int)result.Year,
                    Poster = result.Poster,
                    Actors = newResults.Actors,
                    Director = newResults.Director,
                    ImdbRating = newResults.ImdbRating,
                    Metascore = newResults.Metascore,
                    Plot = newResults.Plot,
                    Rated =  newResults.Rated,
                    Ratings = newResults.Ratings,
                    //Locations = utellyResults.Locations
                };
                userSearch.Add(movie);
                Task.Delay(2000);  
            }

            return userSearch;
        }
    }
}
