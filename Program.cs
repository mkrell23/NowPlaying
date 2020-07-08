using System;

namespace NowPlaying
{
    class Program
    {
        static void Main(string[] args)
        {            
            // Hello there
            UserInteraction.DisplayWelcome();
            Console.ReadKey();

            // Start the show
            UserInteraction.MainMenu();
        }
    }
}
