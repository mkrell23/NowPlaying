using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NowPlaying
{
    // Parses when search is by ID
    public partial class UtellyResultById
    {
        public class Imdb
        {
                public string Url { get; set; }
                public string Id { get; set; }
        }

        public class SourceIds
        {
            public Imdb Imdb { get; set; }
        }

        public class Collection
        {
                public string Id { get; set; }
                public string Picture { get; set; }
                public string Name { get; set; }
                public Location[] Locations { get; set; }
                public string Provider { get; set; }
                public int Weight { get; set; }
                public SourceIds Source_ids { get; set; }
        }

        public Collection collection { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public int Status_code { get; set; }
        public string Variant { get; set; }
    }

// Parses when search is by title
    public partial class UtellyResult
    {
        [JsonProperty("results")]
        public Result[] Results { get; set; }

        [JsonProperty("updated")]
        public DateTimeOffset Updated { get; set; }

        [JsonProperty("term")]
        public string Term { get; set; }

        [JsonProperty("status_code")]
        public long StatusCode { get; set; }

        [JsonProperty("variant")]
        public string Variant { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("picture")]
        public object Picture { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("locations")]
        public Location[] Locations { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("external_ids")]
        public ExternalIds ExternalIds { get; set; }
    }

    public partial class ExternalIds
    {
        [JsonProperty("iva_rating")]
        public object IvaRating { get; set; }

        [JsonProperty("imdb")]
        public Imdb Imdb { get; set; }

        [JsonProperty("tmdb")]
        public Imdb Tmdb { get; set; }

        [JsonProperty("wiki_data")]
        public Imdb WikiData { get; set; }

        [JsonProperty("iva")]
        public Iva Iva { get; set; }

        [JsonProperty("gracenote")]
        public object Gracenote { get; set; }

        [JsonProperty("rotten_tomatoes")]
        public object RottenTomatoes { get; set; }

        [JsonProperty("facebook")]
        public object Facebook { get; set; }
    }

    public partial class Imdb
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Iva
    {
        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Id { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("icon")]
        public object Icon { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public object Url { get; set; }
    }

    // These are all the methods to actually return something usable. Mostly auto-generated and then tweaked to actually work by me (helping or hurting?)
    public partial class UtellyResult
    {
        public static UtellyResult FromJson(string json) => JsonConvert.DeserializeObject<UtellyResult>(json, NowPlaying.Converter.Settings);
    }

    public partial class UtellyResultById
    {
        public static UtellyResultById FromJson(string json) => JsonConvert.DeserializeObject<UtellyResultById>(json, NowPlaying.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this OmdbResult self) => JsonConvert.SerializeObject(self, NowPlaying.Converter.Settings);
        public static string ToJson(this UtellyResult self) => JsonConvert.SerializeObject(self, NowPlaying.Converter.Settings);
        public static string ToJson(this UtellyResultById self) => JsonConvert.SerializeObject(self, NowPlaying.Converter.Settings);
        public static string ToJson(this OmdbSearchByString self) => JsonConvert.SerializeObject(self, NowPlaying.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
