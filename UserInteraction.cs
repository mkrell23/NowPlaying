using System;
using System.Collections.Generic;
using System.Text;

namespace NowPlaying
{
    //This holds the code that the user interface is built from.
    class UserInteraction
    {
        // DisplayWelcome seems to be surperfluous. Delete?
        public static void DisplayWelcome()
        {
            Console.WriteLine(@"
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

Find what streaming provider is serving a movie or show!

Press ESC to quit
Press any other key to start");

        }

        // Gets something to search for
        public static string GetSearchFromUser()
        {
            Console.WriteLine(@"
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

Type the name of the movie you want to search for:
");
            return Console.ReadLine();
        }

        // Takes list of movies and displays results to user, prompts for selection, and returns IMDB number for movie
        public static string DisplayAndReturnSelection(string movieChoice, List<Movie> userSearch)
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
            return userSearch[userSelection].ImdbId;
        }

        // Overload of method taking the Utelly Result[] instead of the earlier generated list of movies
        public static string DisplayAndReturnSelection(string movieChoice, Result[] userSearch)
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
        public static int UserPicksResult()
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

        // Give the final list of addresses to find the selected media
        public static void DisplayStreamingLocations(UtellyResultById.Collection resultToDisplay)
        {
            Console.Clear();
            if (resultToDisplay.Locations.Count == 0 )
            {
                Console.WriteLine("No results streaming");
            }
            else
            {
                StringBuilder display = new StringBuilder($"Here are your results for {resultToDisplay.Name}:");
                foreach (var location in resultToDisplay.Locations)
                {
                    display.AppendFormat($"\r\n\r\n\t{location.DisplayName}\r\n\t{location.Url}");
                } 
                Console.WriteLine(display);
            }
        }        
    }
}