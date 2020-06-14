using System;
using System.Collections.Generic;

namespace NowPlaying
{
    public class Movie
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; } 
        public string Plot { get; set; }
        public string Director { get; set; } 
        public string Actors { get; set; }
        public Uri Poster { get; set; }
        public List<Rating> Ratings { get; set; }
        public string Metascore { get; set; }
        public string ImdbRating { get; set; }
        public string ImdbVotes { get; set; }
        public string ImdbId { get; set; }
        public object Picture { get; set; }
        public Location[] Locations { get; set; }
        public string Provider { get; set; }
        public Imdb Imdb { get; set; }

    }
}