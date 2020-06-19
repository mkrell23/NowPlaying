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

        public static void MainMenu()
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
        }

        public static void DisplayResults(string movieChoice, List<Movie> userSearch)
        {
            Console.Clear();
            StringBuilder display = new StringBuilder($"You searched for {movieChoice}.\r\n\r\nResults:");

            for (int i = 0; i < userSearch.Count; i++)
            {
                display.AppendFormat($"\r\n\r\n[{i+1}]: {userSearch[i].Title}, directed by {userSearch[i].Director}. MPAA Rating: {userSearch[i].Rated}\r\n\t{userSearch[i].Plot}\r\n\tStarring: {userSearch[i].Actors}\r\n\tCritic Ratings: ");
                // Stricken line, after director: , in {userSearch[i].Year}
                foreach (var rating in userSearch[i].Ratings)
                {
                    display.AppendFormat($"\r\n\t\t{rating.Source}, {rating.Value}");
                    
                }
            }

            Console.WriteLine(display + "\r\n\r\n");
        }
    }
}