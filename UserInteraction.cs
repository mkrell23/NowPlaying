using System;
using System.Collections.Generic;
using System.Text;

namespace NowPlaying
{
    //This holds the code that the user interface is built from.
    class UserInteraction
    {
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

Find what streaming provider is serving a movie!

Press any key to start
Press the ESC key at any tme to exit");

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

        // Displays results to user, prompts for selection, and returns IMDB number for movie
        public static string DisplayAndReturnSelection(string movieChoice, List<Movie> userSearch)
        {
            Console.Clear();
            StringBuilder display = new StringBuilder($"You searched for {movieChoice}.");

            for (int i = 0; i < userSearch.Count; i++)
            {
                display.AppendFormat($"\r\n\r\n[{i+1}]: {userSearch[i].Title}");
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
    
            int userSelection =  UserPicksResult(userSearch);

            return userSearch[userSelection].ImdbId;
        }

        // Helper method to get and return user input to select proper result
        public static int UserPicksResult(List<Movie> userSearch)
        {
            int userSelect = -1;
            int userNumber;
            Console.WriteLine("Please type the number of the result you'd like to select:");
            try
            {
                    userNumber = int.Parse(Console.ReadLine());
                    userSelect = userNumber - 1;
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
            // if (int.TryParse(rawNumber, out userNumber))
            // {
            //     userSelect = userNumber - 1;
            // }
            
            return userSelect;
        }


    }
}