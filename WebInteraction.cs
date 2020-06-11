using System;
using System.IO;

namespace NowPlaying
{
    // This class will hold the methods for calling and working with APIs. Fingers crossed on all this..
    class WebInteraction
    {
        
        public static void PrintUtellyKey()
        {
            string key;

            using (StreamReader utellykey = new StreamReader("UtellyKey.txt") )
            {
               key = utellykey.ReadToEnd();
            }

            Console.WriteLine($"The key is {key}");
            
        }
    }
}