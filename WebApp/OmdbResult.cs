using System;
using Newtonsoft.Json;

namespace NowPlaying
{

    //Parses when the search type is Title
    public partial class OmdbResult
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Year")]
        public string Year { get; set; }

        [JsonProperty("Rated")]
        public string Rated { get; set; }

        [JsonProperty("Released")]
        public string Released { get; set; }

        [JsonProperty("Runtime")]
        public string Runtime { get; set; }

        [JsonProperty("Genre")]
        public string Genre { get; set; }

        [JsonProperty("Director")]
        public string Director { get; set; }

        [JsonProperty("Writer")]
        public string Writer { get; set; }

        [JsonProperty("Actors")]
        public string Actors { get; set; }

        [JsonProperty("Plot")]
        public string Plot { get; set; }

        [JsonProperty("Language")]
        public string Language { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("Awards")]
        public string Awards { get; set; }

        [JsonProperty("Poster")]
        public Uri Poster { get; set; }

        [JsonProperty("Ratings")]
        public Rating[] Ratings { get; set; }

        [JsonProperty("Metascore")]
        public string Metascore { get; set; }

        [JsonProperty("imdbRating")]
        public string ImdbRating { get; set; }

        [JsonProperty("imdbVotes")]
        public string ImdbVotes { get; set; }

        [JsonProperty("imdbID")]
        public string ImdbId { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("totalSeasons")]
        public string TotalSeasons { get; set; }

        [JsonProperty("Response")]
        public string Response { get; set; }
    }

    public partial class Rating
    {
        [JsonProperty("Source")]
        public string Source { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }

//Parses when the search is String and returns multiple values in array
    public partial class OmdbSearchByString
    {
        [JsonProperty("Search")]
        public Search[] Search { get; set; }

        [JsonProperty("totalResults")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long TotalResults { get; set; }

        [JsonProperty("Response")]
        public string Response { get; set; }
    }

    public partial class Search
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Year")]
        public string Year { get; set; }

        [JsonProperty("imdbID")]
        public string ImdbId { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Poster")]
        public Uri Poster { get; set; }
    }

    public enum TypeEnum { Movie };

    public partial class OmdbSearchByString
    {
        public static OmdbSearchByString FromJson(string json) => JsonConvert.DeserializeObject<OmdbSearchByString>(json, NowPlaying.Converter.Settings);
    }

    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "movie")
            {
                return TypeEnum.Movie;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            if (value == TypeEnum.Movie)
            {
                serializer.Serialize(writer, "movie");
                return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }

    public partial class OmdbResult
    {
        public static OmdbResult FromJson(string json) => JsonConvert.DeserializeObject<OmdbResult>(json, NowPlaying.Converter.Settings);
    } 
}
