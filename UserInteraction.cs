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
        private static string _extension = ".json";

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
              
            // Gets something to search for
            Console.WriteLine(_banner + "Type the name of the movie or show you want to search for:");
            var input = Console.ReadLine().Trim();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Please type something.");
                input = Console.ReadLine().Trim();
            }         
            var movieChoice = input;

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

// TODO: Allow user to save result from OMDB list before Utelly search?
// What would that flow look like? Would that be normal?
        private static List<Movie> ShowAndPickOmdbSearch(List<Movie> movies)
        {
            // Display results and ask for user to select one
            if(movies[0].Title == null)
            {
                Console.WriteLine("No results found");
                Console.WriteLine("Press any key to return to menu");
                Console.ReadKey();
                MainMenu();                
            }
            Console.WriteLine(DisplayMovieInfo(movies));
            int userSelection;
            userSelection =  UserPicksArray();
            while (userSelection >= movies.Count || userSelection < 0)
            {
                Console.WriteLine("Selection is out of range, please pick again");
                userSelection =  UserPicksArray();
            }
            var selectedMovie = movies[userSelection];
        
            // Search for streaming providers for selection using Utelly
            var utellyResult = WebInteraction.SearchUtellyById(selectedMovie.ImdbId);
            // Put streaming locations on our selected movie object
            selectedMovie.Locations = utellyResult.collection.Locations;

            Console.Clear();
            Console.WriteLine($"Here are your results for {selectedMovie.Title}:\r\n\r\n"); 
            Console.WriteLine(DisplayStreamingLocations(selectedMovie));

            // Search again, save results to file, return to results, or exit?
            Console.WriteLine("\r\nType \"F\" to find another movie or show, type \"S\" to save result to a list file, type \"L\" to load a previous list.\r\nType \"R\" to return to results, and type \"Q\" to quit.");
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
                    Console.WriteLine("Press any key to go back to main menu");
                    Console.ReadKey();
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

        // Displays the list of movies in a "nice" way.
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

        // Give the list of addresses to find the selected media
        private static string DisplayStreamingLocations(Movie movie)
        {
            if (movie.Locations == null || movie.Locations.Length == 0)
            {
                return "No streams found.";
            }
            else
            {
                StringBuilder display = new StringBuilder();
                foreach (var location in movie.Locations)
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

        private static void UserSaveMovie(Movie movie)
        {
            Console.WriteLine("Please type filename to save to (one word, no extension) or \"Q\" to return to menu.");
            var fileName = Console.ReadLine().Trim();

            if (fileName.ToUpper() == "Q") { return;}                
            while(string.IsNullOrWhiteSpace(fileName) || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
            {
                if (fileName.ToUpper() == "Q") { return; }
                Console.WriteLine("Please enter a valid filename with no extension or \"Q\" to return");
                fileName = Console.ReadLine().Trim();
            }
            if (!File.Exists(fileName + _extension) && fileName != "Q")
            {
                MovieInteraction.SaveMovieToFile(movie, (fileName + _extension));
            }
            else
            {
                if (fileName.ToUpper() == "Q")
                { return;}
                MovieInteraction.AddMovieToFile(movie, (fileName + _extension));
            }
        }

        private static void UserSaveList(List<Movie> movies)
        {
            Console.WriteLine("Please type filename to save to (one word, no extension) or \"Q\" to return to menu.");
            var fileName = Console.ReadLine().Trim();

            if (fileName.ToUpper() == "Q") { return;}                
            while(string.IsNullOrWhiteSpace(fileName) || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
            {
                if (fileName.ToUpper() == "Q") { return; }
                Console.WriteLine("Please enter a valid filename with no extension or \"Q\" to return");
                fileName = Console.ReadLine().Trim();
            }
            if (!File.Exists(fileName + _extension) && fileName != "Q")
            {
                MovieInteraction.SaveMoviesToList(movies, (fileName + _extension));
            }
            else
            {
                if (fileName.ToUpper() == "Q")
                { return;}
                MovieInteraction.AddMoviesToList(movies, (fileName + _extension));
            }
        }

        private static void UserLoadMovieList()
        {
            Console.WriteLine("Please type filename to load (no extension) or \"Q\" to return to menu");
            var fileName = Console.ReadLine().Trim();

            if (fileName.ToUpper() == "Q") { MainMenu();}

            while (!File.Exists(fileName + _extension))
            {
                if (fileName.ToUpper() == "Q") { MainMenu();}
                Console.WriteLine("File does not exist, please try again or type \"Q\" to return to menu.");
                fileName = Console.ReadLine().Trim();
                while(string.IsNullOrWhiteSpace(fileName) || (fileName + _extension).IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    if (fileName.ToUpper() == "Q") { MainMenu();}
                    Console.WriteLine("Please enter a valid filename with no extension or \"Q\" to return to menu");
                    fileName = Console.ReadLine().Trim();
                }
            }
            var movies = MovieInteraction.LoadMovieList(fileName + _extension);

            UserListInteraction(movies, fileName);
        }

        private static void UserListInteraction(List<Movie> movies, string fileName)
        {
            Console.Clear();

            Console.WriteLine($"File Loaded: {fileName}.json\r\n\r\n");
            Console.WriteLine(DisplayMovieInfo(movies));

            Console.WriteLine("\r\nType \"S\" to save movie to a different file, type \"C\" to Copy list to another file, type \"D\" to Delete movie from list, or type \"R\" to Reload streaming providers for a result.\r\nType \"F\" to find a new movie or show, type \"L\" to load another file.\r\nType \"Q\" to quit.");
            var menuChoice = Console.ReadLine().ToUpper().Trim();
            while (menuChoice != "S"  && menuChoice != "C" && menuChoice != "D" && menuChoice != "R" && menuChoice != "F" && menuChoice != "L" && menuChoice != "Q")
            {
                Console.WriteLine("Please enter S (Save movie to different file), C (Copy list to another list), D (Delete a movie), R (Reload providers), F (Find a movie), L (Load a file), or Q (Quit)");
                menuChoice = Console.ReadLine().ToUpper().Trim();
            }
            switch (menuChoice)
            {
                case "S":
                    int i = UserPicksArray();
                    while (i >= movies.Count || i < 0)
                    {
                        Console.WriteLine("Selection is out of range, please pick again");
                        i =  UserPicksArray();
                    }
                    UserSaveMovie(movies[i]);
                    Console.WriteLine("List may need to be reloaded. Press any key to continue");
                    Console.ReadKey();
                    UserListInteraction(movies, fileName);
                    break;
                case "C":
                    UserSaveList(movies);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    UserListInteraction(movies, fileName);
                    break;
                case "D":
                    i = UserPicksArray();
                    while (i >= movies.Count || i < 0)
                    {
                        Console.WriteLine("Selection is out of range, please pick again");
                        i =  UserPicksArray();
                    }
                    movies.RemoveAt(i);
                    MovieInteraction.SaveMoviesToList(movies, fileName + _extension);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    UserListInteraction(movies, fileName);
                    break;
                case "R":
                    i = UserPicksArray();
                    while (i >= movies.Count || i < 0)
                    {
                        Console.WriteLine("Selection is out of range, please pick again");
                        i =  UserPicksArray();
                    }
                    var results = WebInteraction.SearchUtellyById(movies[i].ImdbId);
                    movies[i].Locations = results.collection.Locations;
                    MovieInteraction.SaveMoviesToList(movies, fileName + _extension);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    UserListInteraction(movies, fileName);
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

        

        // Helper method to get and return user input to select proper result
        // Overdoing it with modularization? References say no.
        private static int UserPicksArray()
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
    }
}