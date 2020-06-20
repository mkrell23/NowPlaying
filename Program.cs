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

            userSearch = ParseThroughOmdbResults(OmdbSearchResults);

            var selectedId = UserInteraction.DisplayAndReturnSelection(movieChoice, userSearch);

            Result[] oneToStream = WebInteraction.SearchUtellyById(selectedId);

            var wTF = WebInteraction.SearchUtellyById("tt0417299");

            // Console.WriteLine(oneToStream[0].Locations[0].Url);

            //OmdbResult omResult = WebInteraction.SearchOmdbForTitle(movieChoice);

            // var hu = userSearch[0];
            // var r = omResult.Type;
            // var whazit = OmdbSearchResults[0].Title;

            Result[] utellyMovie = WebInteraction.SearchUtelly(movieChoice);
            var pleaseHelpMe = WebInteraction.SearchUtellyById(utellyMovie[0].ExternalIds.Imdb.Id);
            // var pic = utellyMovie[0].Picture;
            // var url = utellyMovie[0].Locations[0].Url;
            // var ugh = utellyMovie[0].Locations[0].Name;

            // userSearch = ParseThroughUtellyResults(utellyMovie);


            WebInteraction.PrintKeys();
            Console.WriteLine("Hello World!");

            return 0;
        }

        static List<Movie> ParseThroughOmdbResults(Search[] results)
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
                Task.Delay(600);  
            }

            return userSearch;
        }


        //PROBLEM: How to add Location[] to movie???
        //APPROACH: use other search (results are better anyway), pick result, call utelly to get providers
        static List<Movie> ParseThroughUtellyResults(Result[] results)
        {
            //NONE OF THIS IS WORKING :sad:
            List<Movie> userSearch = new List<Movie>();
            foreach (var result in results)
            {
                Movie movie = new Movie
                {
                    Title = result.Name,
                    ImdbId = result.Id
                };

                // movie.locations[] = Array.CreateInstance(string, 4);
                // Array.Copy(result.Locations, movie.Locations, 3);

                
                // foreach (var provider in Locations)
                // {
                //     movie.
                // }
            }

            return userSearch;
        }
    }
}
