using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NowPlaying
{
    class MovieInteraction
    {
        // Generates a list of movies/shows from OMDB results
        public static List<Movie> CreateListOfOmdbResults(Search[] results)
        {
            List<Movie> userSearch = new List<Movie>();
            foreach (var result in results)
            {
                if(result.Type != "game") // This prevents garbage results later
                {    
                    var newResults = WebInteraction.SearchOmdbForId(result.ImdbId);
                    Movie movie = new Movie
                    {
                        Title = result.Title, 
                        ImdbId = result.ImdbId, 
                        Year = result.Year,
                        Poster = result.Poster,
                        Actors = newResults.Actors,
                        Director = newResults.Director,
                        ImdbRating = newResults.ImdbRating,
                        Metascore = newResults.Metascore,
                        Plot = newResults.Plot,
                        Rated =  newResults.Rated,
                        Ratings = newResults.Ratings,
                    };
                    userSearch.Add(movie);
                    Task.Delay(1200); //OMDB does not like getting hammered
                }
            }
            return userSearch;
        }

        public static void SaveMovieToFile(Movie movie, string fileName)
        {
            List<Movie> movies = new List<Movie>{movie};
            var serializer = new JsonSerializer();
            using (StreamWriter writer = new StreamWriter(fileName))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, movies);               
            }          
        }

        public static void SaveMoviesToList(List<Movie> movies, string fileName)
        {
            var serializer = new JsonSerializer();
            using (StreamWriter writer = new StreamWriter(fileName))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, movies);               
            }
        }

        public static void AddMoviesToList(List<Movie> movies, string fileName)
        {
            var oldMovies = LoadMovieList(fileName);
            
            foreach (Movie movie in movies)
            {
                oldMovies.Add(movie);
            }
            var serializer = new JsonSerializer();
            using (StreamWriter writer = new StreamWriter(fileName))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, oldMovies);               
            }        
        }

        public static void AddMovieToFile(Movie movie, string fileName)
        {
            var oldMovies = LoadMovieList(fileName);
            oldMovies.Add(movie);
            var serializer = new JsonSerializer();
            using (StreamWriter writer = new StreamWriter(fileName))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, oldMovies);               
            }          
        }

        public static List<Movie> LoadMovieList(string fileName)
        {
            var movies = new List<Movie>();
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(fileName))
            using (var jsonReader = new JsonTextReader(reader))
            {

                movies = serializer.Deserialize<List<Movie>>(jsonReader);
                return movies;
            }
 
        }
    }
}