using System;
using System.IO;

namespace NowPlaying
{
    // This class will hold the methods for calling and working with APIs. Fingers crossed on all this..
    class WebInteraction
    {
        
        public static void PrintUtellyKey()
        {
            Console.WriteLine(WebInteraction.GetUtellyKey());
        }

        private static string GetUtellyKey()
        {
            string key;
            using (StreamReader utellykey = new StreamReader("UtellyKey.txt") )
            {
               key = utellykey.ReadToEnd();
            }
            return key;
        }
    }
}