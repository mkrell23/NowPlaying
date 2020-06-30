using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NowPlaying
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Movie> searchedMovieList = new List<Movie>();

            while (true)
                {
                    Console.Clear();    
                    
                // Ask for and get movie to search
                    var movieChoice = UserInteraction.GetSearchFromUser();

                // Perform serch on OMDB
                    Search[] OmdbSearchResults = WebInteraction.SearchOmdbByString(movieChoice);
                // Put results into a list of movies with titles, directors, ratings, etc
                    searchedMovieList = CreateOmdbListOfResults(OmdbSearchResults);

                // Display results and ask for user to select one
                    var selectedId = UserInteraction.DisplayAndReturnSelection(movieChoice, searchedMovieList);
                // Search for and display streaming providers for selection using Utelly
                    var oneToStream = WebInteraction.SearchUtellyById(selectedId);
                    UserInteraction.DisplayStreamingLocations(oneToStream);

                // Search again, save results to file, or exit?
                    Console.WriteLine("\r\n\r\nWould you like to search for another?\r\nType \"1\" to search again, type \"2\" to save results to file (NOT WORKING). Type \"3\" (or anything else) to quit.");
                    var menuChoice = Console.ReadLine();
                    switch (menuChoice)
                    {
                        case "1":
                            continue;
                        case "2":
                            //SAVE TO FILE!!! NOT WORKING RIGHT NOW!!
                            Console.WriteLine("Please type filename to save to (one word please)");
                            var userFileName = Console.ReadLine();
                            WebInteraction.SaveMovieToFile(oneToStream.Locations[0].Url, userFileName);
                            break;
                        case "3":
                            break;
                        default:
                            break;
                    }
                    break;
                }
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
