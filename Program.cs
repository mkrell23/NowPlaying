using System;
using Newtonsoft.Json;

namespace NowPlaying
{
    class Program
    {
        static void Main(string[] args)
        {
           var result = WebInteraction.SearchUtelly("bojack");
            Console.WriteLine("Hello World!");
            WebInteraction.PrintUtellyKey();

            string bojack = JsonConvert.DeserializeObject(result.Content).ToString();

          WebInteraction.SaveMovieToFile(bojack, "bojack.json") ; 
        }
    }
}
