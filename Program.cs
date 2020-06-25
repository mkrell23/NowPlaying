using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NowPlaying
{
    class Program
    {
        static int Main(string[] args)
        {

          // MAIN LOOP GOES HERE IN THE FUTURE

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

            //This is a handy spot to put a breakpoint for now because currently there is no graceful exit
            WebInteraction.PrintKeys();

            return 0;
        }

        // Generates the intial list of movies/shows we'll be using later to pick a result
        static List<Movie> CreateOmdbListOfResults(Search[] results)
        {
            List<Movie> userSearch = new List<Movie>();
            foreach (var result in results)
            {
                if(result.Type != "game") // This prevents nonsense results later
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
                    Task.Delay(1200); //OMDB does not like getting hammered
                }
            }
            return userSearch;
        }
    }
}
