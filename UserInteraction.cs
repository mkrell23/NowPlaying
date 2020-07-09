using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NowPlaying
{
    //This holds the code that the user interface is built from.
    class UserInteraction
    {
        private static string _banner;

        static UserInteraction()
        {
            _banner = @"
.__   __.   ______   ____    __    ____ 
|  \ |  |  /  __  \  \   \  /  \  /   / 
|   \|  | |  |  |  |  \   \/    \/   /  
|  . `  | |  |  |  |   \            /   
|  |\   | |  `--'  |    \    /\    /    
|__| \__|  \______/      \__/  \__/     
                                        
.______    __          ___   ____    ____  __  .__   __.   _______ 
|   _  \  |  |        /   \  \   \  /   / |  | |  \ |  |  /  _____|
|  |_)  | |  |       /  ^  \  \   \/   /  |  | |   \|  | |  |  __  
|   ___/  |  |      /  /_\  \  \_    _/   |  | |  . `  | |  | |_ | 
|  |      |  `----./  _____  \   |  |     |  | |  |\   | |  |__| | 
| _|      |_______/__/     \__\  |__|     |__| |__| \__|  \______| 
__________________________________________________________________

";
        }

        public static void DisplayWelcome()
        {
            Console.WriteLine(_banner + "\tFind what streaming provider is serving a movie or show!\r\n\r\n\t\tGet critic reviews!\r\n\r\n\t\t\tSave and load movie lists!\r\n\r\n\t\t\t\tPress any key to start");
        }

        public static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine(_banner + "Type \"F\" to find a movie or show.\r\nType \"L\" to load a movie list from a JSON file.\r\nType \"Q\" to quit.");
            var menuChoice = Console.ReadLine().ToUpper().Trim();
            while (menuChoice != "F" && menuChoice != "L" && menuChoice != "Q")
            {
                Console.WriteLine("Please enter F (Find a movie), L (Load a JSON file), or Q (Quit)");
                menuChoice = Console.ReadLine().ToUpper().Trim();
            }
            
            switch(menuChoice)
            {
                case "F":
                    // Main Search Program
                    MainSearch();
                    break;
                case "L":
                    // Load JSON file
                    UserLoadMovieList();
                    break;
                case "Q":
                    break;
            }
        }

        private static void MainSearch()
        {
                List<Movie> searchedMovieList = new List<Movie>();

                Console.Clear();    
                
                // Ask for movie to search for
                var movieChoice = GetSearchFromUser();

                // Perform serch on OMDB
                Search[] omdbSearchResults = WebInteraction.SearchOmdbByString(movieChoice);

                // Put results into a list of movies with titles, directors, ratings, etc
                if (omdbSearchResults == null)
                {
                    // Getting null is usually a "too many results" error, so we'll take what we get here
                    OmdbResult newOmdbResults = WebInteraction.SearchOmdbForTitle(movieChoice);
                    Movie movie = new Movie
                    {
                        Title = newOmdbResults.Title,
                        ImdbId = newOmdbResults.ImdbId, 
                        Year = newOmdbResults.Year,
                        Poster = newOmdbResults.Poster,
                        Actors = newOmdbResults.Actors,
                        Director = newOmdbResults.Director,
                        ImdbRating = newOmdbResults.ImdbRating,
                        Metascore = newOmdbResults.Metascore,
                        Plot = newOmdbResults.Plot,
                        Rated =  newOmdbResults.Rated,
                        Ratings = newOmdbResults.Ratings,
                    };
                    searchedMovieList.Add(movie);
                }
                else
                {
                    searchedMovieList = MovieInteraction.CreateListOfOmdbResults(omdbSearchResults);
                }

                Console.Clear();
                Console.WriteLine($"You searched for {movieChoice}.\r\n\r\n");
                ShowAndPickOmdbSearch(searchedMovieList);
        }

        // Gets something to search for
        private static string GetSearchFromUser()
        {
            Console.WriteLine(_banner + "Type the name of the movie you want to search for:");
            var input = Console.ReadLine().Trim();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please type something.");
                input = Console.ReadLine().Trim();
            }
            return input;
        }

        // Takes list of movies and displays results to user, prompts for selection, and returns IMDB number for movie
        private static Movie DisplayResultsReturnSelection(List<Movie> movies)
        {
            Console.WriteLine(DisplayMovieInfo(movies));
            int userSelection;
            userSelection =  UserPicksResult();
            while (userSelection >= movies.Count || userSelection < 0)
            {
                Console.WriteLine("Selection is out of range, please pick again");
                userSelection =  UserPicksResult();
            }
            return movies[userSelection];
        }

        // Helper method to get and return user input to select proper result
        // Overdoing it with modularization?
        private static int UserPicksResult()
        {
            int midNumber;
            int userSelect = -1;
            Console.WriteLine("Please type the number of the result you'd like to select:");
            var userNumber = Console.ReadLine();
            while (!int.TryParse(userNumber, out midNumber))
            {
                Console.WriteLine("Please input a number");
                userNumber = Console.ReadLine();
            }
            userSelect = midNumber - 1;
            return userSelect;
        }

        private static List<Movie> ShowAndPickOmdbSearch(List<Movie> movies)
        {
            // Display results and ask for user to select one
                var selectedMovie = DisplayResultsReturnSelection(movies);
                // Search for streaming providers for selection using Utelly
                var utellyResult = WebInteraction.SearchUtellyById(selectedMovie.ImdbId);
                // Put streaming locations on our selected movie object
                selectedMovie.Locations = utellyResult.collection.Locations;

                Console.Clear();
                Console.WriteLine($"Here are your results for {selectedMovie.Title}:"); 
                Console.WriteLine(DisplayStreamingLocations(selectedMovie));

                // Search again, save results to file, return to results, or exit?
                Console.WriteLine("\r\nType \"F\" to find another movie or show, type \"S\" to save result to a movie list file, type \"L\" to load a previous list.\r\nType \"R\" to return to results.\r\nType \"Q\" to quit.");
                var menuChoice = Console.ReadLine().ToUpper().Trim();
                while (menuChoice != "F" && menuChoice != "S"  && menuChoice != "L" && menuChoice != "R" &&menuChoice != "Q")
                {
                    Console.WriteLine("Please enter F (Find another), S (Save result to a list), L (Load list from file), R (Return to results), or Q (quit)");
                    menuChoice = Console.ReadLine().ToUpper().Trim();
                }
                switch (menuChoice)
                {
                    case "F":
                        MainSearch();
                        break;
                    case "S":
                        UserSaveMovie(selectedMovie);
                        MainMenu();
                        break;
                    case "L":
                        UserLoadMovieList();
                        break;
                    case "R":
                        Console.Clear();
                        ShowAndPickOmdbSearch(movies);
                        break;
                    case "Q":
                        break;
                }

            return movies;
        }

        // Give the list of addresses to find the selected media
        private static string DisplayStreamingLocations(Movie resultToDisplay)
        {
            if (resultToDisplay.Locations.Length == 0 )
            {
                return "No streams found.";
            }
            else
            {
                StringBuilder display = new StringBuilder();
                foreach (var location in resultToDisplay.Locations)
                {
                    if (location.Country[0] == "us")
                    {
                        // I don't know why "IVAUS" is added to the name of providers but I don't like it
                        display.AppendFormat($"\r\n\t{location.DisplayName.TrimEnd(new char[] {'I', 'V', 'A', 'U', 'S'})}\r\n\t{location.Url}\r\n");
                    }
                } 
                
                return display.ToString();
            }
        }

        private static string DisplayMovieInfo(List<Movie> movies)
        {
            StringBuilder display = new StringBuilder();

            for (int i = 0; i < movies.Count; i++)
            {
                display.AppendFormat($"[{i+1}]: {movies[i].Title}");
                // Displaying results as "N/A" is ugly and useless
                if (movies[i].Director != "N/A")
                {
                    display.AppendFormat($", directed by {movies[i].Director}");
                }
                display.AppendFormat($", {movies[i].Year}.");
                if (movies[i].Rated != "N/A")
                {
                    display.AppendFormat($" {movies[i].Rated}");
                }
                if (movies[i].Plot != "N/A")
                {
                    display.AppendFormat($" \r\n\t{movies[i].Plot}");
                }
                display.AppendFormat($"\r\n\tStarring: {movies[i].Actors}\r\n\tCritic Ratings: ");


                foreach (var rating in movies[i].Ratings)
                {
                    display.AppendFormat($"\r\n\t\t{rating.Source}, {rating.Value}");
                }

                display.Append("\r\n\r\n");

                if (movies[i].Locations != null)
                {
                    display.Append("Streaming Locations:");
                    display.Append(DisplayStreamingLocations(movies[i]));
                    display.Append("\r\n--------------------------------------------------------------------\r\n\r\n");
                }
            }
            return display.ToString();
        }

        private static void UserSaveMovie(Movie saveThis)
        {
            Console.WriteLine("Please type filename to save to using (one word, no extension) or \"Q\" to return to menu.");
            var fileName = Console.ReadLine().Trim();

            if (fileName.ToUpper() == "Q") { return;}                
            while(string.IsNullOrWhiteSpace(fileName) || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
            {
                if (fileName.ToUpper() == "Q") { return; }
                Console.WriteLine("Please enter a valid filename with no extension or \"Q\" to return");
                fileName = Console.ReadLine().Trim();
            }
            if (!File.Exists(fileName + ".json") && fileName != "Q")
            {
                MovieInteraction.SaveMovieToFile(saveThis, (fileName + ".json"));
            }
            else
            {
                if (fileName.ToUpper() == "Q")
                { return;}
                MovieInteraction.AddMovieToFile(saveThis, (fileName + ".json"));
            }
        }

        private static void UserSaveMovieList(List<Movie> movies)
        {

        }

        private static void UserLoadMovieList()
        {
            Console.WriteLine("Please type filename to load (no extension) or \"Q\" to return to menu");
            var fileName = Console.ReadLine().Trim();

            if (fileName.ToUpper() == "Q") { MainMenu();}

            while (!File.Exists(fileName + ".json"))
            {
                if (fileName.ToUpper() == "Q") { MainMenu();}
                Console.WriteLine("File does not exist, please try again or type \"Q\" to return to menu.");
                fileName = Console.ReadLine().Trim();
                while(string.IsNullOrWhiteSpace(fileName) || (fileName + ".json").IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    if (fileName.ToUpper() == "Q") { MainMenu();}
                    Console.WriteLine("Please enter a valid filename with no extension or \"Q\" to return to menu");
                    fileName = Console.ReadLine().Trim();
                }
            }
            var movies = MovieInteraction.LoadMovieList(fileName + ".json");

            Console.Clear();
            Console.WriteLine(DisplayMovieInfo(movies));

            Console.WriteLine("\r\nType \"S\" to save movie to a different file, type \"D\" to Delete movie from list, type \"F\" to find a movie or show, type \"L\" to load another file.\r\nType \"Q\" to quit.");
            var menuChoice = Console.ReadLine().ToUpper().Trim();
            while (menuChoice != "S" && menuChoice != "D" && menuChoice != "F" && menuChoice != "L" && menuChoice != "Q")
            {
                Console.WriteLine("Please enter S (Save movie to different file), D (Delete a movie), F (Find a movie), L (Load a file), or Q (Quit)");
                menuChoice = Console.ReadLine().ToUpper().Trim();
            }
            switch (menuChoice)
            {
    //TODO: MAKE FILE STUFF WORK RIGHT
                case "S":
                    int i = UserPicksResult();
                    UserSaveMovie(movies[i]);
                    break;
                case "D":
                    i = UserPicksResult();
                    movies.RemoveAt(i);
                    MovieInteraction.SaveMoviesToList(movies, fileName + ".json");
                    break;
                case "F":
                    MainSearch();
                    break;
                case "L":
                    UserLoadMovieList();
                    break;
                case "Q":
                    break;
            }

        }        
    }
}