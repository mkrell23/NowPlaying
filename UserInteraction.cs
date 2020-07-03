using System;
using System.Collections.Generic;
using System.Text;

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
            Console.WriteLine(_banner + "\tFind what streaming provider is serving a movie or show!\r\n\r\n\t\tGet critic reviews!\r\n\r\n\t\t\tPress any key to start");
        }

        public static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine(_banner + "Type \"1\" to search for a movie.\r\nType \"2\" to load from a JSON file.\r\nType \"Q\" to quit.");
            var menuChoice = Console.ReadLine().ToUpper().Trim();
            while (menuChoice != "1" && menuChoice != "2" && menuChoice != "Q")
            {
                Console.WriteLine("Please enter 1 (search), 2 (load a JSON file), or Q (quit)");
                menuChoice = Console.ReadLine().ToUpper().Trim();
            }
            
            switch(menuChoice)
            {
                case "1":
                    // Main Search Program
                    MainSearch();
                    break;
                case "2":
                    // Load JSON file
                    UserLoadMovie();
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
                Search[] OmdbSearchResults = WebInteraction.SearchOmdbByString(movieChoice);
                // Put results into a list of movies with titles, directors, ratings, etc
                searchedMovieList = MovieInteraction.CreateOmdbListOfResults(OmdbSearchResults);

                // Display results and ask for user to select one
                var selectedMovie = DisplayAndReturnSelection(movieChoice, searchedMovieList);
                // Search for and display streaming providers for selection using Utelly
                var oneToStream = WebInteraction.SearchUtellyById(selectedMovie.ImdbId);
                selectedMovie.Locations = oneToStream.collection.Locations;

                Console.Clear();
                Console.WriteLine($"Here are your results for {selectedMovie.Title}:"); 
                DisplayStreamingLocations(selectedMovie);

                // Search again, save results to file, or exit?
                Console.WriteLine("\r\n\r\nWould you like to search for another?\r\nType \"1\" to search again, type \"2\" to save result to JSON file, type \"3\" to load a previous result.\r\nType \"Q\" to quit.");
                var menuChoice = Console.ReadLine().ToUpper().Trim();
                while (menuChoice != "1" && menuChoice != "2"  && menuChoice != "3" && menuChoice != "Q")
                {
                    Console.WriteLine("Please enter 1 (search again), 2 (save result to JSON file), or Q (quit)");
                    menuChoice = Console.ReadLine().ToUpper().Trim();
                }
                switch (menuChoice)
                {
                    case "1":
                        MainSearch();
                        break;
                    case "2":
                        UserSaveMovie(selectedMovie);
                        MainMenu();
                        break;
                    case "3":
                        UserLoadMovie();
                        break;
                    case "Q":
                        break;
                }
        }

        // Gets something to search for
        private static string GetSearchFromUser()
        {
            Console.WriteLine(_banner + "Type the name of the movie you want to search for:\r\n");
            var input = Console.ReadLine().Trim();
            if (input == "" || input == null)
            {
                Console.WriteLine("Please type something.\r\n");
                input = Console.ReadLine().Trim();
            }
            return input;
        }

        // Takes list of movies and displays results to user, prompts for selection, and returns IMDB number for movie
        private static Movie DisplayAndReturnSelection(string movieChoice, List<Movie> userSearch)
        {
            Console.Clear();
            StringBuilder display = new StringBuilder($"You searched for {movieChoice}.");

            for (int i = 0; i < userSearch.Count; i++)
            {
                display.AppendFormat($"\r\n\r\n[{i+1}]: {userSearch[i].Title}");
                // Displaying results as "N/A" is ugly and useless
                if (userSearch[i].Director != "N/A")
                {
                    display.AppendFormat($", directed by {userSearch[i].Director}");
                }
                display.AppendFormat($", {userSearch[i].Year}.");
                if (userSearch[i].Rated != "N/A")
                {
                    display.AppendFormat($" {userSearch[i].Rated}");
                }
                if (userSearch[i].Plot != "N/A")
                {
                    display.AppendFormat($" \r\n\t{userSearch[i].Plot}");
                }
                display.AppendFormat($"\r\n\tStarring: {userSearch[i].Actors}\r\n\tCritic Ratings: ");


                foreach (var rating in userSearch[i].Ratings)
                {
                    display.AppendFormat($"\r\n\t\t{rating.Source}, {rating.Value}");
                }
            }
            Console.WriteLine(display + "\r\n\r\n");   
            int userSelection =  UserPicksResult();
            while (userSelection >= userSearch.Count || userSelection < 0)
            {
                Console.WriteLine("Selection is out of range, please pick again");
                userSelection =  UserPicksResult();
            }
            return userSearch[userSelection];
        }

        // Overload of method taking the Utelly Result[] instead of the earlier generated list of movies
        // Not needed in current version of program, but useful in checking things are working
        private static string DisplayAndReturnSelection(string movieChoice, Result[] userSearch)
        {
            Console.Clear();
            StringBuilder display = new StringBuilder($"You searched for {movieChoice}.");

            for (int i = 0; i < userSearch.Length; i++)
            {
                display.AppendFormat($"\r\n\r\n[{i+1}]: {userSearch[i].Name}");
            }

            Console.WriteLine(display + "\r\n\r\n");
    
            int userSelection =  UserPicksResult();

            return userSearch[userSelection].ExternalIds.Imdb.Id;
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

        // Give the list of addresses to find the selected media
        private static void DisplayStreamingLocations(Movie resultToDisplay)
        {
            if (resultToDisplay.Locations.Length == 0 )
            {
                Console.WriteLine("No results streaming");
            }
            else
            {
                StringBuilder display = new StringBuilder();
                foreach (var location in resultToDisplay.Locations)
                {
                    // I don't know why "IVAUS" is added to the name of providers but I don't like it
                    display.AppendFormat($"\r\n\r\n\t{location.DisplayName.TrimEnd(new char[] {'I', 'V', 'A', 'U', 'S'})}\r\n\t{location.Url}");
                } 
                Console.WriteLine(display);
            }
        }

        private static void DisplayMovieInfo(Movie movie)
        {
            StringBuilder display = new StringBuilder();
            display.AppendFormat($"{movie.Title}");
            // Displaying results as "N/A" is ugly and useless
            if (movie.Director != "N/A")
            {
                display.AppendFormat($", directed by {movie.Director}");
            }
            display.AppendFormat($", {movie.Year}.");
            if (movie.Rated != "N/A")
            {
                display.AppendFormat($" {movie.Rated}");
            }
            if (movie.Plot != "N/A")
            {
                display.AppendFormat($"\r\n\t{movie.Plot}");
            }
            display.AppendFormat($"\r\n\r\n\tStarring: {movie.Actors}\r\n\tCritic Ratings: ");

            foreach (var rating in movie.Ratings)
            {
                display.AppendFormat($"\r\n\r\n\t\t{rating.Source}, {rating.Value}");
            }

            Console.WriteLine(display);
        }

        private static void UserSaveMovie(Movie saveThis)
        {
            Console.WriteLine("Please type filename to save to using (one word, no extension)");
            var userFileName = Console.ReadLine();
            var realFileName = userFileName + ".json";
            MovieInteraction.SaveMovieToFile(saveThis, realFileName);
        }

        private static void UserLoadMovie()
        {
            Console.WriteLine("Please type filename to load (no extension)");
            var userFileName = Console.ReadLine();
            var fileName = userFileName + ".json";
            var result = MovieInteraction.LoadMovieFromFile(fileName);

            Console.Clear();
            DisplayMovieInfo(result);
            Console.WriteLine("\r\n\r\nStreaming Locations:");
            DisplayStreamingLocations(result);

            Console.WriteLine("\r\n\r\nType \"1\" to search for a movie, type \"2\" to load another file.\r\nType \"Q\" to quit.");
            var menuChoice = Console.ReadLine().ToUpper().Trim();
            while (menuChoice != "1" && menuChoice != "2"  && menuChoice != "3" && menuChoice != "Q")
            {
                Console.WriteLine("Please enter 1 (search), 2 (load a file), or Q (quit)");
                menuChoice = Console.ReadLine().ToUpper().Trim();
            }
            switch (menuChoice)
            {
                case "1":
                    MainSearch();
                    break;
                case "2":
                    UserLoadMovie();
                    break;
                case "Q":
                    break;
            }

        }        
    }
}