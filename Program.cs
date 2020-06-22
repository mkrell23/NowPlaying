using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            
            var movieChoice = UserInteraction.GetSearchFromUser();
            
            Search[] OmdbSearchResults = WebInteraction.SearchOmdbByString(movieChoice);

            userSearch = CreateOmdbListOfResults(OmdbSearchResults);

            var selectedId = UserInteraction.DisplayAndReturnSelection(movieChoice, userSearch);

            var oneToStream = WebInteraction.SearchUtellyById(selectedId);

            UserInteraction.DisplayStreamingLocations(oneToStream);

                    // // This code searches and displays only utelly
                    // Result[] results = WebInteraction.SearchUtelly(movieChoice);

                    // var getAnImdbId = UserInteraction.DisplayAndReturnSelection(movieChoice, results);

                    // var finalResult = WebInteraction.SearchUtellyById(getAnImdbId);

                    // UserInteraction.DisplayStreamingLocations(finalResult);

            WebInteraction.PrintKeys();
            Console.WriteLine("Hello World!");

            return 0;
        }

        static List<Movie> CreateOmdbListOfResults(Search[] results)
        {
            List<Movie> userSearch = new List<Movie>();
            foreach (var result in results)
            {
                var newResults = WebInteraction.SearchOmdbForId(result.ImdbId);
                Movie movie = new Movie
                {
                    Title = result.Title, 
                    ImdbId = result.ImdbId, 
                    Year = result.Year,
                    Poster = result.Poster,
                    Actors = newResults.Actors,
                    Director = newResults.Director,
                    ImdbRating = newResults.ImdbRating,
                    Metascore = newResults.Metascore,
                    Plot = newResults.Plot,
                    Rated =  newResults.Rated,
                    Ratings = newResults.Ratings,
                };
                userSearch.Add(movie);
                Task.Delay(1200);  
            }

            return userSearch;
        }


        // //PROBLEM: How to add Location[] to movie???
        // //APPROACH: use other search (results are better anyway), pick result, call utelly to get providers
        // static List<Movie> ParseThroughUtellyResults(Result[] results)
        // {
        //     //NONE OF THIS IS WORKING :sad:
        //     List<Movie> userSearch = new List<Movie>();
        //     foreach (var result in results)
        //     {
        //         Movie movie = new Movie
        //         {
        //             Title = result.Name,
        //             ImdbId = result.Id
        //         };

        //         // movie.locations[] = Array.CreateInstance(string, 4);
        //         // Array.Copy(result.Locations, movie.Locations, 3);

                
        //         // foreach (var provider in Locations)
        //         // {
        //         //     movie.
        //         // }
        //     }

        //     return userSearch;
        // }
    }
}
